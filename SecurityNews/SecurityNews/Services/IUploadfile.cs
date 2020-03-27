using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Services
{
   public interface IUploadfile
    {

        string UploadFiles(IEnumerable<IFormFile> files, string uploadPath, string uploadthumbnailPath);
    }
}
