using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Lab5
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string BaseUrl = "https://the-internet.herokuapp.com/";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        [Test]
        public void Test1_AddRemoveElements()
        {
            driver.Navigate().GoToUrl(BaseUrl + "add_remove_elements/");
            driver.FindElement(By.XPath("//button[text()='Add Element']")).Click();
            Assert.That(driver.FindElement(By.ClassName("added-manually")).Displayed, Is.True);
            driver.FindElement(By.ClassName("added-manually")).Click();
            Assert.That(driver.FindElements(By.ClassName("added-manually")).Count, Is.EqualTo(0));
        }

        [Test]
        public void Test2_Checkboxes()
        {
            driver.Navigate().GoToUrl(BaseUrl + "checkboxes");
            var checkboxes = driver.FindElements(By.CssSelector("#checkboxes input"));
            if (!checkboxes[0].Selected) checkboxes[0].Click();
            Assert.That(checkboxes[0].Selected, Is.True);
        }

        [Test]
        public void Test3_Dropdown()
        {
            driver.Navigate().GoToUrl(BaseUrl + "dropdown");
            var select = new SelectElement(driver.FindElement(By.Id("dropdown")));
            select.SelectByText("Option 1");
            Assert.That(select.SelectedOption.Text, Is.EqualTo("Option 1"));
        }

        [Test]
        public void Test4_Inputs()
        {
            driver.Navigate().GoToUrl(BaseUrl + "inputs");
            var input = driver.FindElement(By.TagName("input"));
            input.SendKeys("12345");
            Assert.That(input.GetAttribute("value"), Is.EqualTo("12345"));
        }

        [Test]
        public void Test5_StatusCodes()
        {
            driver.Navigate().GoToUrl(BaseUrl + "status_codes");
            driver.FindElement(By.LinkText("200")).Click();
            Assert.That(driver.FindElement(By.TagName("body")).Text, Does.Contain("200 status code"));
        }

        [Test]
        public void Test6_DynamicControls()
        {
            driver.Navigate().GoToUrl(BaseUrl + "dynamic_controls");
            driver.FindElement(By.CssSelector("#checkbox-example button")).Click();
            var msg = wait.Until(d => d.FindElement(By.Id("message")));
            Assert.That(msg.Text, Is.EqualTo("It's gone!")); 
        }

        [Test]
        public void Test7_FileUpload()
        {
            driver.Navigate().GoToUrl(BaseUrl + "upload");
            string filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "testfile.txt");
            if (!File.Exists(filePath)) File.WriteAllText(filePath, "testfile.txt");
            driver.FindElement(By.Id("file-upload")).SendKeys(filePath);
            driver.FindElement(By.Id("file-submit")).Click();
            Assert.That(driver.FindElement(By.TagName("h3")).Text, Is.EqualTo("File Uploaded!"));
        }

        [Test]
        public void Test8_JavaScriptAlerts()
        {
            driver.Navigate().GoToUrl(BaseUrl + "javascript_alerts");
            driver.FindElement(By.XPath("//button[text()='Click for JS Alert']")).Click();
            driver.SwitchTo().Alert().Accept();
            Assert.That(driver.FindElement(By.Id("result")).Text, Is.EqualTo("You successfully clicked an alert"));
        }

        [Test]
        public void Test9_MultipleWindows()
        {
            driver.Navigate().GoToUrl(BaseUrl + "windows");
            string original = driver.CurrentWindowHandle;
            driver.FindElement(By.LinkText("Click Here")).Click();
            wait.Until(d => d.WindowHandles.Count == 2);
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            Assert.That(driver.FindElement(By.TagName("h3")).Text, Is.EqualTo("New Window"));
            driver.Close();
            driver.SwitchTo().Window(original);
        }

        [Test]
        public void Test10_NotificationMessages()
        {
            driver.Navigate().GoToUrl(BaseUrl + "notification_message_rendered");
            driver.FindElement(By.LinkText("Click here")).Click();
            var msg = wait.Until(d => d.FindElement(By.Id("flash")));
            Assert.That(msg.Text, Does.Contain("Action"));
        }
    }
}