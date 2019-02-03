using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIMatrixML.Application
{
    public interface IWebApplication
    {
        string Url { get; }
        IWebDriver Driver { get; set; }

        /// <summary>
        /// Initialize web application.
        /// </summary>
        void Init(int width = 1200, int height = 800);

        /// <summary>
        /// Quit web application.
        /// </summary>
        void Quit();
    }
}
