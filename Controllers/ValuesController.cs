using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;

namespace temp.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        [Route("api/body-or-form")]
        public IActionResult PostDoSomething([ModelBinder(typeof(BodyOrForm))] ModelData model)
        {
            return new OkObjectResult(new
            {
                model,
                Request.ContentType
            });
        }
    }

    public class BodyOrForm : IModelBinder
    {
        private readonly IModelBinderFactory factory;

        public BodyOrForm(IModelBinderFactory factory) => this.factory = factory;

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var contentType = 
                bindingContext.ActionContext.HttpContext.Request.ContentType;

            BindingInfo bindingInfo = new BindingInfo();
            if (contentType == "application/json")
            {
                bindingInfo.BindingSource = BindingSource.Body;
            }
            else if (contentType == "application/x-www-form-urlencoded")
            {
                bindingInfo.BindingSource = BindingSource.Form;
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            var binder = factory.CreateBinder(new ModelBinderFactoryContext
            {
                Metadata = bindingContext.ModelMetadata,
                BindingInfo = bindingInfo,
            });

            await binder.BindModelAsync(bindingContext);
        }
    }

    public class ModelData
    {
        public string Email { get; set; }
    }
}
