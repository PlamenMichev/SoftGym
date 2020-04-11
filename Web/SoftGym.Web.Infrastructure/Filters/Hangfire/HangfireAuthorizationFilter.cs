namespace SoftGym.Web.Infrastructure.Filters.Hangfire
{
    using global::Hangfire.Dashboard;
    using SoftGym.Common;

    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
        }
    }
}
