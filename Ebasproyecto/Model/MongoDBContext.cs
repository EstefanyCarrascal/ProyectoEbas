using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver; 

namespace Ebasproyecto.Model

{
   
    public class MongoDBContext : Controller
    {
        private readonly IMongoDatabase _database;

       

        public MongoDBContext()
        {
            var connectionString = System.Configuration.ConfigurationManager.AppSettings["MongoDBSettings:ConnectionString"];
            var databaseName = System.Configuration.ConfigurationManager.AppSettings["MongoDBSettings:DatabaseName"];

            var client = new MongoClient(connectionString); 
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Usuarios> Users
        {
            get { return _database.GetCollection<Usuarios>("Usuarios"); }
        }


        
    }
}