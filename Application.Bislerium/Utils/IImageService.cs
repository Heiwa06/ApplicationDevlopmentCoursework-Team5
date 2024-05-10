using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bislerium.Utils
{
    public interface IImageService
    {
        public Tuple<int, string> SaveImageFile(IFormFile imageFile);
        public Task DeleteImageFile(string imageFileName);
    }
}
