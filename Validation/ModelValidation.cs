using System.ComponentModel.DataAnnotations;
using dotnet_api_test.Exceptions.ExceptionResponses;
using dotnet_api_test.Models.Dtos;

namespace dotnet_api_test.Validation
{
    public static class ModelValidation
    {
        public static void ValidateCreateDishDto(CreateDishDto entity)
        {
            var validationResults = new List<ValidationResult>();
            ValidateEntityPropertyIsNotNull(nameof(entity.Name), entity.Name, validationResults);
            ValidateEntityPropertyIsNotNull(nameof(entity.Cost), entity.Cost, validationResults);
            ValidateEntityPropertyIsNotNull(nameof(entity.MadeBy), entity.MadeBy, validationResults);

            if (validationResults.Count > 0)
                throw new BadRequestExceptionResponse("Model is not valid because following properties are missing: " +
                                                      string.Join(", ", validationResults.Select(s => s.ErrorMessage).ToArray()));
        }

        public static void ValidateUpdateDishDto(UpdateDishDto entity)
        {
            var validationResults = new List<ValidationResult>();
            ValidateEntityPropertyIsNotNull(nameof(entity.Name), entity.Name, validationResults);
            ValidateEntityPropertyIsNotNull(nameof(entity.Cost), entity.Cost, validationResults);
            ValidateEntityPropertyIsNotNull(nameof(entity.MadeBy), entity.MadeBy, validationResults);

            if (validationResults.Count > 0)
                throw new BadRequestExceptionResponse("Model is not valid because following properties are missing: " +
                                                      string.Join(", ", validationResults.Select(s => s.ErrorMessage).ToArray()));
        }

        private static void ValidateEntityPropertyIsNotNull<T>(string entityName, T value, ICollection<ValidationResult> validationResults)
        {
            if (value == null) validationResults.Add(new ValidationResult(entityName));
        }
    }
}