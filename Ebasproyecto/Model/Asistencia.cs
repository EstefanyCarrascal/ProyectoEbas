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
        public string Asistio { get; set; } // Cambiado a string
        public string Fecha { get; set; }
        public string Hora { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string EventoId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; }
    }

}