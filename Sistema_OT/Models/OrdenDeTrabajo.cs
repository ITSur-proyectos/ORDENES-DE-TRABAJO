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
        public string Cliente { get; set; } //antes eran int
        public string Sistema { get; set; }
        public string Proyecto { get; set; }
        public int DependeDe { get; set; }
        public string Modulo { get; set; }
        public string Asunto { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public int CantidadHorasEstimadas { get; set; }
        public int CantidadHorasConsumidas { get; set; }
        //public int HorasInsumidas { get; set; }
        public int Estado { get; set; }
        public int PorcentajeAvance { get; set; }
        public string UsuarioSolicitante { get; set; }
        public string UsuarioResponsable { get; set; }
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
            string consulta = "Select Descripcion, " + Tabla + " From " + Tabla + "s" + " ORDER BY Descripcion ASC"; // +s porque menos mal que las tablas estan en plural
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


        public static void Actualizar(OrdenDeTrabajo orden)
        {
            ConexionDB conexion = new ConexionDB();
            conexion.AbrirConexion();

            using (SqlCommand cmd = new SqlCommand("sp_Actualizar_OrdenDeTrabajoConDescripciones", conexion.con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NroOrdenTrabajo", orden.NroOrdenTrabajo);

                // Validar nulos para evitar errores
                cmd.Parameters.AddWithValue("@Asunto", !string.IsNullOrEmpty(orden.Asunto) ? (object)orden.Asunto : DBNull.Value);
                cmd.Parameters.AddWithValue("@Descripcion", !string.IsNullOrEmpty(orden.Descripcion) ? (object)orden.Descripcion : DBNull.Value);

                cmd.ExecuteNonQuery();
            }
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

        //public static string LimpiarRTF(string rtf)
        //{
        //    if (string.IsNullOrWhiteSpace(rtf))
        //        return string.Empty;

        //    try
        //    {
        //        string texto = rtf;

        //        // 1. Decodifica caracteres hexadecimales RTF (\'xx)
        //        texto = Regex.Replace(texto, @"\\'([0-9a-fA-F]{2})", match =>
        //        {
        //            var hex = match.Groups[1].Value;
        //            byte b = Convert.ToByte(hex, 16);
        //            return Encoding.GetEncoding(1252).GetString(new byte[] { b });
        //        });

        //        // 1b. Decodifica unicode RTF \u1234?
        //        texto = Regex.Replace(texto, @"\\u(-?\d+)\?", match =>
        //        {
        //            int code = int.Parse(match.Groups[1].Value);
        //            return char.ConvertFromUtf32(code);
        //        });

        //        // 2. Reemplaza \par por salto de línea
        //        texto = texto.Replace(@"\par", "\n");

        //        // 3. Elimina comandos RTF comunes
        //        texto = Regex.Replace(texto, @"\\[a-zA-Z]+\d* ?", "");

        //        // 4. Elimina llaves y barras sobrantes
        //        texto = texto.Replace("{", "").Replace("}", "").Replace("\\", "");

        //        // 5. Limpieza línea a línea con filtros para ambas descripciones
        //        var lineasFiltradas = texto
        //            .Split('\n')
        //            .Select(l => l.Trim())
        //            .Where(l =>
        //            {
        //                if (string.IsNullOrWhiteSpace(l))
        //                    return false;

        //                // Filtrar líneas basura primera descripción (Arial, Symbol, TX_RTF32, fecha ddd-dd-dddd)
        //                if (Regex.IsMatch(l, @"^(Arial;?|Symbol;?|TX_RTF32\s*\d+(\.\d+)*|d\d{2}-\d{2}-\d{4}:?)$", RegexOptions.IgnoreCase))
        //                    return false;

        //                // Filtrar líneas basura segunda descripción (Times New Roman, Courier, Calibri, Verdana, Normal, etc.)
        //                if (Regex.IsMatch(l, @"^(Times New Roman;?|Courier New;?|Calibri;?|Courier;?|Tahoma;?|Verdana;?|Normal;?|heading \d;?|\*Default Paragraph Font;?|\[Normal\];?|Plain Text;?|H\d;?|Body Text;?|Body Text \d;?|Body Text Indent \d;?|header;?|Blockquote;?|Normal \(Web\);?|Address;?|E-mail Signature;?|HTML Address;?|msolistparagraph;?|List Paragraph;?|HTML Preformatted;?|Preformatted;?|Párrafo de lista;?|normal\d?;?|a4;?|J_Titulo \d;?|annotation text;?|x_msonormal;?|x_x_xmsolistparagraph;?|p-mail;?|x_xxxxxmsonormal;?|x_xmsolistparagraph;?|x_xmsonormal;?|x_msolistparagraph;?|contentpasted\d+;?|elementtoproof;?|\*Hyperlink;?|\*Strong;?|\*Emphasis;?|\*HTML Markup;?|\*spelle;?|\*Typewriter;?|\*Título \d Car;?|\*apple-style-span;?|\*apple-tab-span;?|\*CODE;?|\*fontblack;?|\*fontmediumbold\d+;?|\*yui\d+;?|\*HTML Typewriter;?|\*hoenzb;?|Default;?|s\d+;?|\*contentpasted\d+;?|\*x_bx-messenger-message;?|\*x_contentpasted\d+;?|\*x_bx-messenger-content-like-button;?|\*x_bx-messenger-content-item-date;?|\*ui-provider;?|\*Smart Link;?)$", RegexOptions.IgnoreCase))
        //                    return false;

        //                // Líneas solo con punto y coma
        //                if (Regex.IsMatch(l, @"^;+$"))
        //                    return false;

        //                // Líneas muy largas con muchas fuentes o estilos, por ejemplo más de 100 caracteres y más de 10 ';'
        //                if (l.Length > 100 && l.Count(c => c == ';') > 10)
        //                    return false;

        //                // Filtrar líneas que sean solo 'd' repetido, o 'd' con espacios o asterisco (ejemplos: "dddddd", "d *", "d*")
        //                if (Regex.IsMatch(l, @"^d[\s\*]*$", RegexOptions.IgnoreCase))
        //                    return false;

        //                if (Regex.IsMatch(l, @"^d+$", RegexOptions.IgnoreCase) && l.Length > 10)
        //                    return false;

        //                // Mantener líneas que son solo "d" + números, ejemplo "d17664"
        //                if (Regex.IsMatch(l, @"^d\d+$", RegexOptions.IgnoreCase))
        //                    return true;

        //                // Mantener el resto
        //                return true;
        //            });

        //        texto = string.Join("\n", lineasFiltradas);

        //        // 6. Normaliza saltos de línea y espacios excesivos
        //        texto = Regex.Replace(texto, @"\n{2,}", "\n\n");
        //        texto = Regex.Replace(texto, @"[ \t]{2,}", " ");

        //        // 7. Trim final
        //        texto = texto.Trim();

        //        return texto;
        //    }
        //    catch
        //    {
        //        return rtf;
        //    }
        //}


        public static string LimpiarRTF(string rtf)
        {
            if (string.IsNullOrWhiteSpace(rtf))
                return string.Empty;

            try
            {
                string texto = rtf;

                // 1. Decodifica caracteres hexadecimales RTF (\'xx)
                texto = Regex.Replace(texto, @"\\'([0-9a-fA-F]{2})", match =>
                {
                    var hex = match.Groups[1].Value;
                    byte b = Convert.ToByte(hex, 16);
                    return Encoding.GetEncoding(1252).GetString(new byte[] { b });
                });

                // 1b. Decodifica unicode RTF \u1234?
                texto = Regex.Replace(texto, @"\\u(-?\d+)\?", match =>
                {
                    int code = int.Parse(match.Groups[1].Value);
                    return char.ConvertFromUtf32(code);
                });

                // 2. Reemplaza \par por salto de línea
                texto = texto.Replace(@"\par", "\n");

                // 3. Elimina comandos RTF comunes
                texto = Regex.Replace(texto, @"\\[a-zA-Z]+\d* ?", "");

                // 4. Elimina llaves y barras sobrantes
                texto = texto.Replace("{", "").Replace("}", "").Replace("\\", "");

                // 5. Limpieza línea a línea con filtros para ambas descripciones
                var lineasFiltradas = texto
                    .Split('\n')
                    .Select(l => l.Trim())
                    .Where(l =>
                    {
                        if (string.IsNullOrWhiteSpace(l))
                            return false;

                        // Filtrar líneas basura primera descripción (Arial, Symbol, TX_RTF32, fecha ddd-dd-dddd)
                        if (Regex.IsMatch(l, @"^(Arial;?|Symbol;?|TX_RTF32\s*\d+(\.\d+)*|d\d{2}-\d{2}-\d{4}:?)$", RegexOptions.IgnoreCase))
                            return false;

                        // Filtrar líneas basura segunda descripción (fuentes, estilos, etc.)
                        // Aquí extendemos para filtrar líneas largas con esas palabras clave repetidas
                        if (Regex.IsMatch(l, @"^(Times New Roman;?|Courier New;?|Calibri;?|Courier;?|Tahoma;?|Verdana;?|Normal;?|heading \d;?|\*Default Paragraph Font;?|\[Normal\];?|Plain Text;?|H\d;?|Body Text;?|Body Text \d;?|Body Text Indent \d;?|header;?|Blockquote;?|Normal \(Web\);?|Address;?|E-mail Signature;?|HTML Address;?|msolistparagraph;?|List Paragraph;?|HTML Preformatted;?|Preformatted;?|Párrafo de lista;?|normal\d?;?|a4;?|J_Titulo \d;?|annotation text;?|x_msonormal;?|x_x_xmsolistparagraph;?|p-mail;?|x_xxxxxmsonormal;?|x_xmsolistparagraph;?|x_xmsonormal;?|x_msolistparagraph;?|contentpasted\d+;?|elementtoproof;?|\*Hyperlink;?|\*Strong;?|\*Emphasis;?|\*HTML Markup;?|\*spelle;?|\*Typewriter;?|\*Título \d Car;?|\*apple-style-span;?|\*apple-tab-span;?|\*CODE;?|\*fontblack;?|\*fontmediumbold\d+;?|\*yui\d+;?|\*HTML Typewriter;?|\*hoenzb;?|Default;?|s\d+;?|\*contentpasted\d+;?|\*x_bx-messenger-message;?|\*x_contentpasted\d+;?|\*x_bx-messenger-content-like-button;?|\*x_bx-messenger-content-item-date;?|\*ui-provider;?|\*Smart Link;?)+$", RegexOptions.IgnoreCase))
                            return false;

                        // Líneas solo con punto y coma
                        if (Regex.IsMatch(l, @"^;+$"))
                            return false;

                        // Líneas muy largas con muchas fuentes o estilos, por ejemplo más de 100 caracteres y más de 10 ';'
                        if (l.Length > 100 && l.Count(c => c == ';') > 10)
                            return false;

                        // Filtrar líneas que sean solo 'd' repetido, o 'd' con espacios o asterisco (ejemplos: "dddddd", "d *", "d*")
                        if (Regex.IsMatch(l, @"^d[\s\*]*$", RegexOptions.IgnoreCase))
                            return false;

                        if (Regex.IsMatch(l, @"^d+$", RegexOptions.IgnoreCase) && l.Length > 10)
                            return false;

                        // Mantener líneas que son solo "d" + números, ejemplo "d17664"
                        if (Regex.IsMatch(l, @"^d\d+$", RegexOptions.IgnoreCase))
                            return true;

                        // Mantener el resto
                        return true;
                    });

                texto = string.Join("\n", lineasFiltradas);

                // 6. Normaliza saltos de línea y espacios excesivos
                texto = Regex.Replace(texto, @"\n{2,}", "\n\n");
                texto = Regex.Replace(texto, @"[ \t]{2,}", " ");

                // 7. Trim final
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