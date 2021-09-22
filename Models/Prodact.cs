using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Globalization;

namespace ShopingBotClassLibrary
{
     public class Prodact
    {
        protected static bool IsAvailable { get; set; } = false;
        protected static ulong Runs { get; set; }
        public static int PurchaseFailes { get; set; } = 0;
        public static int ProdactsOnTheShopingList { get; set; }
        public static int PurchasedProdacts { get; set; } = 0;
        public IWebDriver Driver { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }

        public enum LogStatus { Normal, Eror, Seccess }



        public static List<IWebDriver> Drivers = new List<IWebDriver>();

        public virtual void UserSignIn(string Url, string Password, string UserName) { }
        
        public virtual void PurchaseAvailableProdact(string URL, string UserName , string Password = "") { }
        
        public virtual void CheckIfAvailble(string ProdactURL, string UserName, string Password) { }

        protected void Log(string Message, LogStatus Status = LogStatus.Normal, bool OneLine = false)
        { 
            
            switch (Status)
            {
                case LogStatus.Normal:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogStatus.Seccess:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogStatus.Eror:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            TextInfo CultInfo = new CultureInfo("en-US", false).TextInfo;
            Message = CultInfo.ToTitleCase(Message.ToLower());
            
            if (OneLine)
            {

                Console.Write(" | "+Message);
            }
            else
            {
                Console.WriteLine(Message);
            }
        }
        public string Report()
        {
            try
            {
                string Report = $"The Program Run { Runs} Times | There Are {ProdactsOnTheShopingList} " +
                $"Prodacts On The List | Faild To Buy {PurchaseFailes} Times |" +
                $" Secceed To Buy {AmazonProdact.PurchasedProdacts} Times";
                return Report;
            }
            catch
            {
                Log("Data Doesn't Exist", LogStatus.Eror);
                return "Data Doesn't Exist";
            }            
        }
        protected void ProgramStatus()
        {
            Log($"The Program Run {Runs} Times", LogStatus.Normal, true);
            Log($"There Are {ProdactsOnTheShopingList} Prodacts On The List", LogStatus.Normal, true);
            Log($"Faild To Buy {PurchaseFailes} Times", LogStatus.Eror, true);
            Log($"Secceed To Buy {PurchasedProdacts} Times", LogStatus.Seccess);
        }


        

     }
}

