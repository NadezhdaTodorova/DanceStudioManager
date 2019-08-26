using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class NameViewComponent : ViewComponent
    {
        private readonly StudioDataAccess _studioDataAccess;

        public NameViewComponent(StudioDataAccess studioDataAccess)
        {
            _studioDataAccess = studioDataAccess;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _studioDataAccess.GetStudioInfo(62);

            return await Task.FromResult((IViewComponentResult)View("~Views/Shared/_LayoutStudio", model.Name));
        }
    }
}
