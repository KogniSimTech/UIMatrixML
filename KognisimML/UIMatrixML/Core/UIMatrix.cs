using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIMatrixML.Application;

namespace UIMatrixML.Core
{
    public static class UIMatrix
    {
        /// <summary>
        /// Iniitialize UIMatrix on front-end.
        /// </summary>
        /// <param name="webApplication"></param>
        public static void Init(IWebApplication webApplication)
        {
            IJavaScriptExecutor jsExecute = ((IJavaScriptExecutor)webApplication.Driver);

            jsExecute.ExecuteScript("return matrixModel.init();");
        }

        /// <summary>
        /// Add matrix element.
        /// </summary>
        /// <param name="webApplication">Web application handler.</param>
        /// <param name="elementSelector">Element selector value.</param>
        public static void AddElement(IWebApplication webApplication, string elementSelector)
        {
            IJavaScriptExecutor jsExecute = ((IJavaScriptExecutor)webApplication.Driver);

            jsExecute.ExecuteScript($"matrixModel.matrices.addElement(\"{elementSelector}\");");
        }

        /// <summary>
        /// Add multiple matrix elements.
        /// </summary>
        /// <param name="webApplication">Web application handler.</param>
        /// <param name="elementSelectors">Element selectors.</param>
        public static void AddElements(IWebApplication webApplication, params string[] elementSelectors)
        {
            foreach(string selector in elementSelectors)
            {
                AddElement(webApplication, selector);
            }
        }

        /// <summary>
        /// Calculate front-end matrices.
        /// </summary>
        /// <param name="webApplication">Web application handler.</param>
        /// <returns>Current matrix.</returns>
        public static IList<double> CalculateMatrices(IWebApplication webApplication)
        {
            IJavaScriptExecutor jsExecute = ((IJavaScriptExecutor)webApplication.Driver);

            return ((IEnumerable<object>)jsExecute.ExecuteScript("return matrixModel.matrices.calculateMatrices();")).Select(x=>Convert.ToDouble(x)).ToList();
        }

        /// <summary>
        /// Wait for front-end to confirm validity.
        /// </summary>
        /// <param name="webApplication">Web Application handler.</param>
        /// <param name="callback">Confirmation callback.</param>
        /// <returns>Current matrix.</returns>
        public static IList<double> AwaitValidityConfirmation(IWebApplication webApplication, Action<bool, IList<double>> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            IJavaScriptExecutor jsExecute = ((IJavaScriptExecutor)webApplication.Driver);

            bool? validity = null;

            Task validationTask = new Task(() =>
            {
                while (validity == null)
                {
                    if (jsExecute.ExecuteScript("return matrixModel.validity;") is bool isValid)
                        validity = isValid;

                    System.Threading.Thread.Sleep(500);
                }
            });

            validationTask.Start();
            validationTask.Wait();

            IList<double> matrix =
                CalculateMatrices(webApplication);

            jsExecute.ExecuteScript("matrixModel.validity = null;");

            callback(validity.Value, matrix);

            return matrix;

        }
    }
}
