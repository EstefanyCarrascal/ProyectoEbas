using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ebasproyecto.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ebasproyecto.Controllers
{
    public class FirmaController : Controller
    {
        private readonly IMongoCollection<Firma> _firmaCollection;
        private readonly IMongoCollection<Usuarios> _usuarioCollection;

        // Constructor sin parámetros
        public FirmaController()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017"); // Reemplaza con tu cadena de conexión
            var database = mongoClient.GetDatabase("Ebas");
            _firmaCollection = database.GetCollection<Firma>("Firma");
            _usuarioCollection = database.GetCollection<Usuarios>("Usuarios");
        }

        // Constructor con parámetros (opcional)
        public FirmaController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Ebas");
            _firmaCollection = database.GetCollection<Firma>("Firma");
            _usuarioCollection = database.GetCollection<Usuarios>("Usuarios");
        }

        [HttpPost]
        public async Task<ActionResult> GuardarFirma(string firmaData, string usuarioId, string eventoId)
        {
            if (!string.IsNullOrEmpty(firmaData))
            {
                // Convertir el ID del usuario a ObjectId
                var objectId = new ObjectId(usuarioId);

                // Verificar si el usuario existe
                var usuario = await _usuarioCollection.Find(u => u.Id == objectId).FirstOrDefaultAsync();
                if (usuario == null)
                {
                    return HttpNotFound("Usuario no encontrado.");
                }

                // Decodificar la imagen base64 a byte array
                var firmaBytes = Convert.FromBase64String(firmaData.Replace("data:image/png;base64,", ""));

                // Crear un nuevo objeto Firma y guardarlo en la base de datos
                var nuevaFirma = new Firma
                {
                    Id = Guid.NewGuid().ToString(),
                    UsuarioId = usuario.Id.ToString(),
                    EventoId = eventoId,
                    FechaFirma = DateTime.Now,
                    ImagenFirma = firmaBytes
                };

                await _firmaCollection.InsertOneAsync(nuevaFirma);
            }

            return RedirectToAction("AsistenciaConfirmada");
        }

        // Acción para mostrar la vista de firma
        public ActionResult Firma(string usuarioId, string eventoId)
        {
            ViewBag.UsuarioId = usuarioId;
            ViewBag.EventoId = eventoId;
            return View();
        }

        private ActionResult HttpNotFound(string mensaje)
        {
            return new HttpNotFoundResult(mensaje);
        }
    }
}
