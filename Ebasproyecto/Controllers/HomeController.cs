using MongoDB.Driver;
using System;
using System.Linq;
using System.Web.Mvc;
using Ebasproyecto.Model;

namespace Ebasproyecto.Controllers
{

    public class HomeController : Controller
    {
        private readonly MongoDBContext _context;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public HomeController()
        {
            _context = new MongoDBContext();
            _client = new MongoClient("mongodb://localhost:27017/");
            _database = _client.GetDatabase("Ebas");
        }

        public ActionResult Index()
        {
            var totaleventos = _context.Evento.CountDocuments(_ => true);

            ViewBag.totalevento = totaleventos;

            var totaladministrativo = _context.Users.CountDocuments(_ => true);

            ViewBag.totaladministrativo = totaladministrativo;


            var totalregistro = _context.RegistroAsistencia.CountDocuments(_ => true);

            ViewBag.totalregistro = totalregistro;


            var totalaprendices = _context.Users.CountDocuments(_ => true);

            ViewBag.totalaprendices = totalaprendices;

            return View();
        }

        [HttpPost]
        public ActionResult Editar(
            string Nombres,
            string Apellidos,
            string Documento,
            string TipoDocumento,
            string Correo,
            string Sexo,
            string Edad,
            string Municipio,
            string Direccion,
            string EstadoCivil,
            string Telefono,
            string TipoPoblacion,
            string TipoUsuario,
            string Contraseña)
        {
            try
            {
                var user = Session["Usuario"] as Usuarios;

                if (user == null)
                {
                    return RedirectToAction("Index1", "Login");
                }

                var collection = _database.GetCollection<Usuarios>("Usuarios");
                var usuario = collection.Find(u => u.Id == user.Id).FirstOrDefault();

                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado.");
                }

                // Actualizar los campos
                usuario.Nombres = Nombres;
                usuario.Apellidos = Apellidos;
                usuario.Documento = Documento;
                usuario.TipoDocumento = TipoDocumento;
                usuario.Correo = Correo;
                usuario.Sexo = Sexo;
                usuario.Edad = Edad;
                usuario.Municipio = Municipio;
                usuario.Direccion = Direccion;
                usuario.EstadoCivil = EstadoCivil;
                usuario.Telefono = Telefono;
                usuario.TipoPoblacion = TipoPoblacion;
                usuario.TipoUsuario = TipoUsuario;
                usuario.Contraseña = Contraseña;

                collection.ReplaceOne(d => d.Id == user.Id, usuario);

                // Actualizar los datos en la sesión
                Session["Usuario"] = usuario;

                return RedirectToAction("Index1");
            }
            catch (Exception ex)
            {
                // Manejo de error: Redirigir a la vista Index con el mensaje de error
                return RedirectToAction("Index1", new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult CargarDatosEditar()
        {
            try
            {
                var usuario = ObtenerUsuarioLogueado();
                if (usuario == null)
                {
                    return Json(new { success = false, message = "Usuario no encontrado." }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = true, data = usuario }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        private Usuarios ObtenerUsuarioLogueado()
        {
            // Asumiendo que tienes la información del usuario en la sesión.
            return Session["Usuario"] as Usuarios;
        }
    }
}
