using BuditelWebServer.Server.Common;

namespace BuditelWebServer.Server.HTTP
{
    public class Header
    {
        // Тези константи са задължителни, за да работи Request.cs!
        public const string ContentType = "Content-Type";
        public const string ContentLength = "Content-Length";
        public const string Date = "Date";
        public const string Location = "Location";
        public const string Server = "Server";

        public Header(string name, string value)
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(value, nameof(value));

            this.Name = name;
            this.Value = value;
        }

        public string Name { get; init; }

        public string Value { get; init; }

        public override string ToString()
            => $"{this.Name}: {this.Value}";
    }
}   