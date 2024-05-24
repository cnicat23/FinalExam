using FinalExam.Business.Exceptions;
using FinalExam.Business.Services.Abstract;
using FinalExam.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "superAdmin")]
    public class ServiceController : Controller
    {
        private readonly IServiceAbstract _serviceAbstract;

        public ServiceController(IServiceAbstract serviceAbstract)
        {
            _serviceAbstract = serviceAbstract;
        }

        public IActionResult Index()
        {
            var existService = _serviceAbstract.GetAllService();
            return View(existService);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Service service) 
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _serviceAbstract.AddServiceAsync(service);
            }
            catch (ImageContentTypeException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (ImageSizeException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var existService = _serviceAbstract.GetService(x => x.Id == id);
            if (existService == null) return NotFound();
            
            return View(existService);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            try
            {
                 _serviceAbstract.DeleteService(id);
            }
            catch (ImageContentTypeException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (ImageSizeException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var existService = _serviceAbstract.GetService(x => x.Id == id);
            if (existService == null) return NotFound();

            return View(existService);
        }

        [HttpPost]
        public IActionResult Update(Service newService)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                 _serviceAbstract.UpdateService(newService.Id, newService);
            }
            catch (ImageContentTypeException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (ImageSizeException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (Business.Exceptions.FileNotFoundException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
