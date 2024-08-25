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
    public class FichasController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");
        // GET: Fichas
        public ActionResult Index()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<Fichas>("Fichas");
            List<Fichas> List = collection.Find(d => true).ToList();
            return View(List);
        }

        [HttpPost]
        public ActionResult Crear(string Codigo, string Tipo, string Jornada, string Modalidad, string FechaInicio, string FechaFin)
        {
            try
            {
                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Fichas>("Fichas"); // Consistencia en el nombre de la colección
                var Fichas = new Fichas
                {
                    Codigo = Codigo,
                    Tipo = Tipo,
                    Jornada = Jornada,
                    Modalidad = Modalidad,
                    FechaInicio = FechaInicio,
                    FechaFin = FechaFin
                };

                collection.InsertOne(Fichas);
                return RedirectToAction("Index"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Index", new { mensaje = "Error al insertar la sala." });
            }
        }

        [HttpPost]
        public ActionResult Editar(string objectId, string Codigo, string Tipo, string Jornada, string Modalidad, string FechaInicio, string FechaFin)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<Fichas>("Fichas"); // Consistencia en el nombre de la colección

                var Fichas = new Fichas
                {
                    Id = parsedObjectId,
                    Codigo = Codigo,
                    Tipo = Tipo,
                    Jornada = Jornada,
                    Modalidad = Modalidad,
                    FechaInicio = FechaInicio,
                    FechaFin = FechaFin
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, Fichas);
                return RedirectToAction("Index"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Index", new { mensaje = ex.Message });
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
                var collection = database.GetCollection<Fichas>("Fichas"); // Consistencia en el nombre de la colección

                collection.DeleteOne(d => d.Id == parsedObjectId);
                return RedirectToAction("Index"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Index", new { mensaje = "Error al eliminar la sala." });
            }
        }
    }
}