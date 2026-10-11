using ePizzaHub.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Claims;

namespace ePizzaHub.UI.Helpers
{
    /// <summary>
    /// This base page view will be accessible in all the views, so that we can get the current logged in user's details from the token, 
    /// from the claim itself, in all the views, without writing the same code in each and every view, so that we can avoid code duplication
    /// and we can get the current logged in user's details in all the views.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class BasePageView<TModel> : RazorPage<TModel>
    {
        public UserModel CurrentUser
        {
            get
            {
                if (User.Claims.Any())
                {
                    string userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value.ToString();
                    string Email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value.ToString();
                    string UserId = User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value.ToString();

                    return new UserModel
                    {
                        Email = Email,
                        Name = userName,
                        UserId = Convert.ToInt32(UserId)
                    };
                }
                return null;
            }
        }
    }
}
