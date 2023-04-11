using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

var driver = new EdgeDriver(new DriverManager().SetUpDriver(new EdgeConfig()));
string URL = "https://ukd.edu.ua/navchalni-plani";
driver.Navigate().GoToUrl(URL);

void PrintSpec((List<string> bachelor, List<string> master, List<string> doctorPh) spec)
{
    Console.WriteLine("Бакалаврат:");
    for (int index = 0; index < spec.bachelor.Count; index++)
    {
        Console.WriteLine($"{index}. {spec.bachelor[index]}");
    }
    Console.WriteLine("Магістратура:");
    for (int index = 0; index < spec.master.Count; index++)
    {
        Console.WriteLine($"{index}. {spec.master[index]}");
    }
    Console.WriteLine("Доктор філософії:");
    for (int index = 0; index < spec.doctorPh.Count; index++)
    {
        Console.WriteLine($"{index}. {spec.doctorPh[index]}");
    }
}

void GetEducationSpec(IWebDriver driver)
{
    var bachelor = new List<string>();
    var master = new List<string>();
    var doctorPh = new List<string>();

    for (int ind = 4; ind < 16; ind++)
    {
        bachelor.Add(driver.FindElement(By.XPath($"/html/body/div[1]/main/section/div/div/article/div/div[3]/div/div[2]/div/div/div/table/tbody/tr[{ind}]/td[1]")).Text);
    }
    for (int ind = 18; ind < 25; ind++)
    {
        master.Add(driver.FindElement(By.XPath($"/html/body/div[1]/main/section/div/div/article/div/div[3]/div/div[2]/div/div/div/table/tbody/tr[{ind}]/td[1]")).Text);
    }
    doctorPh.Add(driver.FindElement(By.XPath("/html/body/div[1]/main/section/div/div/article/div/div[3]/div/div[2]/div/div/div/table/tbody/tr[27]/td[1]")).Text);

    PrintSpec((bachelor, master, doctorPh));
    driver.Close();
}

Console.OutputEncoding = System.Text.Encoding.UTF8;
GetEducationSpec(driver);