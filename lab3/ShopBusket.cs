using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class ShopBusket
    {
        public Dictionary<string, int> purchaseList;
        public double moneyCount;

        public ShopBusket(double amountOfMoney = 0)
        {
            InitShopBusket(amountOfMoney);
        }

        public void InitShopBusket(double amountOfMoney)
        {
            if (amountOfMoney >= 0)
                moneyCount = amountOfMoney;
            purchaseList = new Dictionary<string, int>();
        }

        public double GetMoney()
        {
            return moneyCount;
        }

        public string GetPurchases()
        {
            string str = "";
            foreach (var elem in purchaseList.Keys)
            {
                str += elem + ": " + purchaseList[elem] + ", ";
            }
            str = str.TrimEnd(' ');
            str = str.TrimEnd(',');
            return str;
        }

        public void PutPurchasesInBusket(string[] purchasesList)
        {
            string[] allProductsList = { "яблоко", "томат", "хлеб", "яйца", "банан" };
            foreach (var elem in purchasesList)
            {
                if (allProductsList.Contains(elem))
                    if (purchaseList.ContainsKey(elem)) 
                        purchaseList[elem] = purchaseList[elem] + 1;
                    else
                        purchaseList[elem] = 1;
                else
                    throw new Exception("Нет такого товара"); // Поменять на ошибку
            }
        }
        public double SeeTotalPrice() // С большой буквы
        {
            Dictionary<string, int> allProductsPrices = new Dictionary<string, int>()
            {
                {"яблоко", 10},
                {"томат", 20},
                {"хлеб", 5},
                {"яйца", 12},
                {"банан", 25},
            };
            double price = 0;

            foreach (var elem in purchaseList.Keys)
            {
                price += allProductsPrices[elem] * purchaseList[elem];
            }

            return price;
        }

        public int SeeAmountOfProducts()
        {
            int amount = 0;
            foreach (var elem in purchaseList.Keys)
            {
                amount += purchaseList[elem];
            }

            return amount;
        }

        public void BuyProducts()
        {
            double price = SeeTotalPrice();
            if (price > moneyCount)
                throw new Exception("Недостаточно денег");
            else
            {
                purchaseList.Clear();
                moneyCount -= price;
            }
        }

        public void DeleteProducts(string[] purchasesList)
        {
            foreach (var elem in purchasesList)
            {
                purchaseList[elem] = purchaseList[elem] - 1;
                if (purchaseList[elem] == 0) // Поправить условие
                    purchaseList.Remove(elem);
            }
        }
    }
}
