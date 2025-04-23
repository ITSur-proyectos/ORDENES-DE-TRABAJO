using Sistema_OT.Services;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Reflection.PortableExecutable;

namespace Sistema_OT.Models
{
    public class OrdenDeTrabajo
    {

        //Asigno get set a todas las variables, estos datos los voy a conseguir en la consulta sql
        //Estos datos estan a cambiar, falta esperar confirmacion para ver que datos son innecesarios para el sistema y se eliminaran

        /*
         Es posible hacerlo de una manera no tan tipada, y sin definir todas estas variables al simplemente almacenar el resultado de la consulta en un diccionario
         pero siento que será mas comodo para insertar todos los valores en los formularios en el futuro, se cambiará de ser necesario  
        */
        public decimal NroOrdenTrabajo { get; set; }
        public int Cliente { get; set; }
        public int Sistema { get; set; }
        public string Modulo { get; set; }
        public string Asunto { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public int CantidadHorasEstimadas { get; set; }
        public int CantidadHorasConsumidas { get; set; }
        public int Estado { get; set; }
        public int PorcentajeAvance { get; set; }
        public int UsuarioSolicitante { get; set; }
        public int UsuarioResponsable { get; set; }
        public string Descripcion { get; set; }
        public string Observaciones { get; set; }
        public int Prioridad { get; set; }
        public string FormulariosModificados { get; set; }
        public string ModificacionesBaseDatos { get; set; }
        public string UserIDSolicitante { get; set; }
        public string UserIDResponsable { get; set; }
        public static List<Dictionary<string, object>> ObtenerLista(string consulta, Dictionary<string, object> parametrosSP)
        {

            List<Dictionary<string, object>> OrdenesTrabajo = new List<Dictionary<string, object>>();
            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();
            using (SqlCommand command = new SqlCommand(consulta, conexionDB.con))

            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (var a in parametrosSP)
                {
                    command.Parameters.AddWithValue(a.Key, a.Value);
                }
                //command.Parameters.AddWithValue("@P_NroOrdenTrabajoDesde", numOTDesde);
                //command.Parameters.AddWithValue("@P_NroOrdenTrabajoHasta", numOTHasta);
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Dictionary<string, object> orden = new Dictionary<string, object>();
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.IsDBNull(i) ? "" : reader.GetValue(i);
                                orden[columnName] = value;
                            }
                          
                            OrdenesTrabajo.Add(orden);
                        };
                        
                            
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Hubo un error al ejecutar la consulta: " + e.ToString());
                }
            }
            return OrdenesTrabajo;
        }
        public static Dictionary<int, string> ConseguirNombres(string Tabla)
        {
            Dictionary<int, string> nombres = new Dictionary<int, string>();
            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();
            string consulta = "Select Descripcion, " + Tabla +" From " + Tabla + "s"; // +s porque menos mal que las tablas estan en plural
            using (SqlCommand command = new SqlCommand(consulta, conexionDB.con))

            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal(Tabla));
                            string nombre = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion"));
                            nombres[id] = nombre;
                            

                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return nombres;
        }




    }
}