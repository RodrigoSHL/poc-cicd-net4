using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAppNetFramework.Models;
namespace WebAppMvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly string apiUrl = "https://localhost:44376/api/product";

        public async Task<ActionResult> Index()
        {
            List<Product> products = new List<Product>();

            try
            {
                // Ignorar errores de certificado SSL en desarrollo
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync("");
                    response.EnsureSuccessStatusCode(); // Lanzará una excepción si el código de estado no es exitoso
                    products = await response.Content.ReadAsAsync<List<Product>>();
                }
            }
            catch (HttpRequestException e)
            {
                // Maneja excepciones de solicitud HTTP aquí
                ModelState.AddModelError(string.Empty, "Error al obtener los productos de la API. Detalles: " + e.Message);
            }
            catch (Exception e)
            {
                // Maneja otras excepciones aquí
                ModelState.AddModelError(string.Empty, "Error inesperado: " + e.Message);
            }

            return View(products);
        }
    }
}
