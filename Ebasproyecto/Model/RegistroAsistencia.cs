using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class RegistroAsistencia
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("UsuarioId")]
    public string UsuarioId { get; set; }

    [BsonElement("EventoId")]
    public string EventoId { get; set; }

    [BsonElement("Fecha")]
    public string Fecha { get; set; }

    [BsonElement("Hora")]
    public string Hora { get; set; }

    [BsonElement("Asistio")]
    public bool Asistio { get; set; } // Ejemplo: "Sí"
}
