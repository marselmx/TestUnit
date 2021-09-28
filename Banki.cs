using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System;

namespace page
{
    class Banki
    {
        IWebDriver driver;
        WebDriverWait wait;

        String url = "https://www.banki.ru";

        public Banki(IWebDriver myDriver, String page = "", int time = 30)
        {
            this.driver = myDriver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(time));

            // driver settings
            driver.Manage().Timeouts().ImplicitWait.Add(TimeSpan.FromSeconds(time * 3));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);

            // open page
            openPage(page);
            System.Threading.Thread.Sleep(time * 10);
        }

        public void openPage(String page = "")
        {
            if (page == "") return;
            By locate = By.CssSelector($".main-menu__sections-link[data-title = '{page}']");
            WaitAndGetElement(locate).Click();

            IList<String> windows = driver.WindowHandles;
            if (windows.Count == 2)
            {
                driver.SwitchTo().Window(windows[0]).Close();
                driver.SwitchTo().Window(windows[1]);
            }
        }

        public void ChangeInput(int index, String value)
        {
            By locate = By.CssSelector(".FormField___sc-12l8l1z-0");
            IList<IWebElement> inputs = WaitAndGetElements(locate);
            if (inputs.Count <= index) return;

            IWebElement input = inputs[index].FindElement(By.CssSelector("div[data-test]"));
            String type = input.GetAttribute("data-test");
            input.Click();

            if (type == "dropdown")
            {
                //Todo: cheange on XPath
                IList<IWebElement> elements = WaitAndGetElements(By.CssSelector(".DropdownList___sc-1c2bzie-3"));
                getElementOnText(elements, value).Click();
            }

            if (type == "input-range")
            {
                By locateInput = By.CssSelector("input");
                IWebElement inputElements = input.FindElement(locateInput);
                inputElements.SendKeys(Keys.Control + "a" + Keys.Delete);
                inputElements.SendKeys(value);
            }
        }

        public String getResult(String text)
        {
            By locate = By.XPath($"//div[contains(@class, 'Text___sc-1qazhk8-0') and text() = '{text}']/following::div[1]");
            return WaitAndGetElement(locate).Text;
        }

        private IWebElement getElementOnText(IList<IWebElement> elements, String text)
        {
            foreach (IWebElement element in elements)
                if (element.Text == text) return element;

            return null;
        }

        private IWebElement WaitAndGetElement(By locate)
        {
            wait.Until(drv => drv.FindElement(locate));
            return driver.FindElement(locate);
        }

        private IList<IWebElement> WaitAndGetElements(By locate)
        {
            wait.Until(drv => drv.FindElement(locate));
            return driver.FindElements(locate);
        }
    }
}
