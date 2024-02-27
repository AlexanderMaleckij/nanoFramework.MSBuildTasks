using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using nanoFramework.MSBuildTasks.Validation;

namespace nanoFramework.MSBuildTasks.Pipelines.ResX.Models
{
    public sealed class ResourcesSourceParsedInput : IValidatableObject
    {
        [Required]
        [DisplayName("Directory path")]
        public string DirectoryPath { get; set; }

        [Regex]
        [DisplayName("Regex filter")]
        public string RegexFilter { get; set; }

        [SearchPattern]
        [DisplayName("Search pattern")]
        public string SearchPattern { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (!string.IsNullOrEmpty(RegexFilter) && !string.IsNullOrEmpty(SearchPattern))
            {
                errors.Add(new ValidationResult("Either regex filter or search pattern may be specified for the same item."));
            }

            return errors;
        }
    }
}
