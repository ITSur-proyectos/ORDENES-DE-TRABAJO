using System.Data.SqlClient;
namespace Sistema_OT.Services
{
    class ConexionDB
    {
        public SqlConnection con = null;
        public SqlDataReader reader = null;

        public ConexionDB()
        {
            con = new SqlConnection("Data Source=;Initial Catalog=;User ID=sa;Password=");
        }

        public void AbrirConexion()
        {
            try
            {
                con.Open();
                Console.WriteLine("Conexión abierta exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir la conexión: {ex.Message}");
            }
        }

        public void CerrarConexion()
        {
            if (con != null && con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
                Console.WriteLine("Conexión cerrada.");
            }
        }
    }
}

