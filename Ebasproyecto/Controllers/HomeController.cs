using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Ebasproyecto.Model;

namespace Ebasproyecto.Controllers
{
    public class HomeController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");

        public ActionResult Index()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<Usuarios>("Usuarios");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        [HttpPost]
        public ActionResult Editar(string Nombres, string Apellidos, string Documento, string TipoDocumento, string Correo, string Sexo, string Edad, string Municipio, string Direccion, string EstadoCivil, string Telefono, string TipoPoblacion, string TipoUsuario, string Contraseña)
        {
            try
            {
                // Obtener el ID del usuario desde la sesión
                var userId = Session["UserId"]?.ToString();

                if (string.IsNullOrEmpty(userId) || !ObjectId.TryParse(userId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid User Id format or User is not authenticated.");
                }

                var database = cn.GetDatabase("Ebas");
                var collection = database.GetCollection<Usuarios>("Usuarios");

                // Encontrar el usuario en la base de datos
                var usuario = collection.Find(u => u.Id == parsedObjectId).FirstOrDefault();
                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado.");
                }

                // Actualizar los campos
                usuario.Nombres = Nombres;
                usuario.Apellidos = Apellidos;
                usuario.Documento = Documento;
                usuario.TipoDocumento = TipoDocumento;
                usuario.Correo = Correo;
                usuario.Sexo = Sexo;
                usuario.Edad = Edad;
                usuario.Municipio = Municipio;
                usuario.Direccion = Direccion;
                usuario.EstadoCivil = EstadoCivil;
                usuario.Telefono = Telefono;
                usuario.TipoPoblacion = TipoPoblacion;
                usuario.TipoUsuario = TipoUsuario;
                usuario.Contraseña = Contraseña;

                collection.ReplaceOne(d => d.Id == parsedObjectId, usuario);

                return RedirectToAction("Perfil"); // Redirigir al perfil del usuario
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Perfil", new { mensaje = ex.Message });
            }
        }

    }

}

