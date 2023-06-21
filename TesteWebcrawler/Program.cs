using HtmlAgilityPack;
using System.Net;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TesteWebcrawler.Models;
using TesteWebcrawler;
using Newtonsoft.Json;

Console.WriteLine("Obtendo Html.");
IWebDriver driver = new ChromeDriver();

driver.Navigate().GoToUrl("https://proxyservers.pro/proxy/list/order/updated/order_dir/desc");

IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
string htmlContent = (string)jsExecutor.ExecuteScript("return document.documentElement.outerHTML;");

Console.WriteLine("Obtenção do conteúdo HTML concluída.");

var htmlDoc = new HtmlDocument();
htmlDoc.LoadHtml(htmlContent);

var extrations = new List<ExtrationModel>();
int executionId = 0;

try
{
    
        int paginas = int.Parse(htmlDoc.DocumentNode.SelectSingleNode("//ul[@class='pagination justify-content-end']/li[last()]").InnerText);

    Console.WriteLine("Inserindo execução no banco de dados");

    var response = await ApiRequests.CreateExecution(new CreateExecutionModel { PageNumbers = paginas });
    executionId = response.Data.ExecutionId;

    Console.WriteLine("Insersão completa");

    Console.WriteLine("Lendo paginas");
    for (int pagina = 1; pagina <= paginas; pagina++)
    {
        driver.Navigate().GoToUrl("https://proxyservers.pro/proxy/list/order/updated/order_dir/desc/page/" + pagina);

        Thread.Sleep(800);

        string htmlContentPage = (string)jsExecutor.ExecuteScript("return document.documentElement.outerHTML;");

        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(documentsPath, $"exec{executionId}-page{pagina}.html");
        File.WriteAllText(filePath, htmlContentPage);

        var htmlDocPage = new HtmlDocument();
        htmlDocPage.LoadHtml(htmlContentPage);

        var container = htmlDocPage.DocumentNode.SelectSingleNode("//table[@class='table table-hover']");
        var nodes = container.SelectNodes("//tbody//tr");

        if (nodes.Count > 0)
        {
            foreach (var node in nodes)
            {
                var celulas = node.SelectNodes("./child::td");

                var ipAdress = celulas[1].InnerText;
                var port = celulas[2].InnerText;
                var country = celulas[3].InnerText;
                var protocol = celulas[6].InnerText;

                var extration = new ExtrationModel
                {
                    Country = country.Trim(),
                    Port = int.Parse(port.Trim()),
                    Protocol = protocol.Trim(),
                    IpAdress = ipAdress.Trim()
                };

                extrations.Add(extration);
            }
        }
    }

    Console.WriteLine("Leitura de paginas concluida");
}
catch (Exception e)
{
    Console.WriteLine("Erro ao processar informaçoes - " + e.Message);
    return;
}
finally
{
    try
    {
        Console.WriteLine("Salvando arquivo");

        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(documentsPath, $"dados-exec{executionId}.json");

        Console.WriteLine("Arquivo criado em " + documentsPath);

        var jsonFile = JsonConvert.SerializeObject(extrations, Formatting.Indented);

        File.WriteAllText(filePath, jsonFile);

        Console.WriteLine("Completando dados no banco de dados");
        await ApiRequests.UpdateExecution(
            new UpdateExecutionModel
            {
                ExecutionId = executionId,
                JsonFile = jsonFile,
                LineNumbers = extrations.Count
            }
        );
        Console.WriteLine("Dados registrados - Execução finalizada com sucesso");
    }
    catch (Exception e)
    {
        Console.WriteLine("Erro ao atualizar execuçao - " + e.Message);
    }
}

driver.Quit();

Console.ReadLine();