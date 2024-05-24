using FinalExam.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinalExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceAbstract _serviceAbstract;

        public HomeController(IServiceAbstract serviceAbstract)
        {
            _serviceAbstract = serviceAbstract;
        }

        public IActionResult Index()
        {
            var existService = _serviceAbstract.GetAllService();
            return View(existService);
        }

    }
}
