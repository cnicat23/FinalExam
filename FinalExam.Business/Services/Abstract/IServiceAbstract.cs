using FinalExam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam.Business.Services.Abstract
{
    public interface IServiceAbstract
    {
        Task AddServiceAsync(Service service);
        void DeleteService(int id);
        void UpdateService(int id, Service newService);
        Service GetService(Func<Service, bool>? func = null);
        List<Service> GetAllService(Func<Service, bool>? func = null);
    }
}
