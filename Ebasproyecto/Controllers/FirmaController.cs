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
        private readonly IMongoCollection<Evento> _evento;
        private readonly MongoDBContext _context;

        // Constructor sin parámetros
        public FirmaController()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017"); 
            var database = mongoClient.GetDatabase("Ebas");
            _firmaCollection = database.GetCollection<Firma>("Firma");
            _usuarioCollection = database.GetCollection<Usuarios>("Usuarios");
            _context = new MongoDBContext();
        }

        // Constructor con parámetros 
        public FirmaController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Ebas");
            _firmaCollection = database.GetCollection<Firma>("Firma");
            _usuarioCollection = database.GetCollection<Usuarios>("Usuarios");
            _evento = database.GetCollection<Evento>("Evento"); 
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
        public ActionResult Firma(string Id)
        {
            var eventoId = new ObjectId(Id);
            var evento = _context.Evento.Find(e => e.Id == eventoId).FirstOrDefault();



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
            ViewBag.EventoNombre = evento.NombreEvento; 

            return View();
        }



        private ActionResult HttpNotFound(string mensaje)
        {
            return new HttpNotFoundResult(mensaje);
        }
    }
}
