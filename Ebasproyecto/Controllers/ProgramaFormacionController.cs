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
    public class ProgramaFormacionController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");

        // GET: ProgramaFormacion
        public ActionResult Programa()
        {
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<ProgramaFormacion>("ProgramaFormacion");
            List<ProgramaFormacion> List = collection.Find(d => true).ToList();
            return View(List);
        }

        [HttpPost]
        public ActionResult Crear(string Codigo,string Nombre, string Tipo, string Duracion)
        {
            try
            {
                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<ProgramaFormacion>("ProgramaFormacion"); // Consistencia en el nombre de la colección
                var ProgramaFormacion = new ProgramaFormacion
                {
                    codigo = Codigo,
                    Tipo = Tipo,
                    Nombre = Nombre,
                    Duracion = Duracion,
                };

                collection.InsertOne(ProgramaFormacion);
                return RedirectToAction("Programa"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                // Manejo de excepción, opcionalmente puedes mostrar un mensaje de error
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Programa", new { mensaje = "Error al insertar la sala." });
            }

        }


        [HttpPost]
        public ActionResult Editar(string objectId, string Codigo, string Nombre, string Tipo, string Duracion)
        {
            try
            {
                if (!ObjectId.TryParse(objectId, out var parsedObjectId))
                {
                    throw new ArgumentException("Invalid ObjectId format.");
                }

                var database = cn.GetDatabase("Ebas"); // Consistencia en el nombre de la base de datos
                var collection = database.GetCollection<ProgramaFormacion>("ProgramaFormacion"); // Consistencia en el nombre de la colección

                var ProgramaFormacion = new ProgramaFormacion
                {
                    Id = parsedObjectId,
                    codigo = Codigo,
                    Tipo = Tipo,
                    Nombre = Nombre,
                    Duracion = Duracion,
                };

                collection.ReplaceOne(d => d.Id == parsedObjectId, ProgramaFormacion);
                return RedirectToAction("Programa"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Programa", new { mensaje = ex.Message });
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
                var collection = database.GetCollection<ProgramaFormacion>("ProgramaFormacion"); // Consistencia en el nombre de la colección

                collection.DeleteOne(d => d.Id == parsedObjectId);
                return RedirectToAction("Programa"); // Corrección en el redireccionamiento
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Programa", new { mensaje = "Error al eliminar la sala." });
            }
        }

    }
}