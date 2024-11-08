using System.Data.SqlClient;
namespace Sistema_OT.Services
{
    class ConexionDB
    {
        public SqlConnection con = null;
        public SqlDataReader reader = null;

        public ConexionDB()
        {
            con = new SqlConnection("Data Source=192.168.110.5;Initial Catalog=Db_ITSur_CSharp;User ID=sa;Password=felisa5");
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

