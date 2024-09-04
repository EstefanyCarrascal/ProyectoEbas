using System;
using System.Linq;
using System.Web.Mvc;
using Ebasproyecto.Model;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Ebasproyecto.Controllers
{
    public class LoginController : Controller
    {
        private readonly MongoDBContext _context;

        public LoginController()
        {
            _context = new MongoDBContext();
        }

        // GET: Login
        public ActionResult Index1()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Correo, string Contraseña)
        {
            // Verificar que los parámetros no sean nulos o vacíos
            if (string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Contraseña))
            {
                ViewBag.Message = "Por favor, ingrese correo y contraseña.";
                return View("Index1");
            }

            // Buscar al usuario en la base de datos
            var user = _context.Users.Find(u => u.Correo == Correo && u.Contraseña == Contraseña).FirstOrDefault();

            // Validar si el usuario fue encontrado
            if (user == null)
            {
                // Si no se encontró el usuario, mostrar un mensaje de error
                ViewBag.Message = "Usuario o contraseña incorrecta.";
                return View("Index1");
            }

            // Guardar la información del usuario en la sesión
            Session["Usuario"] = user;

            // Redirigir según el tipo de usuario
            if (user.TipoUsuario == "PersonalAdministrativo")
            {
                return RedirectToAction("Index", "Home");
            }
            if (user.TipoUsuario == "Aprendiz")
            {
                return RedirectToAction("Index", "HomeAprendiz");
            }
            else
            {
                // Si el tipo de usuario no coincide con ningún caso, mostrar un mensaje de error
                ViewBag.Message = "Tipo de usuario no reconocido.";
                return View("Index1");
            }
        }

        [HttpPost]
        public ActionResult Registrar(string Nombres, string Apellidos, string Correo, string Contraseña, string TipoUsuario)
        {
            var existeUser = _context.Users.Find(u => u.Correo == Correo).FirstOrDefault();

            if (existeUser != null)
            {
                ViewBag.Message = "Este correo ya está registrado.";
                return View("Index1");
            }

            var nuevoUsuario = new Usuarios
            {
                Id = ObjectId.GenerateNewId(),
                Nombres = Nombres,
                Apellidos = Apellidos,
                Correo = Correo,
                Contraseña = Contraseña,
                TipoUsuario = TipoUsuario
            };

            _context.Users.InsertOne(nuevoUsuario);

            return RedirectToAction("Index1", "Login");
        }
    }
}
