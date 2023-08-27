using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using C3Apparel.Features.Base.API;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Features.Admin.Brand.API;
using C3Apparel.Frontend.Data.Settings;
using C3Apparel.Web.Authentication;
using C3Apparel.Web.Features.Brand.API.Requests;
using C3Apparel.Web.Features.Brand.API.Responses;
using C3Apparel.Web.Features.User.API.Requests;
using C3Apparel.Web.Features.User.API.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace C3Apparel.Features.Admin.User
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class UserController : Controller
    {
        private readonly IUserProvider _userProvider;
        public UserController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }
        
       
        public async Task<ActionResult> UserListing(int brandId = 0)
        {

            var vm = new UserListingPageViewModel();

            return View("~/Features/Admin/User/UserListingPage.cshtml",vm);
        }
        
        [Route("getusers")]
        [HttpPost]
        public async Task<ActionResult> GetUsers([FromBody]GetUserParameters requests)
        {
            int GetTotalPage(int totalItems, int itemsPerPage)
            {
                if (totalItems % itemsPerPage == 0)
                {
                    return totalItems / itemsPerPage;
                }

                return (int) Math.Floor((double) totalItems / itemsPerPage) + 1;
            }
            var response = new GetUsersResponse();
            var filter = new UserFilter()
            {
                UserName = requests.Filters.FilterUserName,
                Role = requests.Filters.FilterRole
            };
            IEnumerable<UserAccount> users = _userProvider.GetAllUsers(filter, requests.PageNumber, AdminSettings.DEFAULT_PAGE_SIZE);

            var totalCount = _userProvider.GetAllUsersCount(filter);

            response.TotalPage = GetTotalPage(totalCount, requests.ItemsPerPage);
            response.Users = users.Select(p => new UserAPIItem()
            {
               UserName = p.UserName,
               UserId = p.UserId,
               RoleName = p.RoleName,
               Email = p.Email,
               RoleId = p.RoleId
            }).ToList();
            
            return Ok(response);
        }

        [Route("delete-user")]
        [HttpPost]
        public async Task<ActionResult> DeleteUser([FromBody] StringIDParameter requests)
        {
            try
            {
                _userProvider.Delete(requests.Id);
                return Ok(new CommandAPIResult
                {
                    Success = true,

                });
            }
            catch (Exception ex)
            {
                return Ok(new CommandAPIResult
                {
                    Message = ex.Message
                });
            }
        }
        
        

    }
}