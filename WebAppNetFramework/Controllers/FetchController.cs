using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebAppNetFramework.Controllers
{
    public class FetchController : Controller
    {
        // GET: Fetch
        public async Task<ActionResult> Index()
        {
            List<string> pokemonList = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                // Make a GET request to the Pokemon API
                HttpResponseMessage response = await client.GetAsync("https://pokeapi.co/api/v2/pokemon?limit=20");

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize the response content to get the list of Pokemon names
                    PokemonResponse pokemonResponse = JsonConvert.DeserializeObject<PokemonResponse>(content);

                    // Get the names of the Pokemon
                    foreach (Pokemon pokemon in pokemonResponse.Results)
                    {
                        pokemonList.Add(pokemon.Name);
                    }
                }
                else
                {
                    // Handle the error response
                    // You can add your own error handling logic here
                    return View("Error");
                }
            }

            // Pass the Pokemon list to the view
            return View(pokemonList);
        }
    }

    // Define the models for deserializing the Pokemon API response
    public class PokemonResponse
    {
        public List<Pokemon> Results { get; set; }
    }

    public class Pokemon
    {
        public string Name { get; set; }
    }
}
