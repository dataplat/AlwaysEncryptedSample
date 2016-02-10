using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AlwayEncryptedSample.Models;

namespace AlwayEncryptedSample.Controllers
{
    [Authorize(Roles="Credit Card Admins")]
    public sealed class CreditCardsController : ControllerBase
    {
        public ActionResult Index()
        {
            return View(_appContext.CreditCards.ToList());
        }

        /// <summary>
        /// Import credit card data from a server side CSV
        /// </summary>
        /// <returns>Redirects to the Credit Card List</returns>
        /// <remarks>
        /// Very optomistic parsing because we are importing a static file we can trust.
        /// However, no bulk insert is happening.
        /// </remarks>
        public ActionResult Import()
        {
            //TODO: This might be slow according to dead link here.
            // http://stackoverflow.com/questions/697339/accessing-file-in-app-data-from-a-class-in-the-app-code-folder
            var csvFile = Path.Combine
                (HttpContext.ApplicationInstance.Request.PhysicalApplicationPath,
                 "App_Data", "CreditCards.csv");

            using (var file = System.IO.File.Open(csvFile, FileMode.Open, FileAccess.Read))
            using (var rdr = new StreamReader(file))
            {
                rdr.ReadLine(); // throw out headers
                var line = rdr.ReadLine();
                while (line != null)
                {
                    var fields = line.Split(',');
                    var cc = _appContext.CreditCards.Create();
                    cc.CardNumber = fields[0];
                    cc.ExpYear = short.Parse(fields[1]);
                    cc.ExpMonth = byte.Parse(fields[2]);
                    cc.CCV = short.Parse(fields[3]);
                    cc.Network = _appContext.CreditCardNetworks.ToList().First
                        (ccn => ccn.Id == (CreditCardNetworks) Enum.Parse(typeof(CreditCardNetworks), fields[4]));
                    _appContext.CreditCards.Add(cc);
                    line = rdr.ReadLine();
                }
                _appContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}