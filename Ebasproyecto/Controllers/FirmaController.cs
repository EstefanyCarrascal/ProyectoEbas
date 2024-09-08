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
        private readonly IMongoCollection<Evento> _eventoCollection;

        public FirmaController()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Ebas");
            _firmaCollection = database.GetCollection<Firma>("Firma");
            _usuarioCollection = database.GetCollection<Usuarios>("Usuarios");
            _eventoCollection = database.GetCollection<Evento>("Evento");
        }



        [HttpPost]
        public async Task<ActionResult> GuardarFirma(string firmaData, string usuarioId, string eventoId)
        {
            if (string.IsNullOrEmpty(firmaData) || firmaData == "data:image/png;base64,")
            {
                // Redirigir con mensaje de error si no hay firma
                ViewBag.Mensaje = "No se ha guardado la firma";
                return RedirectToAction("Firma", new { Id = eventoId, mensaje = "No se ha recibido la firma. Por favor, firme antes de guardar." });
            }

            // Validar que usuarioId y eventoId sean válidos ObjectId
            if (!ObjectId.TryParse(usuarioId, out ObjectId objectIdUsuario))
            {
                return RedirectToAction("Firma", new { Id = eventoId, mensaje = "ID de usuario no válido." });
            }

            if (!ObjectId.TryParse(eventoId, out ObjectId objectIdEvento))
            {
                return RedirectToAction("Firma", new { Id = eventoId, mensaje = "ID de evento no válido." });
            }

            // Verificar si el usuario existe
            var usuario = await _usuarioCollection.Find(u => u.Id == objectIdUsuario).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return HttpNotFound("Usuario no encontrado.");
            }

            // Verificar si el evento existe
            var evento = await _eventoCollection.Find(e => e.Id == objectIdEvento).FirstOrDefaultAsync();
            if (evento == null)
            {
                return HttpNotFound("Evento no encontrado.");
            }

            // Eliminar el prefijo data:image/png;base64, de la cadena base64
            var base64Data = firmaData.Replace("data:image/png;base64,", "");
            var imageBase64 = base64Data; // Usa la cadena base64 directamente

            // Crear una nueva instancia del modelo Firma
            var nuevaFirma = new Firma
            {
                UsuarioId = usuarioId,
                EventoId = eventoId,
                FechaFirma = DateTime.Now,
                ImagenFirma = imageBase64 // Guardar solo la cadena base64 sin prefijo
            };

            // Insertar la firma en MongoDB
            TempData["Mensaje"] = "Firma guardada exitosamente";
            return RedirectToAction("Firma", new { Id = eventoId });
        }

        public ActionResult Firma(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return HttpNotFound("El ID del evento no ha sido proporcionado.");
            }

            // Convertir el ID del evento a ObjectId
            if (!ObjectId.TryParse(Id, out ObjectId objectIdEvento))
            {
                return HttpNotFound("ID del evento no válido.");
            }

            // Obtener el evento desde la base de datos
            var evento = _eventoCollection.Find(e => e.Id == objectIdEvento).FirstOrDefault();
            if (evento == null)
            {
                return HttpNotFound("Evento no encontrado.");
            }

            // Recuperar el usuario de la sesión
            var usuario = Session["Usuario"] as Usuarios;
            if (usuario == null)
            {
                return HttpNotFound("Usuario no encontrado.");
            }

            // Pasar los datos del usuario a la vista
            ViewBag.Nombres = usuario.Nombres;
            ViewBag.Apellidos = usuario.Apellidos;
            ViewBag.Documento = usuario.Documento;
            ViewBag.Telefono = usuario.Telefono;
            ViewBag.Correo = usuario.Correo;
            ViewBag.Sexo = usuario.Sexo;
            ViewBag.Edad = usuario.Edad;
            ViewBag.Municipio = usuario.Municipio;
            ViewBag.Direccion = usuario.Direccion;
            ViewBag.EstadoCivil = usuario.EstadoCivil;
            ViewBag.TipoDocumento = usuario.TipoDocumento;
            ViewBag.TipoPoblacion = usuario.TipoPoblacion;
            ViewBag.TipoUsuario = usuario.TipoUsuario;

            // Pasar los IDs del usuario y del evento
            ViewBag.UsuarioId = usuario.Id.ToString();
            ViewBag.EventoId = evento.Id.ToString();
            ViewBag.EventoNombre = evento.NombreEvento;
            

            return View();
        }

    }
}
