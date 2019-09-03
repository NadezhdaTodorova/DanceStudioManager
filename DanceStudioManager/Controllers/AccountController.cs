﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Transactions;

namespace DanceStudioManager
{
    public class AccountController : Controller
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly StudioDataAccess _studioDataAccess;
        private readonly HashPassword _hashPassword;
        private readonly SendEmail _email;
        private byte[] salt;

        public AccountController(UserDataAccess userDataAccess, StudioDataAccess studioDataAccess, HashPassword hashPassword, SendEmail email)
        {
            _userDataAccess = userDataAccess;
            _studioDataAccess = studioDataAccess;
            _hashPassword = hashPassword;
            _email = email;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                ViewBag.register = true;
                Studio newStudio = new Studio();

                int saltLength = 10;
                byte[] saltBytes = _hashPassword.GenerateRandomCryptographicBytes(saltLength);
                salt = saltBytes;
                user.Password = _hashPassword.HashWithSalt(user.Password, saltLength, SHA256.Create(), saltBytes);
                user.Salt = salt;

                newStudio.Name = user.StudioName;

                var allUsers = _userDataAccess.GetAllUsers();
                var allStudios = _studioDataAccess.GetAllStudios();

                if (allUsers.Any(x => x.Username == user.Username))
                {
                    ModelState.AddModelError(string.Empty, "User with this username already exists");
                }

                if (allUsers.Any(x => x.Email == user.Email))
                {
                    ModelState.AddModelError(string.Empty, "User with this email already exists");
                }

                if (allStudios.Any(x => x.Name == user.StudioName))
                {
                    ModelState.AddModelError(string.Empty, "This studio name already exists");
                }


                else if (ModelState.IsValid)
                {
                    try
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            _studioDataAccess.AddNewStudio(newStudio);

                            int studioId = _studioDataAccess.GetStudioId(newStudio);

                            _userDataAccess.AddNewUser(user, studioId);

                            int userId = _userDataAccess.GetUserId(user);

                            var path = Url.Action("AuthenticateLogin", "Home", new { userId = user.Id }, protocol: HttpContext.Request.Scheme);
                            string message = "Please confirm your account by clicking <a href=\"" + path + "\">here</a>";

                            _email.Send(user.Email, user, message);

                            scope.Complete();

                            return View("Views/Account/ConfirmEmail.cshtml");
                            
                        }
                    }
                    catch (ThreadAbortException ex)
                    {
                        ModelState.AddModelError(ex.Message, "");
                    }

                    
                }
            }

            return View("Views/Account/RegisterLogin.cshtml");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            ModelState.Clear();
            ViewBag.login = true;
            var allUsers = _userDataAccess.GetAllUsers();
            var userId = _userDataAccess.GetUserId(user);
            user.Id = userId;
            var userInfo = _userDataAccess.GetUserById(user.Id);

            if (!allUsers.Any(x => x.Email == user.Email))
            {
                ModelState.AddModelError(string.Empty, "User with this email doesn't exists!");
            }
            else if (!userInfo.ConfirmAccount)
            {
                ModelState.AddModelError(string.Empty, "We have send an email to your email account, please confirm it!");
            }
            else if (ModelState.IsValid)
            {
                var userEnteredPass = _hashPassword.HashWithSalt(user.Password, 10, SHA256.Create(), userInfo.Salt);

                if (userInfo.Password != userEnteredPass)
                {
                    ModelState.AddModelError("WrongPassword", "Wrong password, please try again!");
                }
                else
                {
                    try
                    {
                        _userDataAccess.SignIn(HttpContext, user.Id);
                        return RedirectToAction("Dashboard", "Studio");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("summary", ex.Message);
                        return View("Views/Account/RegisterLogin.cshtml");
                    }
                }
            }
            return View("Views/Account/RegisterLogin.cshtml");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Email = email;
                var userId = _userDataAccess.GetUserId(user);
                var userInfo = _userDataAccess.GetUserById(userId);

                var path = Url.Action("ResetPassword", "Account", null, protocol: HttpContext.Request.Scheme);
                string message = "Please confirm your email by clicking <a href=\"" + path + "\">here</a>";

                userInfo.ConfirmAccount = false;
                _userDataAccess.UpdateUser(userInfo);
                _email.Send(user.Email, user, message);

                return View("Views/Account/ConfirmEmail.cshtml");
            }
            else return RedirectToAction("ForgotPassword", "Account");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(User user)
        {
            ViewBag.login = true;
            ModelState.Clear();
            var allUsers = _userDataAccess.GetAllUsers();
            if (ModelState.IsValid)
            {
                if (!allUsers.Any(x => x.Email == user.Email))
                {
                    ModelState.AddModelError(string.Empty, "User with this email doesn't exists!");
                    return View();
                }
                else
                {
                    int saltLength = 10;
                    byte[] saltBytes = _hashPassword.GenerateRandomCryptographicBytes(saltLength);
                    salt = saltBytes;
                    var userId = _userDataAccess.GetUserId(user);
                    var userWithNewPass = _userDataAccess.GetUserById(userId);
                    userWithNewPass.Password = _hashPassword.HashWithSalt(user.Password, saltLength, SHA256.Create(), saltBytes);
                    userWithNewPass.Salt = salt;
                    userWithNewPass.ConfirmAccount = true;
                    _userDataAccess.UpdateUser(userWithNewPass);

                    ModelState.AddModelError(string.Empty, "You have successfuly changed your password, please log in!");
                    return View("Views/Account/RegisterLogin.cshtml");
                }
            }

            ModelState.AddModelError(string.Empty, "Please provide the necessary data!");
            return View();
        }
    }
}
