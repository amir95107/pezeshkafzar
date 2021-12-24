using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace MyEshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class PriceUpdaterController : Controller
    {
        // GET: Admin/PriceUpdater
        medab_DBEntities db = new medab_DBEntities();
        public ActionResult Index()
        {
            return View();
        }

        private IList<IList<Object>> GetData()
        {
            string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
            string ApplicationName = "Pezeshkafzar Price Updater";
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "113B6GbzhMJkB-8mzgFOwaeAYDJdzE4eBjlJFTNRfcLE";
            String range = "Sheet1";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            return values;
        }

        
        [HttpPost]
        public int UpdateData()
        {
            IList<IList<Object>> values = GetData();
            int i = 0;
            if (values.Any())
            {
                List<Products> products = db.Products.ToList();
                
                foreach (var item in values)
                {
                    if (i != 0)
                    {
                        int productId = int.Parse(item[0].ToString());
                        if (products.Any(p => p.ProductID == productId))
                        {
                            var product = products.FirstOrDefault(p => p.ProductID == productId);
                            product.Price = int.Parse(item[2].ToString());
                            product.PriceAfterDiscount = int.Parse(item[3].ToString());
                            product.Stock = int.Parse(item[4].ToString());
                            product.LastUpdated = DateTime.Now;

                            db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    i++;
                }

                db.SaveChanges();
            }
            return i;
        }
    }
}