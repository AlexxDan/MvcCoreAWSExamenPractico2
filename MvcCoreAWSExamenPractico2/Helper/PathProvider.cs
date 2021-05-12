using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSExamenPractico2.Helper
{
    public enum Folders
    {
        Images = 0
    }

    public class PathProvider
    {
        IWebHostEnvironment enviroiment;

        public PathProvider(IWebHostEnvironment environment)
        {
            this.enviroiment = environment;
        }

        public String MapPath(String filename, Folders folders)
        {
            String carparta = "";
            if (folders == Folders.Images)
            {
                carparta = "images";
            }

            String path = Path.Combine(this.enviroiment.WebRootPath, carparta, filename);
            return path;
        }
    }
}
