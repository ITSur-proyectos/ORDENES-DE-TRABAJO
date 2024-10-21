using Sistema_OT.Services;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

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
        public static List<OrdenDeTrabajo> ObtenerLista(string consulta)
        {
           
            List<OrdenDeTrabajo> OrdenesTrabajo = new List<OrdenDeTrabajo>();
            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();
            using (SqlCommand command = new SqlCommand(consulta, conexionDB.con))

            {
                
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrdenDeTrabajo Orden = new OrdenDeTrabajo
                            {

                                //Le asigno a cada variable de la clase orden de trabajo su valor respectivo desde la base de datos, segun la consulta de arriba
                                NroOrdenTrabajo = reader.GetDecimal(reader.GetOrdinal("NroOrdenTrabajo")),
                                Cliente = reader.GetInt32(reader.GetOrdinal("Cliente")),
                                Sistema = reader.GetInt32(reader.GetOrdinal("Sistema")),
                                Modulo = reader.GetString(reader.GetOrdinal("Modulo")),
                                Asunto = reader.GetString(reader.GetOrdinal("Asunto")),
                                FechaSolicitud = reader.GetDateTime(reader.GetOrdinal("FechaSolicitud")),
                                FechaFinalizacion = reader.GetDateTime(reader.GetOrdinal("FechaFinalizacion")),
                                CantidadHorasEstimadas = reader.GetInt32(reader.GetOrdinal("CantidadHorasEstimadas")),
                                CantidadHorasConsumidas = reader.GetInt32(reader.GetOrdinal("CantidadHorasConsumidas")),
                                Estado = reader.GetInt32(reader.GetOrdinal("Estado")),
                                PorcentajeAvance = reader.GetInt32(reader.GetOrdinal("PorcentajeAvance")),
                                UsuarioSolicitante = reader.GetInt32(reader.GetOrdinal("UsuarioSolicitante")),
                                UsuarioResponsable = reader.GetInt32(reader.GetOrdinal("UsuarioResponsable")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Observaciones = reader.GetString(reader.GetOrdinal("Observaciones")),
                                Prioridad = reader.GetInt32(reader.GetOrdinal("Prioridad")),
                                FormulariosModificados = reader.GetString(reader.GetOrdinal("FormulariosModificados")),
                                ModificacionesBaseDatos = reader.GetString(reader.GetOrdinal("ModificacionesBaseDatos")),
                                UserIDSolicitante = reader.GetString(reader.GetOrdinal("UserIDSolicitante")),
                                UserIDResponsable = reader.GetString(reader.GetOrdinal("UserIDResponsable"))
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
    }

}
