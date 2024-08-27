using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ebasproyecto.Model
{
    public class Evento
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Descripcion")] 
        public string Descripcion { get; set; }

        [BsonElement("FechaEvento")]
        public string FechaEvento { get; set; }

        [BsonElement("Organizador")]
        public string Organizador { get; set; }

        [BsonElement("NombreEvento")]
        public string NombreEvento { get; set; }

        [BsonElement("Tipo")]
        public string Tipo { get; set; } 


    }
}