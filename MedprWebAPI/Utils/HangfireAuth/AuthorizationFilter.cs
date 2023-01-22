using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace MedprWebAPI.Utils.HangfireAuth;

public class AuthorizationFilter : IDashboardAuthorizationFilter
{
    /// <summary>
    /// This filter allows all users to check Dashboard
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}
