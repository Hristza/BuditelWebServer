using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; // Може да се наложи да използваш System.Net.WebUtility вместо това в по-нови .NET версии
using BuditelWebServer.Server.HTTP;

namespace BuditelWebServer.Server.HTTP
{
    public class Request
    {
        public Method Method { get; private set; }
        public string Url { get; private set; }
        public HeaderCollection Headers { get; private set; }
        public string Body { get; private set; }

        // 1. Добави свойството Form
        public IReadOnlyDictionary<string, string> Form { get; private set; }

        public static Request Parse(string request)
        {
            var lines = request.Split("\r\n");

            var startLine = lines.First().Split(" ");
            var method = ParseMethod(startLine[0]);
            var url = startLine[1];

            var headers = ParseHeaders(lines.Skip(1));

            var bodyLines = lines.Skip(headers.Count + 2).ToArray();
            var body = string.Join("\r\n", bodyLines);

            // 2. Тук извикваме новия метод за парсване на формата
            var form = ParseForm(headers, body);

            return new Request
            {
                Method = method,
                Url = url,
                Headers = headers,
                Body = body,
                Form = form // 3. Записваме резултата
            };
        }

        // ... (запази си методите ParseMethod и ParseHeaders както са си били) ...
        private static Method ParseMethod(string method)
        {
            // Твоята съществуваща логика тук...
            try
            {
                return (Method)Enum.Parse(typeof(Method), method, true);
            }
            catch
            {
                throw new InvalidOperationException($"Method '{method}' is not supported");
            }
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> headerLines)
        {
            // Твоята съществуваща логика тук...
            var headerCollection = new HeaderCollection();
            foreach (var headerLine in headerLines)
            {
                if (headerLine == string.Empty) break;
                var headerParts = headerLine.Split(":", 2);
                if (headerParts.Length != 2) throw new InvalidOperationException("Request is not valid.");

                headerCollection.Add(headerParts[0], headerParts[1].Trim());
            }
            return headerCollection;
        }

        // 4. Добави метод ParseForm
        private static Dictionary<string, string> ParseForm(HeaderCollection headers, string body)
        {
            var formCollection = new Dictionary<string, string>();

            // Проверяваме дали хедърите съдържат Content-Type и дали той е за форма
            if (headers.Contains(Header.ContentType)
                && headers[Header.ContentType] == ContentType.FormUrlEncoded)
            {
                var parsedResult = ParseFormData(body);

                foreach (var (name, value) in parsedResult)
                {
                    formCollection.Add(name, value);
                }
            }

            return formCollection;
        }

        // 5. Добави метод ParseFormData (същинското декодиране)
        private static Dictionary<string, string> ParseFormData(string bodyLines)
            => System.Net.WebUtility.UrlDecode(bodyLines) // Използвам System.Net.WebUtility, защото е стандартно
                .Split('&')
                .Select(part => part.Split('='))
                .Where(part => part.Length == 2)
                .ToDictionary(
                    part => part[0],
                    part => part[1],
                    StringComparer.InvariantCultureIgnoreCase);
    }
}