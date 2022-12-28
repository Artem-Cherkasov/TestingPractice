using Lab9.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NickBuhro.Translit;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Unidecode.NET;

namespace APITests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Shop client = new Shop();
        private readonly IConfigurationRoot config = new ConfigurationBuilder()
        .AddJsonFile("C:/Users/Àäìèí/ÒèÎÏÎ/Lab9_1/Lab9/APITests/TestEntities.json").Build();
        //private static JObject testEntities = JObject.Parse(File.ReadAllText("C:/Users/Àäìèí/ÒèÎÏÎ/Lab9_1/Lab9/APITests/TestEntities.json"));
        private readonly List<int> productIds = new();

        private bool AreEqual(Product p1, Product p2)
        {
            Assert.AreEqual(p1.title, p2.title);
            Assert.AreEqual(p1.content, p2.content);
            Assert.AreEqual(p1.price, p2.price);
            Assert.AreEqual(p1.status, p2.status);
            Assert.AreEqual(p1.keywords, p2.keywords);
            Assert.AreEqual(p1.description, p2.description);
            Assert.AreEqual(p1.hit, p2.hit);

            return true;
        }

        private static JToken? GetÑertainProduct(int id, JArray products)
        {
            foreach (var product in products)
            {
                if (product["id"]!.ToObject<int>() == id)
                    return product;
            }

            return null;
        }

        [TestCleanup]
        public async Task Delete()
        {
            foreach (var id in productIds)
                await client.DeleteProduct(id);
            productIds.Clear();
        }

        [TestMethod]
        public async Task Get_Not_Empty_Product_List()
        {
            var response = await client.GetProductsList();
            Assert.IsTrue(response.Count > 0);
        }

        [TestMethod]
        public async Task Add_Valid_Product()
        {
            //Arrange
            var product = config.GetSection("valid_item_1").Get<Product>();

            //Act
            var response = await client.CreateProduct(product);
            var productId = response["id"]!.ToObject<int>();
            productIds.Add(productId);
            var products = await client.GetProductsList();
            var createdProduct = GetÑertainProduct(productId, products).ToObject<Product>()!;
            Debug.Print(createdProduct.alias);

            //Assertion
            Assert.IsNotNull(createdProduct);
            Assert.IsTrue(AreEqual(product, createdProduct));
            //Assert.AreEqual(createdProduct.alias, createdProduct.title.ToLower());
        }

        [TestMethod]
        public async Task Add_Invalid_Product()
        {
            //Arrange
            var product = config.GetSection("invalid_item").Get<Product>();

            //Act
            var response = await client.CreateProduct(product);
            var productId = response["id"]!.ToObject<int>();
            productIds.Add(productId);
            var products = await client.GetProductsList();

            // À ïðîâåðèòü ñòàòóñ àïèøêè?
            //Assertion
            Assert.AreEqual(0, response["status"]!.ToObject<int>());
            Assert.IsNull(GetÑertainProduct(productId, products));
        }

        [TestMethod]
        public async Task Add_Same_Valid_Item_Few_Times()
        {
            //Arrange
            var product = config.GetSection("valid_item_1").Get<Product>();

            //Act
            var response1 = await client.CreateProduct(product);
            var response2 = await client.CreateProduct(product);
            var firstProductId = response1["id"]!.ToObject<int>();
            var secondProductId = response2["id"]!.ToObject<int>();
            productIds.Add(firstProductId);
            productIds.Add(secondProductId);
            var products = await client.GetProductsList();
            var firstCreatedProduct = GetÑertainProduct(firstProductId, products).ToObject<Product>()!;
            var secondCreatedProduct = GetÑertainProduct(secondProductId, products).ToObject<Product>()!;

            //Assertion
            Assert.IsNotNull(firstCreatedProduct);
            Assert.IsNotNull(secondCreatedProduct);
            Assert.IsTrue(AreEqual(product, firstCreatedProduct));
            Assert.IsTrue(AreEqual(product, secondCreatedProduct));
            Assert.AreEqual(firstCreatedProduct.alias, firstCreatedProduct.title.ToLower());
            Assert.AreEqual(secondCreatedProduct.alias, secondCreatedProduct.title.ToLower() + "-0");
        }

        [TestMethod]
        public async Task Update_Existing_Product()
        {
            //Arrange
            var product = config.GetSection("valid_item_1").Get<Product>();
            var updProduct = config.GetSection("update_valid_item").Get<Product>();

            //Act
            var response1 = await client.CreateProduct(product);
            var productId = response1["id"]!.ToObject<int>();
            productIds.Add(productId);
            updProduct.id = productId;
            // Ïî÷åìó íå èñïîëüçóåòñÿ?
            var response2 = await client.UpdateProduct(updProduct);
            var products = await client.GetProductsList();
            var resultProduct = GetÑertainProduct(productId, products).ToObject<Product>()!;

            //Assertion
            Assert.AreEqual(1, response2["status"].ToObject<int>());
            Assert.IsTrue(AreEqual(resultProduct, updProduct));
            Assert.AreEqual(resultProduct.alias, updProduct.title.ToLower());
        }

        [TestMethod]
        public async Task Update_Not_Existing_Product()
        {
            //Arrange
            var updProduct = config.GetSection("update_not_existing_item").Get<Product>();

            //Act
            var response = await client.UpdateProduct(updProduct);
            var resultStatus = response["status"].ToObject<int>();
            var products = await client.GetProductsList();
            var lastProduct = products.Last;
            var lastProductId = lastProduct["id"]!.ToObject<int>();
            productIds.Add(lastProductId);

            // Ïëîõèå àññåðòû
            //Assertion
            Assert.AreEqual(0, resultStatus);
            //Assert.IsNull(GetÑertainProduct());
            //Assert.IsTrue(AreEqual(updProduct, lastProduct.ToObject<Product>()));
            //Assert.AreEqual(lastProduct.ToObject<Product>().alias, updProduct.title.ToLower());
        }

        [TestMethod]
        public async Task Update_Product_With_Product_With_No_Id()
        {
            //Arrange
            var updProduct = config.GetSection("update_item_with_no_id").Get<Product>();

            //Act
            var response = await client.UpdateProduct(updProduct);

            //Arrange
            Assert.AreEqual(0, response["status"].ToObject<int>());
        }

        [TestMethod]
        public async Task Delete_Existing_Product()
        {
            //Arrange
            var product = config.GetSection("valid_item_1").Get<Product>();

            //Act
            var response = await client.CreateProduct(product);
            var productId = response["id"]!.ToObject<int>();
            productIds.Add(productId);
            var products = await client.GetProductsList();
            var deleteResponse = await client.DeleteProduct(productId);

            // Ïðîâåðèòü, ÷òî ïðîäóêò óäàëèëñÿ
            //Assertion
            Assert.AreEqual(1, deleteResponse["status"].ToObject<int>());
            Assert.IsNull(GetÑertainProduct(productId, products));
        }

        [TestMethod]
        public async Task Delete_Not_Existing_Product()
        {
            //Arrange
            var productId = 500000;

            //Act
            var deleteResponse = await client.DeleteProduct(productId);

            //Assertion
            Assert.AreEqual(0, deleteResponse["status"].ToObject<int>());
        }
    }
}
