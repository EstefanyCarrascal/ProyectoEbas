using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ebasproyecto.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ebasproyecto.Controllers
{
    public class UsuarioController : Controller
    {
            private readonly IMongoCollection<Usuarios> _usuarios;

            public UsuarioController()
            {
                // Con o sin autenticación según lo que necesites
                MongoClient cn = new MongoClient("mongodb://usuario:contraseña@localhost:27017/?authSource=admin");
                var database = cn.GetDatabase("Ebas");
                _usuarios = database.GetCollection<Usuarios>("Usuarios");
            }

        public ActionResult EditarPerfil(string id )
        {
            var usuario = _usuarios.Find(u => u.Id == new ObjectId(id)).FirstOrDefault();
            return View(usuario);
        }

        [HttpPost]
        public ActionResult EditarPerfil(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                _usuarios.ReplaceOne(u => u.Id == usuario.Id, usuario);
                return RedirectToAction("PerfilActualizado");
            }
            return View(usuario);
        }

        public ActionResult PerfilActualizado()
        {
            return View();
        }
    }
}
