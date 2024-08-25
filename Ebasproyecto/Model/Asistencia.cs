using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ebasproyecto.Model
{
    public class Asistencia
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Fecha")]
        public string Fecha { get; set; }

        [BsonElement("HoraInicio")]
        public string HoraInicio { get; set; }

        [BsonElement("HoraFin")]
        public string HoraFin { get; set; }

        [BsonElement("Detalles")]
        public string Detalles { get; set; }

    }
}