using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;

namespace UIMatrixML.Application.DOM
{
    public abstract class BaseDomElement : IDomElement
    {
        IWebElement Element { get; set; }

        public string TagName => Element.TagName;
        public string Text => Element.Text;
        public bool Enabled => Element.Enabled;
        public bool Selected => Element.Selected;
        public Point Location => Element.Location;
        public Size Size => Element.Size;
        public bool Displayed => Element.Displayed;

        public void Clear() => Element.Clear();
        public void Click() => Element.Click();
        public IWebElement FindElement(By by) => Element.FindElement(by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => Element.FindElements(by);
        public string GetAttribute(string attributeName) => Element.GetAttribute(attributeName);
        public string GetCssValue(string propertyName) => Element.GetCssValue(propertyName);
        public string GetProperty(string propertyName) => Element.GetProperty(propertyName);
        public void SendKeys(string text) => Element.SendKeys(text);
        public void Submit() => Element.Submit();


        /*==========================================================================*/


        public string Name { get; set; }
        public string ID { get; set; }
        public string[] Class { get; set; }
        public string Url { get; set; }
        public IWebDriver WebDriver { get; set; }

        public virtual string Selector
        {
            get
            {
                return $"{(string.IsNullOrWhiteSpace(this.ID) ? $"[name='{this.Name}']" : $"#{this.ID}")}";
            }
        }

        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="val">Value.</param>
        /// <returns>Value.</returns>
        public virtual void SetValue(string val = null)
        {
            {
                if(val == null)
                    ((IJavaScriptExecutor)this.WebDriver)
                        .ExecuteScript($"$(\"{this.Selector}\").val(null); $(\"{this.Selector}\").trigger('change');");
                else if(val is string)
                    ((IJavaScriptExecutor)this.WebDriver)
                        .ExecuteScript($"$(\"{this.Selector}\").val(\"{val}\"); $(\"{this.Selector}\").trigger('change');");
                else
                    ((IJavaScriptExecutor)this.WebDriver)
                        .ExecuteScript($"$(\"{this.Selector}\").val({(val).ToString().ToLower()}); $(\"{this.Selector}\").trigger('change');");
            }

            return;
        }

        public virtual string GetValue()
        {
            return Convert.ToString(((IJavaScriptExecutor)this.WebDriver)
                        .ExecuteScript($"return $(\"{this.Selector}\").val();"));
        }

        /// <summary>
        /// Get or set text.
        /// </summary>
        /// <param name="text">Value.</param>
        /// <returns>Text.</returns>
        public virtual string Txt(string text = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                text =
                    (string)((IJavaScriptExecutor)this.WebDriver).ExecuteScript($"return $('{this.Selector}').text();");
            }
            else
            {
                ((IJavaScriptExecutor)this.WebDriver)
                    .ExecuteScript($"$('{this.Selector}').text(\"{text}\"); $('{this.Selector}').trigger('change');");
            }

            return text;
        }

        /// <summary>
        /// Trigger element.
        /// </summary>
        /// <param name="trigger">Handler.</param>
        public virtual void Trigger(string trigger)
        {
            ((IJavaScriptExecutor)this.WebDriver)
                    .ExecuteScript($"$('{this.Selector}').trigger('{trigger}');");
        }

        /// <summary>
        /// Show html element.
        /// </summary>
        public virtual void Show()
        {
            ((IJavaScriptExecutor)this.WebDriver)
                    .ExecuteScript($"$('{this.Selector}').show();");
        }

        /// <summary>
        /// Hide html element.
        /// </summary>
        public virtual void Hide()
        {
            ((IJavaScriptExecutor)this.WebDriver)
                    .ExecuteScript($"$('{this.Selector}').hide();");
        }

        /// <summary>
        /// Remove html element.
        /// </summary>
        public virtual void Remove()
        {
            ((IJavaScriptExecutor)this.WebDriver)
                    .ExecuteScript($"$('{this.Selector}').remove();");
        }

        /// <summary>
        /// Verify element exists.
        /// </summary>
        /// <returns></returns>
        public virtual bool Verify()
        {
            return (bool)((IJavaScriptExecutor)this.WebDriver)
                    .ExecuteScript($"return $('{this.Selector}').length > 0;");
        }

        public BaseDomElement(IWebDriver driver)
        {
            this.WebDriver = driver;
        }
    }
}
