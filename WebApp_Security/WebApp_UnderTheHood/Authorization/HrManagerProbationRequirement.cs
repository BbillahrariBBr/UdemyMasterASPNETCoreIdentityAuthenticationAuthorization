using Microsoft.AspNetCore.Authorization;

namespace WebApp_UnderTheHood.Authorization
{
    public class HrManagerProbationRequirement:IAuthorizationRequirement
    {
        public HrManagerProbationRequirement(int probationMoths)
        {
            ProbationMoths = probationMoths;
        }

        public int ProbationMoths { get; }
    }

    public class HrManagerProbationRequirementHandler : AuthorizationHandler<HrManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HrManagerProbationRequirement requirement)
        {
          if(!context.User.HasClaim(x => x.Type == "EmployementDate"))
                return Task.CompletedTask;

          if(DateTime.TryParse(context.User.FindFirst(x => x.Type == "EmployementDate")?.Value, out DateTime employementDate))
            {
                var period = DateTime.Now - employementDate;
                if (period.Days > 30 * requirement.ProbationMoths)
                    context.Succeed(requirement);
            }
          return Task.CompletedTask;
        }
    }
}
