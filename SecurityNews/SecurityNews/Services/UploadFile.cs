using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Services
{
    public class UploadFile:IUploadfile
    {


        private readonly IHostingEnvironment _appEnvironment;
        public UploadFile(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public string UploadFiles(IEnumerable<IFormFile> files, string uploadPath, string uploadthumbnailPath)
        {
            //"upload\\userimage\\normalimage\\"
            var upload = Path.Combine(_appEnvironment.WebRootPath, uploadPath);
            var filename = "";
            foreach (var file in files)
            {
                filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
                {
                    file.CopyTo(fs);
                }
            }
            //"\\upload\\userimage\\thumbnailimage\\"
            /////////تغییر سایز عکس و ذخیره
            if (uploadthumbnailPath != "")
            {
                InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer();
                img.Resize(upload + filename, _appEnvironment.WebRootPath + uploadthumbnailPath + filename);
            }
            return filename;
        }
    }
}
