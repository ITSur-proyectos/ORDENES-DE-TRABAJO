using Sistema_OT.Services;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Text;

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
        //public int HorasInsumidas { get; set; }
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
                                OrdenDeTrabajo ordenTrabajo = new OrdenDeTrabajo();
                                orden["DescripcionPlano"] = LimpiarRTF(orden.ContainsKey("Descripcion") ? orden["Descripcion"].ToString() : string.Empty);
                                orden["FormulariosModificadosPlano"] = LimpiarRTF(orden.ContainsKey("FormulariosModificados") ? orden["FormulariosModificados"].ToString() : string.Empty);
                                orden["ModificacionesBaseDatosPlano"] = LimpiarRTF(orden.ContainsKey("ModificacionesBaseDatos") ? orden["ModificacionesBaseDatos"].ToString() : string.Empty);


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


            // Preparar campos a consultar según la tabla
            string campos = Tabla;
            if (Tabla == "Proyecto")
            {
                campos = $"{Tabla}, Nombre, Descripcion";
            }
            else
            {
                campos = $"{Tabla}, Descripcion";
            }
            string consulta = "Select " + campos +  Tabla + " From " + Tabla + "s" + " ORDER BY Descripcion ASC"; // +s porque menos mal que las tablas estan en plural
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

        public string DescripcionPlano
        {
            get
            {
                return LimpiarRTF(Descripcion);
            }
        }

        public string FormulariosModificadosPlano
        {
            get
            {
                return LimpiarRTF(FormulariosModificados);
            }
        }

        public string ModificacionesBaseDatosPlano
        {
            get
            {
                return LimpiarRTF(ModificacionesBaseDatos);
            }
        }

        public static string LimpiarRTF(string rtf)
        {
            if (string.IsNullOrWhiteSpace(rtf))
                return string.Empty;

            try
            {
                // 1. Decodifica los caracteres hexadecimales RTF: \'f3 => ó
                string texto = Regex.Replace(rtf, @"\\'([0-9a-fA-F]{2})", match =>
                {
                    var hex = match.Groups[1].Value;
                    byte b = Convert.ToByte(hex, 16);
                    return Encoding.GetEncoding(1252).GetString(new byte[] { b });
                });

                // Opción robusta con Replace (solo si sabés que siempre vendrá igual)
                texto = texto.Replace("Arial;\r\n Symbol;\r\n;;;\r\n [Normal];* Default Paragraph Font;\r\n TX_RTF32 9.0.310.500", "");
                texto = texto.Replace("Arial;\n Symbol;\n;;;\n [Normal];* Default Paragraph Font;\n TX_RTF32 9.0.310.500", "");

                // 2. Reemplaza \par (fin de párrafo RTF) con salto de línea
                texto = texto.Replace(@"\par", "\n");

                // 3. Elimina comandos RTF comunes como \fs24, \b0, \f0, etc.
                texto = Regex.Replace(texto, @"\\[a-z]+\d*", "");

                // 4. Elimina llaves {, } y barras \ sobrantes
                texto = texto.Replace("{", "").Replace("}", "").Replace("\\", "");

                // 5. Normaliza saltos de línea y espacios
                texto = Regex.Replace(texto, @"\n{3,}", "\n\n");  // máximo 2 saltos seguidos
                texto = Regex.Replace(texto, @"[ \t]{2,}", " ");  // espacios excesivos


                // Opción: quitar encabezado literal si está presente
                string cabecera = "Arial; Symbol; ;;; [Normal];* Default Paragraph Font; TX_RTF32 9.0.310.500";
                texto = texto.Replace(cabecera, "").Trim();

                // Eliminar la "d" al principio si está presente
                texto = Regex.Replace(texto, @"^d\s*", "");  // Quita "d" y los espacios después de ella


                // Si querés limpiar el encabezado con Regex para mayor flexibilidad, reemplázalo por esto:
                texto = Regex.Replace(
                    texto,
                    @"Arial;\s*Symbol;\s*;;;\s*\[Normal\];\* Default Paragraph Font;\s*TX_RTF32 9\.0\.310\.500",
                    "",
                    RegexOptions.IgnoreCase);

                texto = texto.Trim();

                return texto;
            }
            catch
            {
                return rtf;
            }
        }


        public static int EjecutarInsert(string consulta, Dictionary<string, object> parametrosSP, out int nroOrdenGenerado)
        {
            nroOrdenGenerado = 0;  // Inicializamos en 0 para evitar null
            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();

            using (SqlCommand command = new SqlCommand(consulta, conexionDB.con))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Añadir parámetros de entrada
                foreach (var param in parametrosSP)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                // Agregar parámetro de salida para el número de orden
                SqlParameter outputParam = new SqlParameter("@NroOrdenGenerado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputParam);

                try
                {
                    // Ejecutar la consulta
                    command.ExecuteNonQuery();

                    // Obtener el valor del parámetro de salida
                    nroOrdenGenerado = outputParam.Value != DBNull.Value ? (int)outputParam.Value : 0;

                    return 1;  // Si no hubo errores
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error al ejecutar INSERT: " + e.ToString());
                    return 0;  // Si ocurrió un error
                }
            }
        }







    }
}