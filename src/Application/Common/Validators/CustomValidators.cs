﻿using FluentValidation;

namespace Application.Common.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> IdMustMatchGuidPattern<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$");
        }
    }
}