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
        public static List<OrdenDeTrabajo> ObtenerLista(string consulta, Dictionary<string, int> parametrosSP)
        {

            List<OrdenDeTrabajo> OrdenesTrabajo = new List<OrdenDeTrabajo>();
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
                            OrdenDeTrabajo Orden = new OrdenDeTrabajo
                            {

                                //Le asigno a cada variable de la clase orden de trabajo su valor respectivo desde la base de datos, segun la consulta de arriba
                                NroOrdenTrabajo = reader.IsDBNull(reader.GetOrdinal("NroOrdenTrabajo")) ? 0 : reader.GetDecimal(reader.GetOrdinal("NroOrdenTrabajo")),
                                //Cliente = reader.IsDBNull(reader.GetOrdinal("Cliente")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cliente")),
                                //Sistema = reader.IsDBNull(reader.GetOrdinal("Sistema")) ? 0 : reader.GetInt32(reader.GetOrdinal("Sistema")),
                                //Modulo = reader.IsDBNull(reader.GetOrdinal("Modulo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Modulo")),
                                Asunto = reader.IsDBNull(reader.GetOrdinal("Asunto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asunto")),
                                //FechaSolicitud = reader.IsDBNull(reader.GetOrdinal("FechaSolicitud")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaSolicitud")),
                                //FechaFinalizacion = reader.IsDBNull(reader.GetOrdinal("FechaFinalizacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaFinalizacion")),
                                //CantidadHorasEstimadas = reader.IsDBNull(reader.GetOrdinal("CantidadHorasEstimadas")) ? 0 : reader.GetInt32(reader.GetOrdinal("CantidadHorasEstimadas")),
                                //CantidadHorasConsumidas = reader.IsDBNull(reader.GetOrdinal("CantidadHorasConsumidas")) ? 0 : reader.GetInt32(reader.GetOrdinal("CantidadHorasConsumidas")),
                                //Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? 0 : reader.GetInt32(reader.GetOrdinal("Estado")),
                                //PorcentajeAvance = reader.IsDBNull(reader.GetOrdinal("PorcentajeAvance")) ? 0 : reader.GetInt32(reader.GetOrdinal("PorcentajeAvance")),
                                //UsuarioSolicitante = reader.IsDBNull(reader.GetOrdinal("UsuarioSolicitante")) ? 0 : reader.GetInt32(reader.GetOrdinal("UsuarioSolicitante")),
                                //UsuarioResponsable = reader.IsDBNull(reader.GetOrdinal("UsuarioResponsable")) ? 0 : reader.GetInt32(reader.GetOrdinal("UsuarioResponsable")),
                                //Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion")),
                                //Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? string.Empty : reader.GetString(reader.GetOrdinal("Observaciones")),
                                //Prioridad = reader.IsDBNull(reader.GetOrdinal("Prioridad")) ? 0 : reader.GetInt32(reader.GetOrdinal("Prioridad")),
                                //FormulariosModificados = reader.IsDBNull(reader.GetOrdinal("FormulariosModificados")) ? string.Empty : reader.GetString(reader.GetOrdinal("FormulariosModificados")),
                                //ModificacionesBaseDatos = reader.IsDBNull(reader.GetOrdinal("ModificacionesBaseDatos")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificacionesBaseDatos")),
                                //UserIDSolicitante = reader.IsDBNull(reader.GetOrdinal("UserIDSolicitante")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserIDSolicitante")),
                                //UserIDResponsable = reader.IsDBNull(reader.GetOrdinal("UserIDResponsable")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserIDResponsable"))

                            };
                            //Se agrega a la lista de OTs en caso de que hay mas de una (Para la pestaña de busqueda de multiples OTs)
                            OrdenesTrabajo.Add(Orden);
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
            string consulta = "Select Descripcion, " + Tabla +" From " + Tabla + "s";
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
