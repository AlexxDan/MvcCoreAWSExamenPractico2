using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSExamenPractico2.Helper
{
    public class UploadHelper
    {
        PathProvider pathProvider;

        public UploadHelper(PathProvider path)
        {
            this.pathProvider = path;
        }

        public async Task<String> UploadFileAsync(IFormFile formFile, Folders folders)
        {
            String filename = formFile.FileName;
            String path = this.pathProvider.MapPath(filename, Folders.Images);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            };
            return path;
        }
    }
}
