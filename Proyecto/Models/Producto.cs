using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombreproducto{ get; set; }
        public string Descripcion{ get; set; }
        public int Cantidad { get; set; }
        public double Preciounitario { get; set; }
        public int  IdProveedor { get; set; }
        public int  IdEmpaque { get; set; }

        //Relaciones externas
        [ForeignKey("IdProveedor")]
        public virtual required Proveerdor Proveerdor { get; set; }
        [ForeignKey("IdEmpaque")]
        public virtual required Empaque Empaque { get; set; }

    }
}
