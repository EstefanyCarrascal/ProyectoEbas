using Ebasproyecto.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ebasproyecto.Controllers
{
    public class AprendicesController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");

        // GET: Aprendices
        public ActionResult Aprendices()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<Usuarios>("Usuarios");
            List<Usuarios> List = collection.Find(d => d.TipoUsuario == "Aprendiz").ToList(); // Filtrar por TipoUsuario = "Aprendiz"
            return View(List);
        }

        [HttpPost]
        public ActionResult Crear(string Nombres, string Apellidos, string Documento, string TipoDocumento, string Correo, string Sexo, string Edad, string Municipio, string Direccion, string EstadoCivil, string Telefono, string TipoPoblacion, string TipoUsuario, string Contraseña)
        {
            try
            {
                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Usuarios>("Usuarios"); // Consistencia en el nombre de la colección
                var Usuarios = new Usuarios
                {
                    Nombres = Nombres,
                    Apellidos = Apellidos,
                    Documento = Documento,
                    TipoDocumento = TipoDocumento,
                    Correo = Correo,
                    Sexo = Sexo,
                    Edad = Edad,
                    Municipio = Municipio,
                    Direccion = Direccion,
                    EstadoCivil = EstadoCivil,
                    Telefono = Telefono,
                    TipoPoblacion = TipoPoblacion,
                    TipoUsuario = TipoUsuario,
                    Contraseña = Contraseña
                };

                collection.InsertOne(Usuarios);
                return RedirectToAction("user"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("user", new { mensaje = "Error al insertar la sala." });
            }
        }

        [HttpPost]
        public ActionResult Editar(string objectId, string Nombres, string Apellidos, string Documento, string TipoDocumento, string Correo, string Sexo, string Edad, string Municipio, string Direccion, string EstadoCivil, string Telefono, string TipoPoblacion, string TipoUsuario, string Contraseña)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Usuarios>("Usuarios"); // Consistencia en el nombre de la colección

                var Usuarios = new Usuarios
                {
                    Id = parsedObjectId, // Asegúrate de que la propiedad Id es del tipo ObjectId
                    Nombres = Nombres,
                    Apellidos = Apellidos,
                    Documento = Documento,
                    TipoDocumento = TipoDocumento,
                    Correo = Correo,
                    Sexo = Sexo,
                    Edad = Edad,
                    Municipio = Municipio,
                    Direccion = Direccion,
                    EstadoCivil = EstadoCivil,
                    Telefono = Telefono,
                    TipoPoblacion = TipoPoblacion,
                    TipoUsuario = TipoUsuario,
                    Contraseña = Contraseña
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, Usuarios);
                return RedirectToAction("Aprendices"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Aprendices", new { mensaje = ex.Message });
            }
        }

        public ActionResult Eliminar(string objectId)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Usuarios>("Usuarios"); // Consistencia en el nombre de la colección

                collection.DeleteOne(d => d.Id == parsedObjectId);
                return RedirectToAction("Aprendices"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Aprendices", new { mensaje = "Error al eliminar la sala." });
            }
        }
    }
}