namespace V.TouristGuide.Server.Middlewares
{
    public class SupportOptionsMiddleware
    {
        private readonly RequestDelegate next;

        public SupportOptionsMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method.ToUpper() == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                return;
            }

            await this.next(context);
        }
    }
}
