using MongoDB.Driver;
using Ebasproyecto.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Ebasproyecto.Controllers
{
    public class HomeAprendizController : Controller
    {
        MongoClient cn = new MongoClient("mongodb://localhost:27017/");

        // GET: HomeAprendiz/Eventos
        public ActionResult Index()
        {
            // Conectar a la base de datos y la colección de eventos
            var database = cn.GetDatabase("Ebas");
            var collection = database.GetCollection<Evento>("Evento");

            // Filtrar los eventos que sean para el rol "Aprendiz"

            List<Evento> eventosAprendiz = collection.Find(d => true).ToList();

            // Retornar la vista con los eventos del aprendiz
            return View(eventosAprendiz);
        }
    }
}
