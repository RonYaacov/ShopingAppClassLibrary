using System;
using OpenQA.Selenium;
using System.Threading;

namespace ShopingBotClassLibrary
{
    public class AmazonProdact : Prodact
    {
        private static int ProdactCount { get; set;}
               
        public override void CheckIfAvailble(string AmazonCartUrl, string UserName, string Password)
        {
            IWebDriver Driver = Prodact.Drivers[Prodact.Drivers.Count - 1];
            Driver.Navigate().GoToUrl(AmazonCartUrl);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            Log("Program Start Amazon", LogStatus.Normal);

            while (true)
            {
                if (Driver.Url != AmazonCartUrl)
                {
                    Driver.Navigate().GoToUrl(AmazonCartUrl);
                }               
                // This Code Block Is For Buying From The Shoping List

                IJavaScriptExecutor Executor = Driver as IJavaScriptExecutor;
                for (int i = 0; i < 10; i++)
                {
                    Executor.ExecuteScript("window.scrollBy(0,2000)");
                    try
                    {
                        Driver.FindElement(By.CssSelector("div[id=endOfListMarker]"));
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
                var AllButtons = Driver.FindElements(By.CssSelector("a[class=\"a-button-text a-text-center\"]"));
                ProdactsOnTheShopingList = AllButtons.Count;
                if(ProdactsOnTheShopingList < 0)
                {
                    Driver.Navigate().GoToUrl(AmazonCartUrl);
                    continue;
                }
                for (int i = 0; i < AllButtons.Count; i++)
                {
                    if (AllButtons[i].Text.Trim() == "Add to Cart" || AllButtons[i].Text.Trim() == "הוסף לסל")
                    {
                        try
                        {
                            AllButtons[i].Click();
                            Log("Clicked On Add to Cart", LogStatus.Seccess);
                            Thread.Sleep(1000);
                            var ProceedToCheckOut = Driver.FindElements(By.CssSelector("a[role=button]"));
                            for (int x = 0; x < ProceedToCheckOut.Count; x++)
                            {
                                if (ProceedToCheckOut[x].Text.Trim() == "Proceed to checkout" || ProceedToCheckOut[x].Text.Trim() == "עבור לדף התשלום")
                                {
                                    ProceedToCheckOut[x].Click();
                                    Log("Clicked On Proceed to Check Out", LogStatus.Seccess);
                                    return;
                                }
                            }
                            
                        }
                        catch { continue; }                      
                    }
                }                
                Runs++;               
                ProgramStatus();
                Driver.Navigate().Refresh();
                Thread.Sleep(2000);
                continue;
               
            }
        }

        public override void UserSignIn(string Url, string UserName, string Password)
        {
            IWebDriver Driver = Prodact.Drivers[Prodact.Drivers.Count - 1];
            try
            {
                try
                {
                    var checkIfAllreadyRun = Driver.FindElement(By.CssSelector("a[id=ap_switch_account_link]")); 
                    var EnterPassword = Driver.FindElement(By.CssSelector("input[name=password]"));
                    EnterPassword.SendKeys(Password);
                    var SignIn = Driver.FindElement(By.CssSelector("input[id=signInSubmit]"));
                    SignIn.Click();
                }
                catch
                {
                    Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    Driver.Navigate().GoToUrl("https://www.amazon.com/ap/signin?_encoding=UTF8&openid.assoc_handle=usflex&openid.claimed_id=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0%2Fidentifier_select&openid.identity=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0%2Fidentifier_select&openid.mode=checkid_setup&openid.ns=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0&openid.ns.pape=http%3A%2F%2Fspecs.openid.net%2Fextensions%2Fpape%2F1.0&openid.pape.max_auth_age=0&openid.return_to=https%3A%2F%2Fwww.amazon.com%2Fgp%2Fyourstore%2Fhome%3Fie%3DUTF8%26action%3Dsign-out%26path%3D%252Fgp%252Fyourstore%252Fhome%26ref_%3Dnav_AccountFlyout_signout%26signIn%3D1%26useRedirectOnSuccess%3D1");
                    var UserNameInputBox = Driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div[2]/div/div[1]/form/div/div/div/div[1]/input[1]"));
                    UserNameInputBox.SendKeys(UserName);
                    var CountinueToPassword = Driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div[2]/div/div[1]/form/div/div/div/div[2]/span/span/input"));
                    CountinueToPassword.Click();
                    var PasswordInputBox = Driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div[2]/div/div/div/form/div/div[1]/input"));
                    PasswordInputBox.SendKeys(Password);
                    var LogInButton = Driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div[2]/div/div/div/form/div/div[2]/span/span/input"));
                    LogInButton.Click();
                }
                
            }
            catch
            {
                Log("Can't Login You May Enterd Incorrect User Name / Password...", LogStatus.Eror);
                Thread.Sleep(5000);
                Log("The Program Is Terminated", LogStatus.Eror);
                Environment.Exit(0);
            }

        }

        public override void PurchaseAvailableProdact(string AmazonCratURL, string USerName, string Password = "safi1990")
        {
            IWebDriver Driver = Prodact.Drivers[Prodact.Drivers.Count - 1];
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            // the code below is needed only If the CheckIfAvelible Method is runing on the
            // save for later and not on the shoping list 

            // code **** IWebElement ProceedToCheckOut = Driver.FindElement(By.CssSelector("input[name=proceedToRetailCheckout")); 
            // ProceedToCheckOut.Click() ****code;
            try
            {
                IWebElement FirstPlaceOrder = Driver.FindElement(By.CssSelector("input[name=placeYourOrder1]"));
                FirstPlaceOrder.Click();
                Log("Purchase", LogStatus.Seccess);
                PurchasedProdacts = ProdactCounter();
                return;
            }
            catch
            {
                try
                {
                    IWebElement ReEnterPassword = Driver.FindElement(By.CssSelector("input[id=ap_password]"));
                    ReEnterPassword.SendKeys(Password);
                    ReEnterPassword.SendKeys(Keys.Enter);
                    PlaceOrder(Driver);
                    return;
                }
                catch
                {
                    try
                    {
                        var AllAdresses = Driver.FindElements(By.CssSelector("a[data-action=page-spinner-show]"));

                        if (AllAdresses.Count > 0)
                        {
                            IWebElement CurrectHomeAdress = AllAdresses[0];
                            CurrectHomeAdress.Click();
                            Log("I Clicked On The First Adress", LogStatus.Seccess);
                        }
                        try
                        {                          
                            var ContinueFirst = Driver.FindElement(By.CssSelector("input[type=submit]"));
                            ContinueFirst.Click();
                           
                            Log("I Clicked On The First Continue", LogStatus.Seccess);
                            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
                            try
                            {
                                PlaceOrder(Driver);
                            }
                            catch
                            {
                                try
                                {
                                    var ContinueSecond = Driver.FindElements(By.CssSelector("span[class=\"a-button a-button-span12 a-button-primary pmts-button-input\"]"));
                                    Thread.Sleep(500);
                                    ContinueSecond[0].Click();
                                }
                                catch
                                {
                                    PlaceOrder(Driver);
                                }
                            }
                            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                            try
                            {
                                var NoThankes = Driver.FindElement(By.CssSelector("a[rel=nt1]"));
                                NoThankes.Click();
                            }
                            catch
                            {
                                PlaceOrder(Driver);
                            }
                            finally
                            {
                                PlaceOrder(Driver);                               
                            }
                        }
                        catch { }

                        try
                        {
                            IWebElement ReEnterPassword = Driver.FindElement(By.CssSelector("input[id=ap_password]"));
                            ReEnterPassword.SendKeys(Password);
                            ReEnterPassword.SendKeys(Keys.Enter);
                        }
                        catch { }
                    }
                    catch { }
                }
            }
            try
            {
                var ForcePurchase = Driver.FindElement(By.CssSelector("input[name=forcePlaceOrder]"));
                ForcePurchase.Click();
                PlaceOrder(Driver);
                return;
            }
            catch { }
            finally
            {
                Prodact.Drivers[0].Close();
                Prodact.Drivers.Clear();
            }
        }
        protected void SaveForLater(string AmazonCartUrl, IWebDriver Driver)
        {
            Driver.Navigate().GoToUrl(AmazonCartUrl);
            var ProdactsInShopingCart = Driver.FindElements(By.CssSelector("input[data-action=save-for-later]"));
            if (ProdactsInShopingCart.Count > 0)
            {
                for (int i = 0; i < ProdactsInShopingCart.Count; i++)
                {
                    try
                    {
                        ProdactsInShopingCart[i].Click();
                        Driver.Navigate().Refresh();
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

        }

         private void PlaceOrder(IWebDriver Driver)
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            IWebElement PlaceOrder = Driver.FindElement(By.CssSelector("input[class=\"a-button-text place-your-order-button\"]"));
            PlaceOrder.Click();
            Log("Purchase", LogStatus.Seccess);
            PurchasedProdacts = ProdactCounter();
            return;
        }


        private int ProdactCounter()
        {
            ProdactCount++;
            int PurchasedProdactNumber = ProdactCount;
            return PurchasedProdactNumber;
        }
    }
}


