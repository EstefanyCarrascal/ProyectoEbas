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
    public class RegistroAsistenciaController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");

        // GET: RegistroAsistencia
        public ActionResult Registro()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<RegistroAsistencia>("RegistroAsistencia");
            List<RegistroAsistencia> List = collection.Find(d => true).ToList();
            return View(List);
        }

        [HttpPost]
        public ActionResult Crear(string Fecha, string Hora, string Asistio)
        {
            try
            {
                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<RegistroAsistencia>("RegistroAsistencia"); // Consistencia en el nombre de la colección
                var Registro = new RegistroAsistencia
                {
                   Fecha = Fecha,
                   Hora = Hora
                };

                collection.InsertOne(Registro);
                return RedirectToAction("Registro"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Registro", new { mensaje = "Error al insertar la sala." });
            }

        }
        [HttpPost]
        public ActionResult Editar(string objectId, string Fecha, string Hora, string Asistio)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<RegistroAsistencia>("RegistroAsistencia"); // Consistencia en el nombre de la colección

                var Registro = new RegistroAsistencia
                {
                    Id = parsedObjectId,
                    Fecha = Fecha,
                    Hora = Hora,
                    Asistio= Asistio, 
               
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, Registro);
                return RedirectToAction("Registro"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Registro", new { mensaje = ex.Message });
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
                var collection = database.GetCollection<RegistroAsistencia>("RegistroAsistencia"); // Consistencia en el nombre de la colección

                collection.DeleteOne(d => d.Id == parsedObjectId);
                return RedirectToAction("Registro"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Registro", new { mensaje = "Error al eliminar la sala." });
            }
        }
    }
   
}