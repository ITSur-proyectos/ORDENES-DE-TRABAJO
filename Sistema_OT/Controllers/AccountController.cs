using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Sistema_OT.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
    private readonly string _connectionString = "Data Source=192.168.110.5;Initial Catalog=Db_ITSur_CSharp;User ID=sa;Password=felisa5";// Asegúrate de tener la cadena de conexión correcta
    //private readonly string _connectionString = "Data Source=Db_ITSur_CSharp.mssql.somee.com;Initial Catalog=Db_ITSur_CSharp;User ID=EugeniaITSur;Password=421295845";// Asegúrate de tener la cadena de conexión correcta



    // Acción GET para mostrar el formulario de login
    [HttpGet]
    public IActionResult Login()
    {
        return View(); // Login.cshtml
    }

    // Acción POST para procesar el login
    [HttpPost]
    public IActionResult Login(string userId, string contrasenia)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(contrasenia))
        {
            ViewBag.ErrorMessage = "Usuario y contraseña son obligatorios.";
            return View();
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

                HttpContext.Session.SetString("UserId", userId); // mantiene el UserId en la sesión
                return RedirectToAction("VistaIndividualBusca", "Buscar");
            }
            else
            {
                // modal
                ViewBag.ErrorMessage = "Usuario o contraseña incorrectos";
                return View(); // Retorna a la vista de login con el mensaje de error
            }
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }


    //  "¿Olvidaste tu contraseña?" 

    // GET: Muestra el formulario para recuperar contraseña
    [HttpGet]
    public IActionResult OlvidasteContrasenia()
    {
        return View();
    }

 

    }