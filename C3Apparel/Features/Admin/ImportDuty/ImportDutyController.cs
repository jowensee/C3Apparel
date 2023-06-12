using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BlankSiteCore.Features.Base.API;
using C3Apparel.Data.Common;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Features.Admin.ImportDuty.API;
using C3Apparel.Features.Admin.ImportDuty.API.Requests;
using C3Apparel.Frontend.Data.Settings;
using C3Apparel.Web.Features.AdminImportDuty.API.Responses;
using C3Apparel.Web.Features.Pricing.API.Requests;
using C3Apparel.Web.Features.Pricing.API.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Features.Admin.ImportDuty
{
    //[TypeFilter(typeof(AdminAuthorizationFilter))]
    public class ImportDutyController : Controller
    {
        private readonly IImportDutyInfoProvider _importDutyInfoProvider;
        public ImportDutyController(IImportDutyInfoProvider importDutyInfoProvider)
        {
            _importDutyInfoProvider = importDutyInfoProvider;
        }
        
       
        public async Task<ActionResult> Index()
        {

            var vm = new ImportDutyEditViewModel();

            return View("~/Features/Admin/ImportDuty/ImportDutyEditPage.cshtml",vm);
        }
        
        
        [Route("getimportduty")]
        [HttpPost]
        public async Task<ActionResult> GetImportDuty()
        {
            var response = new GetEditImportDutyResponse();

            var importDutyInfo =  _importDutyInfoProvider.Get();

            if (importDutyInfo != null)
            {
                return Ok(new GetEditImportDutyResponse
                {
                    ImportDutyAU = importDutyInfo.ImportDutyAustralia,
                    ImportDutyNZ = importDutyInfo.ImportDutyNewZealand
                });
            }
            
            return Ok(response);
        }


        
        [Route("save-import-duties")]
        [HttpPost]
        public async Task<ActionResult> SaveImportDuties([FromBody] SaveImportDutyRequest requests)
        {
            try
            {
                var importDutyInfo = new ImportDutyInfo()
                {
                    ImportDutyAustralia = requests.ImportDutyAU,
                    ImportDutyNewZealand = requests.ImportDutyNZ
                };

                _importDutyInfoProvider.Set(importDutyInfo);
                

                return Ok(new CommandAPIResult
                {
                   Success = true,
                   Message = "Saved."
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