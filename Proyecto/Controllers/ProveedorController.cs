using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
namespace Proyecto.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly ConexionDB ConexionDB;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProveedorController(ConexionDB context, IWebHostEnvironment hostEnvironment)
        {
            ConexionDB = context;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Vistprovee()
        {

            var ListaDatos = ConexionDB.Proveedor.ToList();


            ViewBag.lista = ListaDatos;


            return View();
        }

        [HttpPost]
        public IActionResult Guardarproveedor(Models.Proveerdor Datos)
        {



            ConexionDB.Proveedor.Add(Datos);

            ConexionDB.SaveChanges();
            return RedirectToAction("Vistprovee", "Proveedor");
        }


    }
}
