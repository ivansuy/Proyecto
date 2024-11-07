using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace Proyecto.Controllers
{
    public class LoginController : Controller
    {
        private readonly ConexionDB ConexionDB;
        private readonly IWebHostEnvironment _hostEnvironment;

        public LoginController(ConexionDB context, IWebHostEnvironment hostEnvironment)
        {
            ConexionDB = context;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario value)
        {
            if (!ModelState.IsValid)
            {
                return View(); // Mantener errores de validación
            }

            // Obtener el token de reCAPTCHA del formulario
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6Lc_N10qAAAAAMK4S4gW4K0nOQeUUahuNXrTDnc0\r\n";

            // Validar el token de reCAPTCHA con la API de Google
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}", null);
            var jsonResult = await response.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(jsonResult);

            // Verificar si el captcha fue exitoso
            if (result.success != true)
            {
                TempData["Resultado"] = "Error";
                TempData["Mensaje"] = "Verificación de CAPTCHA fallida.";
                return View(); // Mostrar el formulario de inicio de sesión con el mensaje de error
            }


            try
            {
                var UsuarioNombre = ConexionDB.Usuario.
                Where(x => x.Nombre == value.Nombre)
                .FirstOrDefault();

                var Usuario = ConexionDB.Usuario.
                Where(x => x.Nombre == value.Nombre && x.Contraseña == EncriptarString(value.Contraseña))
                .FirstOrDefault();

                if (Usuario != null)
                {

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, value.Nombre),
                new Claim("IdUsuario", value.Id.ToString()),
                // Agregar claims adicionales según sea necesario
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true // Opcional: Permitir inicios de sesión persistentes (cookies en las sesiones del navegador)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    //Guardando la informacion del usuario 

                    var json = JsonConvert.SerializeObject(Usuario);
                    HttpContext.Session.SetString("Usuario", json);


                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Resultado"] = "Error";
                    if (UsuarioNombre != null)
                    {
                        TempData["Mensaje"] = "Contraseña incorrecta";
                    }
                    else
                    {
                        TempData["Mensaje"] = "Usuario no encontrado";
                    }
                }
            }
            catch (Exception Error)
            {
                TempData["Resultado"] = "Error";
                TempData["Mensaje"] = Error.Message;
            }
            return View(); // Mostrar el formulario de inicio de sesión con un mensaje de error
        }

        [HttpPost]
        public IActionResult Registrarse(Usuario value)
        {
            try
            {
                value.Contraseña = EncriptarString(value.Contraseña);

                ConexionDB.Usuario.Add(value);
                ConexionDB.SaveChanges();

                TempData["Mensaje"] = "Registro exitoso";

            }
            catch (Exception error)
            {
                TempData["Mensaje"] = error.Message;
            }
            return RedirectToAction("Login", "Login");

        }

        public static string EncriptarString(string texto)
        {

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
