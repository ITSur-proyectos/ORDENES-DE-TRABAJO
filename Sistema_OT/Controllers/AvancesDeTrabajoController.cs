using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_OT.Models;
using System.Data.SqlClient;
using System;
namespace Sistema_OT.Controllers
{
    public class AvancesDeTrabajoController : Controller

    {
        private readonly string _connectionString = "Data Source=192.168.110.5;Initial Catalog=Db_ITSur_CSharp;User ID=sa;Password=felisa5";

        // GET: AvancesDeTrabajoController
        public ActionResult Index()
        {
            var avances= GetAllAvances();
            return View("index", avances);

        }

        private List<AvanceDeTrabajo> GetAllAvances()
        {
            var avances = new List<AvanceDeTrabajo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Avances_Trabajo where NroOrdenTrabajo= 17658" ;
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            avances.Add(new AvanceDeTrabajo
                            {
                                NroOrdenTrabajo = reader["NroOrdenTrabajo"] != DBNull.Value ? (decimal)reader["NroOrdenTrabajo"] : 0,
                                AvanceTrabajo = reader["AvanceTrabajo"] != DBNull.Value ? (int)reader["AvanceTrabajo"] : 0,
                                Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : string.Empty,
                                Fecha = reader["Fecha"] != DBNull.Value ? (DateTime)reader["Fecha"] : DateTime.MinValue,
                                HorasInsumidas = reader["HorasInsumidas"] != DBNull.Value ? (decimal)reader["HorasInsumidas"] : 0,
                                Terminado = reader["Terminado"] != DBNull.Value ? Convert.ToChar(reader["Terminado"]) : 'N', // Default 'N'
                                FechaAlta = reader["FechaAlta"] != DBNull.Value ? (DateTime)reader["FechaAlta"] : DateTime.MinValue,
                                UserIDAlta = reader["UserIDAlta"] != DBNull.Value ? reader["UserIDAlta"].ToString() : string.Empty,
                                FechaUltimaModificacion = reader["FechaUltimaModificacion"] != DBNull.Value ? (DateTime)reader["FechaUltimaModificacion"] : DateTime.MinValue,
                                UserIDModificacion = reader["UserIDModificacion"] != DBNull.Value ? reader["UserIDModificacion"].ToString() : string.Empty,
                                FechaBaja = reader["FechaBaja"] != DBNull.Value ? (DateTime)reader["FechaBaja"] : DateTime.MinValue,
                                UserIDBaja = reader["UserIDBaja"] != DBNull.Value ? reader["UserIDBaja"].ToString() : string.Empty,
                                Usuario = reader["Usuario"] != DBNull.Value ? (int)reader["Usuario"] : 0,
                                TipoAvance = reader["TipoAvance"] != DBNull.Value ? (int)reader["TipoAvance"] : 0
                            });
                        }
                    }
                }
            }

            return avances;
        }



        public void InsertarAvance(AvanceDeTrabajo avance)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO Avances_Trabajo
            (
                NroOrdenTrabajo, 
                AvanceTrabajo, 
                Descripcion, 
                Fecha, 
                HorasInsumidas, 
                Terminado, 
                FechaAlta, 
                UserIDAlta, 
                FechaUltimaModificacion, 
                UserIDModificacion, 
                FechaBaja, 
                UserIDBaja, 
                Usuario, 
                TipoAvance
            )
            VALUES
            (
                @NroOrdenTrabajo, 
                @AvanceTrabajo, 
                @Descripcion, 
                @Fecha, 
                @HorasInsumidas, 
                @Terminado, 
                @FechaAlta, 
                @UserIDAlta, 
                @FechaUltimaModificacion, 
                @UserIDModificacion, 
                @FechaBaja, 
                @UserIDBaja, 
                @Usuario, 
                @TipoAvance
            )";

                using (var command = new SqlCommand(query, connection))
                {
                    // Parametrizamos los valores para prevenir inyecciones SQL
                    command.Parameters.AddWithValue("@NroOrdenTrabajo", 17658); // Valor de NroOrdenTrabajo
                    command.Parameters.AddWithValue("@AvanceTrabajo", 20);      // Valor de AvanceTrabajo
                    command.Parameters.AddWithValue("@Descripcion", "Continuar");
                    command.Parameters.AddWithValue("@Fecha", DateTime.Parse("2024-08-28 00:00:00"));
                    command.Parameters.AddWithValue("@HorasInsumidas", 3.0);     // Valor de HorasInsumidas
                    command.Parameters.AddWithValue("@Terminado", DBNull.Value);  // NULL para Terminado
                    command.Parameters.AddWithValue("@FechaAlta", DateTime.Now);  // Fecha de alta (actual)
                    command.Parameters.AddWithValue("@UserIDAlta", "ERUIZ");     // Usuario que realiza la acción
                    command.Parameters.AddWithValue("@FechaUltimaModificacion", DBNull.Value); // NULL si no hay modificación
                    command.Parameters.AddWithValue("@UserIDModificacion", DBNull.Value); // NULL si no hay modificación
                    command.Parameters.AddWithValue("@FechaBaja", DBNull.Value); // NULL si no se ha dado de baja
                    command.Parameters.AddWithValue("@UserIDBaja", DBNull.Value); // NULL si no se ha dado de baja
                    command.Parameters.AddWithValue("@Usuario", 48);              // ID del usuario
                    command.Parameters.AddWithValue("@TipoAvance", 2);            // TipoAvance (ajustar según necesidad)

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    


        // GET: AvancesDeTrabajoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(AvanceDeTrabajo avance)
        {
         
                //valores automaticos
                avance.FechaAlta = DateTime.Now;
                avance.UserIDAlta = "ERUIZ";
                avance.Usuario = 48;

                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"INSERT INTO Avances_Trabajo 
                          (NroOrdenTrabajo, AvanceTrabajo, Descripcion, HorasInsumidas, Terminado, FechaAlta, UserIDAlta, Usuario) 
                          VALUES (@NroOrdenTrabajo, @AvanceTrabajo, @Descripcion, @HorasInsumidas, @Terminado, @FechaAlta, @UserIDAlta, @Usuario)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NroOrdenTrabajo", avance.NroOrdenTrabajo);
                        command.Parameters.AddWithValue("@AvanceTrabajo", (object)avance.AvanceTrabajo ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Descripcion", (object)avance.Descripcion ?? DBNull.Value);
                        command.Parameters.AddWithValue("@HorasInsumidas", (object)avance.HorasInsumidas ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Terminado", (object)avance.Terminado ?? DBNull.Value);
                        command.Parameters.AddWithValue("@FechaAlta", avance.FechaAlta);
                        command.Parameters.AddWithValue("@UserIDAlta", avance.UserIDAlta);
                        command.Parameters.AddWithValue("@Usuario", avance.Usuario);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }


            return RedirectToAction("Index");
        }

        // POST: AvancesDeTrabajoController/Create



        // GET: AvancesDeTrabajoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AvancesDeTrabajoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AvancesDeTrabajoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AvancesDeTrabajoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
