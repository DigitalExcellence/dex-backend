using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Resources.Project;
using FluentValidation;

namespace API.Resources.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateProjectResourceValidator: AbstractValidator<CreateProjectResource>
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateProjectResourceValidator()
        {
            RuleFor(a => a.ShortDescription).Must(a => a.Length <= 170 && a.Length > 0)
                .WithMessage("The short description length should be between 1 and 170");

        }
    }
}
