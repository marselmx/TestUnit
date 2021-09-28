using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using page;
using System;

namespace TestProjectNUnit
{
    class UnitTest
    {
        IWebDriver driver = new ChromeDriver("C:\\Program Files (x86)\\Google\\Chrome\\Application");
        Banki page;

        [SetUp]
        public void Setup()
        {
            page = new Banki(driver, "Кредиты", 200);
        }


        [Test]
        public void Test1()
        {
            page.ChangeInput(0, "Приобретение недвижимости");
            page.ChangeInput(1, "500000");
            page.ChangeInput(2, "100000");
            page.ChangeInput(3, "10 лет");
            System.Threading.Thread.Sleep(1000);

            String resultText = page.getResult("Ежемесячный платеж").Replace(" ", "").Replace("₽", "");
            int result = int.Parse(resultText);

            Assert.That(result, Is.GreaterThan(10000));
        }

        [TearDown]
        public void close_Browser()
        {
            driver.Quit();
        }
    }
}
