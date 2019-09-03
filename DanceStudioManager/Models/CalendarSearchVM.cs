﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class CalendarSearchVM
    {
        public CalendarSearchVM()
        {

        }

        [Range(2000, 2050)]
        public int Year { get; set; }
        public int Month { get; set; }
        public List<CalendarData> CalendarData { get; set; } = new List<CalendarData>();
        public Dictionary<DateTime, DayVM> Days { get; set; } = new Dictionary<DateTime, DayVM>();

        public List<SelectListItem> AllYears
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                var start = DateTime.Now.AddYears(-3).Year;
                for (int i = start; i < start + 6; i++)
                {
                    items.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                }
                return items;
            }
        }
    }
}