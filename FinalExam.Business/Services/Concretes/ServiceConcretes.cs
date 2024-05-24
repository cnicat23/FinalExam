using FinalExam.Business.Exceptions;
using FinalExam.Business.Services.Abstract;
using FinalExam.Core.Models;
using FinalExam.Core.RepositoryAbstract;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam.Business.Services.Concretes
{
    public class ServiceConcretes : IServiceAbstract
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IWebHostEnvironment _env;

        public ServiceConcretes(IServiceRepository serviceRepository, IWebHostEnvironment env)
        {
            _serviceRepository = serviceRepository;
            _env = env;
        }

        public async Task AddServiceAsync(Service service)
        {
            if (service.ImageFile.ContentType != "image/png" && service.ImageFile.ContentType != "image/jpeg")
            throw new ImageContentTypeException("fayl formati duzgun deyil");

            if (service.ImageFile.Length > 2097152) throw new ImageSizeException("sekilin olcusu maksimum 2mb ola biler");

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(service.ImageFile.FileName);
            string path = _env.WebRootPath + "\\uploads\\images\\" + fileName;

            using(FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                service.ImageFile.CopyTo(fileStream);
            }

            service.ImageUrl = fileName;

            await _serviceRepository.AddAsync(service);
            await _serviceRepository.CommitAsync();
        }

        public void DeleteService(int id)
        {
            var existService = _serviceRepository.Get(x => x.Id == id);
            if (existService == null) throw new EntityNotFoundException("bele id movcud deyil");

            string path = _env.WebRootPath + "\\uploads\\images\\" + existService.ImageUrl;

            if (!File.Exists(path)) throw new Exceptions.FileNotFoundException("fayl tapilmadi");

            File.Delete(path);

            _serviceRepository.Delete(existService);
            _serviceRepository.Commit();
        }

        public List<Service> GetAllService(Func<Service, bool>? func = null)
        {
            return _serviceRepository.GetAll(func);
        }

        public Service GetService(Func<Service, bool>? func = null)
        {
            return _serviceRepository.Get(func);

        }

        public void UpdateService(int id, Service newService)
        {
            var oldService = _serviceRepository.Get(x => x.Id == id);
            if (oldService == null) throw new EntityNotFoundException("bele id movcud deyil");

            if (newService.ImageFile != null)
            {
                if (newService.ImageFile.ContentType != "image/png" && newService.ImageFile.ContentType != "image/jpeg")
                    throw new ImageContentTypeException("fayl formati duzgun deyil");

                if (newService.ImageFile.Length > 2097152) throw new ImageSizeException("sekilin olcusu maksimum 2mb ola biler");

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(newService.ImageFile.FileName);
                string path = _env.WebRootPath + "\\uploads\\images\\" + fileName;

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    newService.ImageFile.CopyTo(fileStream);
                }

                string oldPath = _env.WebRootPath + "\\uploads\\images\\" + oldService.ImageUrl;

                if (!File.Exists(oldPath)) throw new Exceptions.FileNotFoundException("fayl tapilmadi");

                File.Delete(oldPath);

                oldService.ImageUrl = fileName;
            }

            oldService.Title = newService.Title;
            oldService.Description = newService.Description;
            oldService.SubDescription = newService.SubDescription;

            _serviceRepository.Commit();
        }
    }
}
