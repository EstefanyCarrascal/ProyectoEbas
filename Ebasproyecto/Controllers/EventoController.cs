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
    public class EventoController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");
        // GET: Evento
        public ActionResult Evento()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<Evento>("Evento");
            List<Evento> List = collection.Find(d => true).ToList();
            return View(List);
        }

        [HttpPost]
        public ActionResult Crear(string NombreEvento, string Descripcion, string Organizador, string FechaEvento, string Tipo)
        {
            try
            {
                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Evento>("Evento"); // Consistencia en el nombre de la colección
                var Evento = new Evento
                {
                    NombreEvento = NombreEvento,
                    Descripcion = Descripcion,
                    Organizador = Organizador,
                    FechaEvento = FechaEvento,
                    Tipo = Tipo, 
                };

                collection.InsertOne(Evento);
                return RedirectToAction("Evento"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Evento", new { mensaje = "Error al insertar la sala." });
            }

        }
        [HttpPost]
        public ActionResult Editar(string objectId, string NombreEvento, string Descripcion, string Organizador, string FechaEvento)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Evento>("Evento"); // Consistencia en el nombre de la colección

                var Evento = new Evento
                {
                    Id = parsedObjectId,
                    NombreEvento = NombreEvento,
                    Descripcion = Descripcion,
                    Organizador = Organizador,
                    FechaEvento = FechaEvento,
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, Evento);
                return RedirectToAction("Evento"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Evento", new { mensaje = ex.Message });
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
                var collection = database.GetCollection<Evento>("Evento"); // Consistencia en el nombre de la colección

                collection.DeleteOne(d => d.Id == parsedObjectId);
                return RedirectToAction("Evento"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Evento", new { mensaje = "Error al eliminar la sala." });
            }
        }
    }
}