using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class RegistroAsistencia
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string UsuarioId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string EventoId { get; set; }

    [BsonElement("Nombres")]
    public string Nombres { get; set; }

    [BsonElement("NombreEvento")]
    public string NombreEvento { get; set; }

    [BsonElement("Fecha")]
    public string Fecha { get; set; }

    [BsonElement("Hora")]
    public string Hora { get; set; }

    [BsonElement("Asistio")]
    public bool Asistio { get; set; } 
}
