using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Sistema_OT.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
    {
        private readonly string _connectionString = "Data Source=192.168.110.5;Initial Catalog=Db_ITSur_CSharp;User ID=sa;Password=felisa5";// Asegúrate de tener la cadena de conexión correcta

        // Acción GET para mostrar el formulario de login
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Retorna la vista Login.cshtml
        }

        // Acción POST para procesar el login
        [HttpPost]
        public IActionResult Login(string userId, string contrasenia)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(contrasenia))
            {
                ViewBag.ErrorMessage = "Usuario y contraseña son obligatorios.";
                return View(); // Retorna a la vista de login si falta información
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM Usuarios WHERE UserId = @UserId AND contrasenia = @Contrasenia AND DadoDeBaja = 'N'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Contrasenia", contrasenia); // Usando el campo correcto 'contrasenia'

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                if (count == 1)
                {
                    // Login exitoso
                    HttpContext.Session.SetString("UserId", userId); // Guardamos el UserId en la sesión
                    return RedirectToAction("VistaIndividualBusca", "Buscar"); // Redirige a la vista VistaIndividualBusca.cshtml después de un login exitoso
                }
                else
                {
                    // Credenciales inválidas o el usuario está dado de baja
                    ViewBag.ErrorMessage = "Usuario o contraseña incorrectos, o el usuario está dado de baja.";
                    return View(); // Retorna a la vista de login con el mensaje de error
                }
            }
        }

    // Pcerrar sesión (logout)
    public IActionResult Logout()
    {
        // Eliminar los datos de sesión
        HttpContext.Session.Clear();

        // Redirigir a la página de login
        return RedirectToAction("Login", "Account");
    }
}