using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Lab3;

namespace ShopBusketTests
{
    [TestClass]
    public class InitClassTests
    {
        [DataTestMethod] // ƒобавить параметры и сократить до 1 теста (ѕогугли, как передавать в один тест разные значени€ переменной)
        [DataRow(20)]
        [DataRow(20.5)]
        [DataRow(0)]
        public void Put_positive_amount_money_in_the_busket(double value)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket(value);

            //Assert
            Assert.AreEqual(value, shopBusket.GetMoney());
            Assert.AreEqual("", shopBusket.GetPurchases());
        }

        [DataTestMethod]
        [DataRow(-20)]
        public void Put_negative_amount_money_in_the_busket(int value)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket(value);

            //Assert
            Assert.AreEqual(0, shopBusket.GetMoney());
            Assert.AreEqual("", shopBusket.GetPurchases());
        }
    }

    [TestClass]
    public class PutProductsTests
    {
        [DataTestMethod]
        [DataRow(new[] { "€блоко" }, "€блоко: 1", 1)]
        [DataRow(new[] { "€блоко", "€блоко" }, "€блоко: 2", 2)]
        [DataRow(new[] { "€блоко", "хлеб" }, "€блоко: 1, хлеб: 1", 2)]
        [DataRow(new[] { "€блоко", "хлеб", "€блоко" }, "€блоко: 2, хлеб: 1", 3)]
        public void Put_items_in_busket(string[] purchaseList, string productsInBusket, int amountOfProducts)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket();
            
            //Act
            shopBusket.PutPurchasesInBusket(purchaseList);

            //Assertion
            Assert.AreEqual(productsInBusket, shopBusket.GetPurchases());
            Assert.AreEqual(amountOfProducts, shopBusket.SeeAmountOfProducts()); // ƒобавить проверку кол-ва продуктов в корзине
        }


        [DataTestMethod]
        [DataRow(new[] { "холодильник" })]
        public void Put_undefined_items_in_busket(string[] purchaseList)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket();

            //Assertion
            Assert.ThrowsException<Exception>(() => { shopBusket.PutPurchasesInBusket(purchaseList); });
        }
    }

    [TestClass]
    public class PriceCountTests
    {
        [DataTestMethod]
        [DataRow(new[] { "€блоко" }, 10)]
        [DataRow(new[] { "€блоко", "€блоко" }, 20)]
        [DataRow(new[] { "€блоко", "хлеб" }, 15)]
        [DataRow(new[] { "€блоко", "хлеб", "€блоко" }, 25)]
        public void TestPriceForOneItem(string[] purchaseList, int price)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket();

            //Act
            shopBusket.PutPurchasesInBusket(purchaseList);

            //Assertion
            Assert.AreEqual(price, shopBusket.SeeTotalPrice());
        }

        [TestMethod]
        public void TestPriceForEmptyBusket()
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket();
            string[] purchaseList = System.Array.Empty<string>();

            //Act
            shopBusket.PutPurchasesInBusket(purchaseList);

            //Assertion
            Assert.AreEqual(0, shopBusket.SeeTotalPrice());
        }

        [TestMethod]
        public void TestPriceForManyEqualItems()
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket();
            string[] purchaseList = Enumerable.Repeat("€блоко", 100).ToArray();

            //Act
            shopBusket.PutPurchasesInBusket(purchaseList);

            //Assertion
            Assert.AreEqual(1000, shopBusket.SeeTotalPrice());
        }  
    }

    [TestClass]
    public class ItemsDeleteTests
    {
        [DataTestMethod]
        [DataRow(new[] { "€блоко" })]
        [DataRow(new[] { "€блоко", "€блоко" })]
        [DataRow(new[] { "€блоко", "хлеб" })]
        [DataRow(new[] { "€блоко", "хлеб", "€блоко" })]
        public void Delete_all_products(string[] purchaseList)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket();

            //Act
            shopBusket.PutPurchasesInBusket(purchaseList);
            shopBusket.DeleteProducts(purchaseList);

            //Assertion
            Assert.AreEqual("", shopBusket.GetPurchases());
            Assert.AreEqual(0, shopBusket.SeeAmountOfProducts()); // ƒобавить проверку кол-ва продуктов в корзине
        }

        [DataTestMethod]
        [DataRow(new[] { "€блоко", "€блоко" }, new[] { "€блоко" }, "€блоко: 1", 1)]
        [DataRow(new[] { "€блоко", "хлеб" }, new[] { "€блоко" }, "хлеб: 1", 1)]
        [DataRow(new[] { "€блоко", "€блоко", "хлеб" }, new[] { "€блоко" }, "€блоко: 1, хлеб: 1", 2)]
        public void Delete_some_products(string[] purchaseList, string[] deleteList, string productString, int productAmount)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket();

            //Act
            shopBusket.PutPurchasesInBusket(purchaseList);
            shopBusket.DeleteProducts(deleteList);

            //Assertion
            Assert.AreEqual(productString, shopBusket.GetPurchases());
            Assert.AreEqual(productAmount, shopBusket.SeeAmountOfProducts());
        }
    }

    [TestClass]
    public class BuyProductsTests
    {
        [DataTestMethod]
        [DataRow(5, new[] { "хлеб" }, 0)]
        [DataRow(10, new[] { "хлеб" }, 5)]
        [DataRow(10, new[] { "хлеб", "хлеб" }, 0)]
        [DataRow(15, new[] { "хлеб", "€блоко" }, 0)]
        public void Buy_products_from_busket(int money, string[] purchaseList, int remainingMoney)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket(money);

            //Act
            shopBusket.PutPurchasesInBusket(purchaseList);
            shopBusket.BuyProducts();

            //Assertion
            Assert.AreEqual("", shopBusket.GetPurchases());
            Assert.AreEqual(0, shopBusket.SeeAmountOfProducts());
            Assert.AreEqual(remainingMoney, shopBusket.GetMoney());
        }

        [DataTestMethod]        
        [DataRow(new[] { "€блоко" }, 0)] // ƒобавить тест, где денег = 0
        [DataRow(new[] { "€блоко" }, 5)]
        public void Buy_products_on_low_money(string[] purchaseList, int money)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket(money);

            //Act
            shopBusket.PutPurchasesInBusket(purchaseList);

            //Assertion
            Assert.ThrowsException<Exception>(() => { shopBusket.BuyProducts(); });
        }
    }
}
