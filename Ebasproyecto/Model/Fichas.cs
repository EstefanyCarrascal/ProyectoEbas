using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ebasproyecto.Model
{
    public class Fichas
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Codigo")] 
        public string Codigo { get; set; }

        [BsonElement("Jornada")]
        public string Jornada { get; set; }

        [BsonElement("Modalidad")]
        public string Modalidad { get; set; }

        [BsonElement("Tipo")]
        public string Tipo { get; set; }

        [BsonElement("FechaInicio")]
        public string FechaInicio { get; set; }

        [BsonElement("FechaFin")]
        public string FechaFin {  get; set; }
    }
}