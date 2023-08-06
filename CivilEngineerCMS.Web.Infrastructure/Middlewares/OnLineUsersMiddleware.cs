namespace CivilEngineerCMS.Web.Infrastructure.Middlewares
{
    using System.Collections.Concurrent;

    using CivilEngineerCMS.Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Memory;

    using static CivilEngineerCMS.Common.GeneralApplicationConstants;

    public class OnLineUsersMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string cookieName;
        private readonly int lastActivityMinutes;

        private static readonly ConcurrentDictionary<string, bool> AllKeys =
            new ConcurrentDictionary<string, bool>();
        public OnLineUsersMiddleware(RequestDelegate next,
                                      string cookieName = OnLineUsersCookieName,
                                      int lastActivityMinutes = OnLineUsersLastActivityInMinutes)
        {
            this.next = next;
            this.cookieName = cookieName;
            this.lastActivityMinutes = lastActivityMinutes;
        }
        /// <summary>
        /// Adds the user to the cache if he is authenticated.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="memoryCache"></param>
        /// <returns></returns>
        public Task InvokeAsync(HttpContext context, IMemoryCache memoryCache)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                if (!context.Request.Cookies.TryGetValue(this.cookieName, out string userId))
                {
                    // First login after being offline
                    userId = context.User.GetId()!;

                    context.Response.Cookies.Append(this.cookieName, userId, new CookieOptions() { HttpOnly = true, MaxAge = TimeSpan.FromDays(30) });
                }

                memoryCache.GetOrCreate(userId, cacheEntry =>
                {
                    if (!AllKeys.TryAdd(userId, true))
                    {
                        // Adding key failed to the concurrent dictionary so we have an error
                        cacheEntry.AbsoluteExpiration = DateTimeOffset.MinValue;
                    }
                    else
                    {
                        cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(this.lastActivityMinutes);
                        cacheEntry.RegisterPostEvictionCallback(this.RemoveKeyWhenExpired);
                    }

                    return string.Empty;
                });
            }
            else
            {
                // User has just logged out
                if (context.Request.Cookies.TryGetValue(this.cookieName, out string userId))
                {
                    if (!AllKeys.TryRemove(userId, out _))
                    {
                        AllKeys.TryUpdate(userId, false, true);
                    }

                    context.Response.Cookies.Delete(this.cookieName);
                }
            }

            return this.next(context);
        }
        /// <summary>
        /// Checks if the user is online.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool CheckIfUserIsOnline(string userId)
        {
            bool valueTaken = AllKeys.TryGetValue(userId.ToLower(), out bool success);

            return success && valueTaken;
        }
        /// <summary>
        /// Removes the key from the concurrent dictionary when it expires, and if it fails to remove it, it updates it.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason"></param>
        /// <param name="state"></param>
        private void RemoveKeyWhenExpired(object key, object value, EvictionReason reason, object state)
        {
            string keyStr = (string)key; //UserId

            if (!AllKeys.TryRemove(keyStr, out _))
            {
                AllKeys.TryUpdate(keyStr, false, true);
            }
        }
        /// <summary>
        /// Returns the count of the online users.
        /// </summary>
        /// <returns></returns>
        public static int GetOnLineUsersCount()
        {
            return AllKeys.Count(x => x.Value);
        }
    }
}
