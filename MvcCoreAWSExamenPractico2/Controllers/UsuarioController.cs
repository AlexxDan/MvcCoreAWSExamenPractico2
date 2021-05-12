using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSExamenPractico2.Helper;
using MvcCoreAWSExamenPractico2.Models;
using MvcCoreAWSExamenPractico2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSExamenPractico2.Controllers
{
    public class UsuarioController : Controller
    {
        private ServicesS3 servicesS3;
        private ServicesDynamoDB servicesDB;
        private UploadHelper uploadHelper;
        

        public UsuarioController(ServicesS3 servicesS3, UploadHelper upload, ServicesDynamoDB servicesDB)
        {
            this.servicesS3 = servicesS3;
            this.uploadHelper = upload;
            this.servicesDB = servicesDB;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.servicesDB.GetAllUsuario());
        }

        public async Task<IActionResult> Details(int iduser)
        {
            return View(await this.servicesDB.GetUsuarioId(iduser));
        }

        public async Task<IActionResult> FileAWS(string filename)
        {
            Stream stream = await this.servicesS3.GetFile(filename);
            return File(stream, "image/jpg");
        }

        public async Task<IActionResult> Delete(int iduser)
        {
            Usuario usuario = await this.servicesDB.GetUsuarioId(iduser);
            await this.servicesDB.DeleteUser(iduser);
            if (usuario.Foto != null && usuario.Foto.Count != 0)
            {
                foreach (Fotos imagen in usuario.Foto)
                {
                    await this.servicesS3.DeleteFile(imagen.Imagen);
                }
             
            }

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Usuario user, String iamgenagree,String titulo ,List<IFormFile> files)
        {
            if (iamgenagree != null)
            {
                user.Foto = new List<Fotos>();
                
                foreach (IFormFile file in files)
                {
                    Fotos fotosnew = new Fotos();

                    fotosnew.Titulo = titulo;

                    using (MemoryStream m = new MemoryStream())
                    {
                        fotosnew.Imagen = file.FileName;
                        file.CopyTo(m);
                        await this.servicesS3.UploadFile(m, file.FileName);
                    }

                    user.Foto.Add(fotosnew);

                }
                

            }

            await this.servicesDB.CreateUsuario(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int iduser)
        {
            Usuario usuario = await this.servicesDB.GetUsuarioId(iduser);

            return View(usuario);
        }

        
    }
}
