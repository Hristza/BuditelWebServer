namespace BuditelWebServer.Server.HTTP
{
    public class ContentType
    {
        public const string PlainText = "text/plain";
        public const string Html = "text/html";
        public const string FormUrlEncoded = "application/x-www-form-urlencoded";
    }


namespace BuditelWebServer.Server.HTTP
    {
        public class TextResponse : ContentResponse
        {
            public TextResponse(string text, Action<Request, Response> preRenderAction = null)
                : base(text, ContentType.PlainText, preRenderAction)
            {
            }
        }
    }
}
