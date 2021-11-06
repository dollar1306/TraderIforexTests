using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;

namespace TraderIforexTests
{
    public class Tests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            try
            {

                OpenBrowser();
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine("Failed to open browser: " + ex);
            }
        }


        [Test]
        public void LoginLogout()
        {
            try
            {
                IWebElement email = driver.FindElement(By.CssSelector("input[name='UserName']"));
                email.SendKeys("test.QAEgypt111@test.com");
                IWebElement password = driver.FindElement(By.CssSelector("input[name='Password']"));
                password.SendKeys("wt6f4z1bnx");
                IWebElement elementBtn = driver.FindElement(By.XPath("//*[@id='btnOkLogin']"));
                elementBtn.Submit();
                driver.Navigate().Back();

                try
                {
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(9);
                    IWebElement element = driver.FindElement(By.XPath("//*[@id='lblUname']/i"));
                    String text = element.GetAttribute("title");
                    Console.WriteLine("This is a Text: " + text);
                    WriteToTextFile(text);
                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine("Element was not found in current context page." +ex);
                    throw;
                }
            }catch(NoSuchElementException ex)
            {
                Console.WriteLine("Failed to find browser element" + ": " + ex);
            }
        }


        private void OpenBrowser()
        {
                driver = new ChromeDriver();
                driver.Url = "https://traderqa1.iforex.com/";
                driver.Manage().Window.Maximize();
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(9));
                Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver Web) =>
                {
                    Console.WriteLine("Waiting for visible button Login");
                    IWebElement element = Web.FindElement(By.XPath("//*[@id='btnOkLogin']"));
                    if (element.GetAttribute("value").Contains("Login"))
                    {
                        return true;
                    }
                    return false;
                });
                wait.Until(waitForElement);
        }



        private void WriteToTextFile(String text)
        {
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(@"C:\Users\USER\source\repos\TraderIforexTests\TextFile1.txt");
                //Write a line of text
                sw.WriteLine(text);
                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        [TearDown]
        public void TearDown()
        {
            //driver.Close();
           // driver.Quit();
        }
    }
}