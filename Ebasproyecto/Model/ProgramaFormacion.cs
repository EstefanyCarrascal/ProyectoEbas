using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ebasproyecto.Model
{
    public class ProgramaFormacion
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Codigo")]
        public string Codigo { get; set; }

        [BsonElement("Nombre")]
        public string Nombre { get; set; }

        [BsonElement("Tipo")]
        public string Tipo { get; set; }

        [BsonElement("Duracion")]
        public string Duracion { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string FichaId { get; set; }
    }
}