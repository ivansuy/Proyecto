using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class inventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public string Fecha { get; set; }
        public int IdProducto { get; set; }
        public int Idmovimiento { get; set; }

        //Relaciones externas
        [ForeignKey("IdProducto")]
        public virtual required Producto Producto { get; set; }
        [ForeignKey("Idmovimiento")]
        public virtual required movimiento movimiento { get; set; }

    }
}
