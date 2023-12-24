using Microsoft.AspNetCore.Mvc;
using NoteProject.Core.Constants;

namespace NoteProject.Host.Base.Controllers
{
    public class BaseMvcControllers : Controller
    {
        public JsonResult SuccessJsonResult(string title, string message, dynamic data = null)
        {
            var response = ToasterResult.Success(title, message, data);

            return Json(response);
        }

        public JsonResult ErrorJsonResult(string title, string message)
        {
            var response = ToasterResult.Error(title, message);

            return Json(response);
        }
    }
}
