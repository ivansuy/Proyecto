using Microsoft.EntityFrameworkCore;

namespace Proyecto
{
    public class ConexionDB : Microsoft.EntityFrameworkCore.DbContext
    {
        public ConexionDB(DbContextOptions<ConexionDB> options) : base(options)
        {
        }
        public DbSet<Models.Usuario> Usuario { get; set; }
        public DbSet<Models.Empaque> Empaque { get; set; }
        public DbSet<Models.marca> marca { get; set; }
        public DbSet<Models.Proveerdor> Proveedor { get; set; }
        public DbSet<Models.cliente> cliente { get; set; }
        public DbSet<Models.Producto> Producto { get; set; }
        public DbSet<Models.movimiento> movimiento { get; set; }
        public DbSet<Models.almacen> almacen { get; set; }    
        public DbSet<Models.inventario> inventario { get; set; }    
        public DbSet<Models.existencia> existencia { get; set; }    
        public DbSet<Models.ingreso> ingreso { get; set; }
        public DbSet<Models.salida> salida { get; set; }

        public List<Models.ExistenciaInfo> ObtenerExistenciasConDetalles()
        {
            var existenciasConDetalles = from e in existencia
                                         join p in Producto on e.Idproducto equals p.Id
                                         join a in almacen on e.IdAlmacen equals a.Id
                                         select new Models.ExistenciaInfo
                                         {
                                             Id = e.Id,
                                             NombreProducto = p.Nombreproducto,
                                             NombreAlmacen = a.Nombre,
                                             Cantidad = e.Cantidad
                                         };

            return existenciasConDetalles.ToList();
        }
    }
}
