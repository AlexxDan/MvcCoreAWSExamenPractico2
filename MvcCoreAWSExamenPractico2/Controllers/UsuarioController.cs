using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
       
        

        public UsuarioController(ServicesS3 servicesS3,  ServicesDynamoDB servicesDB)
        {
            this.servicesS3 = servicesS3;
            
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
            List<Fotos> fotos = usuario.Foto;
            ViewData["FOTO"] = fotos;
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Usuario user, String accion, String titulo, List<IFormFile> files,List<String> fotoseliminar)
        {
            Usuario usuario = await this.servicesDB.GetUsuarioId(user.IdUsuario);
            if (accion == "delete")
            {
               
                if (usuario.Foto != null && usuario.Foto.Count != 0)
                {
                    foreach (String fotodelete in fotoseliminar)
                    {
                        await this.servicesS3.DeleteFile(fotodelete);
                    }
                    List<int> posiciones = new List<int>();
                    foreach (Fotos foto  in usuario.Foto)
                    {
                        int post = 0;
                        foreach (String fotosdelete in fotoseliminar)
                        {
                            if (foto.Imagen==fotosdelete)
                            {
                                posiciones.Add( post);
                              
                            }
                        }
                    }
                    foreach (int pos in posiciones)
                    {
                        usuario.Foto.RemoveAt(pos);
                    }
                }


            }

           if (files != null)
            {
                user.Foto = new List<Fotos>();
                user.Foto = usuario.Foto;

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
    }
}
