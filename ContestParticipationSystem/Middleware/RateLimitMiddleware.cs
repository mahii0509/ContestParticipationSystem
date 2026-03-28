using System.Collections.Concurrent;

namespace ContestParticipationSystem.Middleware
{
    public class RateLimitMiddleware
    {
        private static ConcurrentDictionary<string, List<DateTime>> requests =
            new ConcurrentDictionary<string, List<DateTime>>();

        private readonly RequestDelegate _next;

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();

            var requestTimes = requests.GetOrAdd(ip, new List<DateTime>());

            lock (requestTimes)
            {
                requestTimes.RemoveAll(t => t < DateTime.UtcNow.AddMinutes(-1));

                if (requestTimes.Count > 60)
                {
                    context.Response.StatusCode = 429;
                    context.Response.WriteAsync("Too many requests");
                    return;
                }

                requestTimes.Add(DateTime.UtcNow);
            }

            await _next(context);
        }
    }
}