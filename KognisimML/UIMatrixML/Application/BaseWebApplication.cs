using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIMatrixML.Application
{
    public abstract class BaseWebApplication : IWebApplication
    {
        public string Url { get; }
        public IWebDriver Driver { get; set; }

        /// <summary>
        /// Initialize web application.
        /// </summary>
        public virtual void Init(int width=1200, int height=800)
        {
            ChromeOptions chromeOptions =
                new ChromeOptions();

            chromeOptions.AddArgument($"--window-size={width},{height}");

            this.Driver = 
                new ChromeDriver("./", chromeOptions);

            this.Driver.Navigate().GoToUrl(this.Url);
        }

        /// <summary>
        /// Quit web application.
        /// </summary>
        public virtual void Quit()
        {
            this.Driver.Close();
        }

        public BaseWebApplication(string url)
        {
            this.Url = url;
        }

        ~BaseWebApplication()
        {
            try
            {
                this.Quit();
            }
            catch(Exception)
            {

            }
        }
    }
}
