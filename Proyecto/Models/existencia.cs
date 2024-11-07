using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class existencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Idproducto { get; set; }
        public int Cantidad { get; set; }
        public int IdAlmacen { get; set; }

        [ForeignKey("Idproducto")]
        public virtual  Producto Producto { get; set; }
        [ForeignKey("IdAlmacen")]
        public virtual  almacen almacen { get; set; }


    }
}
