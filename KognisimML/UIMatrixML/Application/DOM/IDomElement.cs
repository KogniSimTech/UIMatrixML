using OpenQA.Selenium;

namespace UIMatrixML.Application.DOM
{
    public interface IDomElement : IWebElement
    {
        string Name { get; set; }
        string ID { get; set; }
        string[] Class { get; set; }

        string Selector { get; }

        IWebDriver WebDriver { get; set; }
    }
}
