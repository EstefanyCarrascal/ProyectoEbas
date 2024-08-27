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
    public class AsistenciaController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");

        // GET: Asistencia
        public ActionResult Asistencia()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<Asistencia>("Asistencia");
            List<Asistencia> List = collection.Find(d => true).ToList();
            return View(List);
        }

        [HttpPost]
        public ActionResult Crear(string Detalles, string Fecha, string HoraInicio, string HoraFin)
        {
            try
            {
                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Asistencia>("Asistencia"); // Consistencia en el nombre de la colección
                var Asistencia = new Asistencia
                {
                    Detalles = Detalles,
                    Fecha = Fecha,
                    HoraInicio = HoraInicio,
                    HoraFin = HoraFin,
                };

                collection.InsertOne(Asistencia);
                return RedirectToAction("Asistencia"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Asistencia", new { mensaje = "Error al insertar la sala." });
            }

        }

        [HttpPost]
        public ActionResult Editar(string objectId, string Detalles, string Fecha, string HoraInicio, string HoraFin)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Asistencia>("Asistencia"); // Consistencia en el nombre de la colección

                var Asistencia = new Asistencia
                {
                    Id = parsedObjectId,
                    Detalles = Detalles,
                    Fecha = Fecha,
                    HoraInicio = HoraInicio,
                    HoraFin = HoraFin,
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, Asistencia);
                return RedirectToAction("Asistencia"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Asistencia", new { mensaje = ex.Message });
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
                var collection = database.GetCollection<Asistencia>("Asistencia"); // Consistencia en el nombre de la colección

                collection.DeleteOne(d => d.Id == parsedObjectId);
                return RedirectToAction("Asistencia"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Asistencia", new { mensaje = "Error al eliminar la sala." });
            }
        }
    }
}