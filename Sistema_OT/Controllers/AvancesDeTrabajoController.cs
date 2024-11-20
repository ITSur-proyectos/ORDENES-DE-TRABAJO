using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_OT.Models;
using System.Data.SqlClient;

namespace Sistema_OT.Controllers
{
    public class AvancesDeTrabajoController : Controller

    {
        private readonly string _connectionString = "Data Source=192.168.110.5;Initial Catalog=Db_ITSur_CSharp;User ID=sa;Password=felisa5";

        // GET: AvancesDeTrabajoController
        public ActionResult Index()
        {
            return View("VistaIndividual");

        }

        private List<AvanceDeTrabajo> GetAllAvances()
        {
            //cambiar el nombre del Model
            var avances = new List<AvanceDeTrabajo>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "select * from Avances_Trabajo";
                using (var command = new SqlCommand(query, connetion)) ;
                {
                    connection.Open();
                    using (var reader = commnad.ExecuteReader())
                    {
        {
                       
                    }
            }
        }        


        // GET: AvancesDeTrabajoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AvancesDeTrabajoController/Create
        public ActionResult Create()
        {
            Console.WriteLine("Estoy en el controlador");
            return View();
        }

        // POST: AvancesDeTrabajoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
