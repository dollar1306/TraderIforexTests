using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using System.IO;
using excel = Microsoft.Office.Interop.Excel;

/*
Thanks for the test, the work turned out not the most beautiful.
There is still something to work on to improve my abilities
My first automation in c #
I did not manage to do a read function from file and reports
*/

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
                //Browser opening function
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
                /*
                Some code that needs to read data from a file, but does not.
                I'm probably doing something wrong
                var user = ConfigurationManager.AppSettings["Username"];
                email.SendKeys(user);
                var pass = ConfigurationManager.AppSettings["Password"];
                password.SendKeys(pass);
                */
                String user = "test.QAEgypt111@test.com";
                IWebElement email = driver.FindElement(By.CssSelector("input[name='UserName']"));
                email.SendKeys(user);
                String pass = "wt6f4z1bnx";
                IWebElement password = driver.FindElement(By.CssSelector("input[name='Password']"));
                password.SendKeys(pass);
                IWebElement elementBtn = driver.FindElement(By.XPath("//*[@id='btnOkLogin']"));
                elementBtn.Submit();
                driver.Navigate().Back();

                try
                {
                    //Search for a question mark selector and grab text
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(9);
                    IWebElement element = driver.FindElement(By.XPath("//*[@id='lblUname']/i"));
                    String text = element.GetAttribute("title");
                    Console.WriteLine("This is a Text: " + text);
                    //Function call, file write
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
                /*
                Some code that needs to read data from a file, but does not.
                I'm probably doing something wrong
                driver.Url = ConfigurationManager.AppSettings["URL"];
                */
                driver.Url = "https://traderqa1.iforex.com/";
            driver.Manage().Window.Maximize();

                //A function that validates the site if loaded in less than 10 seconds
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver Web) =>
                {
                    //Waiting for element, visible button
                    Console.WriteLine("Waiting for visible button - 'Login'");
                    IWebElement element = Web.FindElement(By.XPath("//*[@id='btnOkLogin']"));
                    if (element.GetAttribute("value").Contains("Login"))
                    {
                        return true;
                    }
                    return false;
                });
                wait.Until(waitForElement);
        }


        //Function for writing text to a file
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
        //Close browser
        public void TearDown()
        {
           driver.Close();
           driver.Quit();
        }
    }
}