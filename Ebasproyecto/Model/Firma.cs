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

        [BsonRepresentation(BsonType.String)] // Para almacenar la imagen como string en formato Base64
        public string ImagenFirma { get; set; } // Imagen en Base64
    }
}
