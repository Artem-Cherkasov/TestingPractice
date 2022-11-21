using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Lab3;

namespace ShopBusketTests
{
    [TestClass]
    public class InitClassTests
    {
        [DataTestMethod] // Добавить параметры и сократить до 1 теста (Погугли, как передавать в один тест разные значения переменной)
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
        [DataRow(new[] { "яблоко" }, "яблоко: 1", 1)]
        [DataRow(new[] { "яблоко", "яблоко" }, "яблоко: 2", 2)]
        [DataRow(new[] { "яблоко", "хлеб" }, "яблоко: 1, хлеб: 1", 2)]
        [DataRow(new[] { "яблоко", "хлеб", "яблоко" }, "яблоко: 2, хлеб: 1", 3)]
        public void Put_items_in_busket(string[] purchaseList, string productsInBusket, int amountOfProducts)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket();
            
            //Act
            shopBusket.PutPurchasesInBusket(purchaseList);

            //Assertion
            Assert.AreEqual(productsInBusket, shopBusket.GetPurchases());
            Assert.AreEqual(amountOfProducts, shopBusket.SeeAmountOfProducts()); // Добавить проверку кол-ва продуктов в корзине
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
        [DataRow(new[] { "яблоко" }, 10)]
        [DataRow(new[] { "яблоко", "яблоко" }, 20)]
        [DataRow(new[] { "яблоко", "хлеб" }, 15)]
        [DataRow(new[] { "яблоко", "хлеб", "яблоко" }, 25)]
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
            string[] purchaseList = Enumerable.Repeat("яблоко", 100).ToArray();

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
        [DataRow(new[] { "яблоко" })]
        [DataRow(new[] { "яблоко", "яблоко" })]
        [DataRow(new[] { "яблоко", "хлеб" })]
        [DataRow(new[] { "яблоко", "хлеб", "яблоко" })]
        public void Delete_all_products(string[] purchaseList)
        {
            //Arrange
            ShopBusket shopBusket = new ShopBusket();

            //Act
            shopBusket.PutPurchasesInBusket(purchaseList);
            shopBusket.DeleteProducts(purchaseList);

            //Assertion
            Assert.AreEqual("", shopBusket.GetPurchases());
            Assert.AreEqual(0, shopBusket.SeeAmountOfProducts()); // Добавить проверку кол-ва продуктов в корзине
        }

        [DataTestMethod]
        [DataRow(new[] { "яблоко", "яблоко" }, new[] { "яблоко" }, "яблоко: 1", 1)]
        [DataRow(new[] { "яблоко", "хлеб" }, new[] { "яблоко" }, "хлеб: 1", 1)]
        [DataRow(new[] { "яблоко", "яблоко", "хлеб" }, new[] { "яблоко" }, "яблоко: 1, хлеб: 1", 2)]
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
        [DataRow(15, new[] { "хлеб", "яблоко" }, 0)]
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
        [DataRow(new[] { "яблоко" }, 0)] // Добавить тест, где денег = 0
        [DataRow(new[] { "яблоко" }, 5)]
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
