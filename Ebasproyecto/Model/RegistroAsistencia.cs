using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ebasproyecto.Model
{
    public class RegistroAsistencia
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Fecha")]
        public string Fecha { get; set; }

        [BsonElement("Hora")]
        public string Hora { get; set; }

        [BsonElement("Asistio")]
        public string Asistio { get; set; }
    }
}