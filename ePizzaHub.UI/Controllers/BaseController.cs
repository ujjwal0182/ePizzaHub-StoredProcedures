using ePizzaHub.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ePizzaHub.UI.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// It does consist of a way to get the current logged in user's details from the token, from the claim itself.
        /// </summary> 

        /// The reason behind creating this base controller is to avoid the code duplication and I want to guess this user model across each 
        /// and every controller, so instead of writing the same code in each and every controller, I have created a base controller and I am inheriting this base controller in each and every controller, so that I can get the current logged in user's details from the token, from the claim itself.

        public UserModel CurrentUser
        {
            get
            {
                //These claims are coming from the LoginController when we are generating the ticket, we are passing the claims in the ticket, so we can get those claims here and extract the details of the user.
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
                                            