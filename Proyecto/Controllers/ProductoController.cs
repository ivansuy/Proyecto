using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.WebRequestMethods;

namespace Proyecto.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ConexionDB ConexionDB;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductoController(ConexionDB context, IWebHostEnvironment hostEnvironment)
        {
            ConexionDB = context;
            _hostEnvironment = hostEnvironment;
        }

        // clase de empaque 

        public IActionResult CrearDetalle()
        {
            var ListaDatos = ConexionDB.Empaque.ToList();


            ViewBag.juan = ListaDatos;


            return View();
        }

        [HttpPost]
        public IActionResult GuardarEmpaque(Models.Empaque Datos)
        {



            ConexionDB.Empaque.Add(Datos);

            ConexionDB.SaveChanges();
            return RedirectToAction("CrearDetalle", "Producto");
        }
        //clase cliente interno 
        public IActionResult cliente_interno()
        {
            var ListaDatos = ConexionDB.cliente.ToList();


            ViewBag.orden = ListaDatos;


            return View();
        }

        [HttpPost]
        public IActionResult guardarcliente(Models.cliente Datos)
        {



            ConexionDB.cliente.Add(Datos);

            ConexionDB.SaveChanges();
            return RedirectToAction("Cliente_interno", "Producto");
        }
        // clase de producto 
        public IActionResult CrearProducto()
        {
            var ListaEmpaques = ConexionDB.Empaque.ToList();

            var ListaProveedor = ConexionDB.Proveedor.ToList();
            var producto = ConexionDB.Producto.ToList();


            ViewBag.ListaEmpaques = ListaEmpaques;
            ViewBag.ListaProveedor = ListaProveedor;
            ViewBag.producto = producto;
            return View();
        }

        [HttpPost]
        public IActionResult GuardarProducto(Models.Producto Datos)
        {



            ConexionDB.Producto.Add(Datos);

            ConexionDB.SaveChanges();
            return RedirectToAction("CrearProducto", "Producto");
        }
        // clase de movimiento 
        public IActionResult move()
        {
            var Lista = ConexionDB.movimiento.ToList();


            ViewBag.movi = Lista;


            return View();
        }

        [HttpPost]
        public IActionResult Guardarmovimiento(Models.movimiento Datos)
        {



            ConexionDB.movimiento.Add(Datos);

            ConexionDB.SaveChanges();
            return RedirectToAction("move", "Producto");
        }

        // clase de alamcen  
        public IActionResult almacen()
        {
            var Lista = ConexionDB.almacen.ToList();


            ViewBag.bodega = Lista;


            return View();
        }

        [HttpPost]
        public IActionResult almacen(Models.almacen Datos)
        {

            var MiRegistro = ConexionDB.almacen.Find(1);


            MiRegistro.Nombre = "nuevo nombre";
            ConexionDB.SaveChanges(); // Guardar los cambios en la base de datos


            ConexionDB.SaveChanges();
            return RedirectToAction("almacen", "Producto");
        }

    // clase existencia 
        public IActionResult existencia()
        {
            var Lista = ConexionDB.ObtenerExistenciasConDetalles();


            ViewBag.exis = Lista;


            return View();
        }

        [HttpPost]
        public IActionResult existencia(Models.existencia Datos)
        {



            ConexionDB.existencia.Add(Datos);

            ConexionDB.SaveChanges();
            return RedirectToAction("existencia", "Producto");
        }

        //clase ingreso
        public IActionResult ingreso()
        {
            var ListaProducto = ConexionDB.Producto.ToList();

            var Listaalmacen = ConexionDB.almacen.ToList();
            var ingress = ConexionDB.ingreso.ToList();


            ViewBag.ListaProducto = ListaProducto;
            ViewBag.Listaalmacen = Listaalmacen;
            ViewBag.ingress = ingress;
            return View();
        }

        [HttpPost]
        public IActionResult ingreso(Models.ingreso Datos)
        {
            var idproductoingreso=Datos.IdProducto;

            var ResultadoBusqueda = ConexionDB.existencia.Where(Table => Table.Idproducto == idproductoingreso).SingleOrDefault();
            
            if (ResultadoBusqueda != null)
            {
                //Actualizar registro existente
                ResultadoBusqueda.Cantidad += Datos.Cantidad;
                ConexionDB.SaveChanges();
            }
            else
            {
                //Crear un nuevo registro
                Models.existencia NuevoDato = new Models.existencia();

                NuevoDato.Idproducto = Datos.IdProducto;
                NuevoDato.Cantidad = Datos.Cantidad;
                NuevoDato.IdAlmacen = Datos.Idalmacen;

                // Agregar el nuevo dato a la base de datos
                ConexionDB.existencia.Add(NuevoDato);
                ConexionDB.SaveChanges();
            }
            ConexionDB.ingreso.Add(Datos);
            ConexionDB.SaveChanges();
            return RedirectToAction("ingreso", "Producto");
        }
        // clase salida de producto 

        public IActionResult salida()
        {
            var ListaProducto = ConexionDB.Producto.ToList();

            var Listaalmacen = ConexionDB.almacen.ToList();
            var sali = ConexionDB.salida.ToList();


            ViewBag.ListaProducto = ListaProducto;
            ViewBag.Listaalmacen = Listaalmacen;
            ViewBag.sali =sali;
            return View();
        }

        [HttpPost]
        public IActionResult salida(Models.salida Datos)
        {
            var idproductoingreso = Datos.IdProducto;

            var ResultadoBusqueda = ConexionDB.existencia.Where(Table => Table.Idproducto == idproductoingreso).SingleOrDefault();

            if (ResultadoBusqueda != null)
            {
                //Actualizar registro existente
                ResultadoBusqueda.Cantidad -= Datos.Cantidad;
                ConexionDB.SaveChanges();
            }
            else
            {
                //Crear un nuevo registro
                Models.existencia NuevoDato = new Models.existencia();

                NuevoDato.Idproducto = Datos.IdProducto;
                NuevoDato.Cantidad = Datos.Cantidad;
                NuevoDato.IdAlmacen = Datos.Idalmacen;

                // Agregar el nuevo dato a la base de datos
                ConexionDB.existencia.Add(NuevoDato);
                ConexionDB.SaveChanges();
            }
            ConexionDB.salida.Add(Datos);
            ConexionDB.SaveChanges();
            return RedirectToAction("salida", "Producto");
        }
        public IActionResult Reporte()
        {
            return View();
        }
    }
}
