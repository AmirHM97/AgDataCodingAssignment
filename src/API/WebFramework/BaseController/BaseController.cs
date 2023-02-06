using AgDataCodingAssignment.Application.Models.Common;
using AgDataCodingAssignment.SharedKernel.Extensions;
using AgDataCodingAssignment.WebFramework.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataCodingAssignment.WebFramework.BaseController
{

    public class BaseController : ControllerBase
    {
        protected IActionResult OperationResult(dynamic result)
        {
            if (result is null)
                return new ServerErrorResult("Server Error");

            if (!((object)result).IsAssignableFromBaseTypeGeneric(typeof(OperationResult<>)))
            {
                throw new InvalidCastException("Given Type is not an OperationResult<T>");
            }


            if (result.IsSuccess) return result.Result is bool ? Ok() : Ok(result.Result);

            if (result.IsNotFound)
            {

                ModelState.AddModelError("GeneralError", result.ErrorMessage);

                var notFoundErrors = new ValidationProblemDetails(ModelState);

                return NotFound(notFoundErrors.Errors);
            }

            ModelState.AddModelError("GeneralError", result.ErrorMessage);

            var badRequestErrors = new ValidationProblemDetails(ModelState);

            return BadRequest(badRequestErrors.Errors);

        }
    }
}
