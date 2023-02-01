using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lab9.Entities
{
    public class Shop
    {
        private readonly string uri = "http://shop.qatl.ru/";
        private readonly string getProducts = "api/products";
        private readonly string addProduct = "api/addproduct";
        private readonly string deleteProduct = "api/deleteproduct";
        private readonly string updateProduct = "api/editproduct";
        private readonly HttpClient client;

        public Shop()
        {
            client = new(new HttpClientHandler()) { BaseAddress = new Uri(uri) };
        }

        public async Task<JArray> GetProductsList()
        {
            var response = await client.GetAsync(uri + getProducts);

            var responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            var responseContentString = responseContent!.ToString();

            var result = JArray.Parse(responseContentString);
            
            return result;
        }

        public async Task<JObject> CreateProduct(Product product)
        {
            var jsonObj = JsonConvert.SerializeObject(product);

            var requestContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri + addProduct, requestContent);

            var responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            var responseContentString = responseContent!.ToString();

            var result = JObject.Parse(responseContentString);            

            return result;
        }

        public async Task<JObject> DeleteProduct(int productId)
        {
            var response = await client.GetAsync(uri + deleteProduct + $"?id={productId}");

            var responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            var responseContentString = responseContent!.ToString();

            var result = JObject.Parse(responseContentString);

            return result;
        }

        public async Task<JObject> UpdateProduct(Product product)
        {
            var jsonObj = JsonConvert.SerializeObject(product);

            var requestContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri + updateProduct, requestContent);

            var responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            var responseContentString = responseContent!.ToString();

            var result = JObject.Parse(responseContentString);

            return result;
        }
    }
}
