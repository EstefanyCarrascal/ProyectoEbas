using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ebasproyecto.Model
{
    public class Usuarios
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Nombres")]
        public string Nombres { get; set; }

        [BsonElement("Apellidos")]
        public string Apellidos { get; set; }

        [BsonElement("Documento")]
        public string Documento { get; set; }

        [BsonElement("Telefono")]
        public string Telefono { get; set; }

        [BsonElement("Correo")]
        public string Correo { get; set; }

        [BsonElement("Sexo")]
        public string Sexo { get; set; }

        [BsonElement("Edad")]
        public string Edad { get; set; }

        [BsonElement("Municipio")]
        public string Municipio { get; set; }

        [BsonElement("Direccion")]
        public string Direccion { get; set; }

        [BsonElement("EstadoCivil")]
        public string EstadoCivil { get; set; }

        [BsonElement("TipoDocumento")]
        public string TipoDocumento { get; set; }

        [BsonElement("TipoPoblacion")]
        public string TipoPoblacion { get; set; }

        [BsonElement("TipoUsuario")]
        public string TipoUsuario { get; set; }

        [BsonElement("Contraseña")]
        public string Contraseña { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string FichaId { get; set; }
    }
}
