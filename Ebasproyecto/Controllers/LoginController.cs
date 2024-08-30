using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson; 
using System.Web.Mvc;
using Ebasproyecto.Model;

namespace Ebasproyecto.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }

        private readonly MongoDBContext _context; 

        public LoginController()
        {
            _context = new MongoDBContext(); 
        }
        [HttpPost]
        public ActionResult Login(string Correo, string Contraseña)
        {

            // Verificar que los parámetros no sean nulos o vacíos
            if (string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Contraseña))
            {
                ViewBag.Message = "Por favor, ingrese correo y contraseña.";
                return View();
            }

            // Buscar al usuario en la base de datos
            var user = _context.Users.Find(u => u.Correo == Correo && u.Contraseña == Contraseña).FirstOrDefault();

            // Validar si el usuario fue encontrado
            if (user == null)
            {
                // Si no se encontró el usuario, mostrar un mensaje de error
                ViewBag.Message = "Usuario o contraseña incorrecta.";
                return RedirectToAction("Index2", "Login");

            }

            Session["UserId"] = user.Id.ToString();


            // Verificar el tipo de usuario y redirigir según corresponda
            if (user.TipoUsuario == "Administrador")
            {
                return RedirectToAction("Index", "Home");
            }
            if (user.TipoUsuario == "PersonalAdministrativo")
            {
                return RedirectToAction("Index", "Home");
            }
            if (user.TipoUsuario == "Aprendiz")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Si el tipo de usuario no coincide con ningún caso, mostrar un mensaje de error
                ViewBag.Message = "Tipo de usuario no reconocido.";
                return View();
            }

        
        }


        [HttpPost]
        public ActionResult Registrar(string Nombres,string Apellidos, string Correo, string Contraseña, string TipoUsuario)
        {
            var existeUser = _context.Users.Find(u => u.Correo == Correo).FirstOrDefault();

            if (existeUser != null)
            {
                ViewBag.Message = "Este correo ya está registrado.";
                return View();
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

            return RedirectToAction("Index", "Login");

    }   }
}