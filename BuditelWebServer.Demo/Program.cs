using BuditelWebServer.Server;
using BuditelWebServer.Server.HTTP;
using static BuditelWebServer.Server.HTTP.RedirectedResponse;

namespace BuditelWebServer.Demo
{
    internal class Program
    {
        private const string HtmlForm = @"<form action='/HTML' method='POST'>
            Name: <input type='text' name='Name'/>
            Age: <input type='number' name='Age'/>
            <input type='submit' value='Save' />
        </form>";

        static void Main(string[] args)
        {
            var server = new HttpServer(8080, routes => routes
                .MapGet("/", new TextResponse("Hello from the server!"))
                .MapGet("/Redirect", new RedirectResponse("https://softuni.org/"))
                .MapGet("/HTML", new HtmlResponse(HtmlForm))
              .MapPost("/HTML", new TextResponse("", AddFormDataAction))
            );

            server.Start();
        }

        private static void AddFormDataAction(Request request, Response response)
        {
            response.Body = "";
            foreach (var (key, value) in request.Form)
            {
                response.Body += $"{key} - {value}";
                response.Body += Environment.NewLine;
            }
        }
    }
}