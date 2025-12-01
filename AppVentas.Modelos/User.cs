using System.ComponentModel.DataAnnotations;

namespace AppVentas.Modelos
{
    public class User
    {
        [Key] public int UserId { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }


    }
}
