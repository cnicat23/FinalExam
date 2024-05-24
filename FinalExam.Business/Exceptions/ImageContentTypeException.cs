using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam.Business.Exceptions
{
    public class ImageContentTypeException : Exception
    {
        public ImageContentTypeException(string? message) : base(message)
        {
        }
    }
}
