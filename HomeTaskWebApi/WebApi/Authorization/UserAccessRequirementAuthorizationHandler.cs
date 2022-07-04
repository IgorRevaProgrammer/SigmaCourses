using Microsoft.AspNetCore.Authorization;
using Models.Models;
using System.Threading.Tasks;

namespace WebApi.Authorization
{
    public class UserAccessRequirementAuthorizationHandler : AuthorizationHandler<UserAccessRequirement, Student>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            UserAccessRequirement requirement,
            Student student)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            if (context.User.Identity?.Name == student.Email)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
