using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ebasproyecto.Model
{
    public class Firma
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UsuarioId")]
        public string UsuarioId { get; set; }

        [BsonElement("EventoId")]
        public string EventoId { get; set; }

        [BsonElement("FechaFirma")]
        public DateTime FechaFirma { get; set; }

        [BsonElement("ImagenFirma")]
        public byte[] ImagenFirma { get; set; }
    }
}
