namespace OpsControl.Api.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt{ get; set; }

    }
}
//| Propiedad | Qué debe guardar                            |
//| ------------- | ------------------------------------------- |
//| `Id`          | El identificador numérico de la incidencia. |
//| `Title`       | Su título.                                  |
//| `Description` | La explicación del problema.                |
//| `Status`      | Su estado; de momento, como texto.          |
//| `CreatedAt`   | La fecha y hora de creación.                |
