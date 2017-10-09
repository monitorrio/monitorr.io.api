using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.ModelBinding;

namespace Core
{
	public interface IValidatable { }
	public static class ValidationHelpers
	{
		public static void EnforceValidity(this IValidatable obj) {
			var results = obj.Validate().ToArray();
			if (results.Any())
				throw new ValidationException(results.Select(v => v.ErrorMessage).Join(Environment.NewLine));
		}

		public static bool IsValid(this IValidatable obj, ValidationContext validationContext = null) {
			return !obj.Validate(validationContext).Any();
		}

		public static IEnumerable<ValidationResult> Validate(this IValidatable obj, ValidationContext validationContext = null) {
			var results = new List<ValidationResult>();
			validationContext = validationContext ?? new ValidationContext(obj);
			Validator.TryValidateObject(obj, validationContext, results, validateAllProperties: true);
			return results.Except(ValidationResult.Success).SelectMany(flattenComposite).ToArray();
		}

		public static ModelStateDictionary ToHttpModelStateDictionary(this IEnumerable<ValidationResult> errors) {
			return errors.Aggregate(new ModelStateDictionary(), (d, res) => {
				d.AddModelError(String.Join(", ", res.MemberNames), res.ErrorMessage);
				return d;
			});
		}

		static IEnumerable<ValidationResult> flattenComposite(ValidationResult result) {
			var asComposite = result as CompositeValidationResult;
			if (asComposite == null)
				yield return result;
			else {
				foreach (var r in asComposite.Results.SelectMany(flattenComposite))
					yield return r;
			}
		}

	}
	/// <summary>
	/// Validate object properties recursively.
	/// </summary>
	public class ValidateObjectAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
			var objectResults = validateObj(value).Select(cloneWithPrefix("."));
			var asEnumerable = value as IEnumerable;
			var allResults = asEnumerable == null ? objectResults : objectResults.Concat( validateEnumeration(asEnumerable.GetEnumerator(), 0) );
			if (allResults.None()) return ValidationResult.Success;
			var results = allResults.Select(cloneWithPrefix(validationContext.DisplayName));
			return new CompositeValidationResult(results, Enumerable.Empty<string>(), validationContext.DisplayName);
		}

		static IEnumerable<ValidationResult> validateObj(object value) {
			var results = new List<ValidationResult>();
			Validator.TryValidateObject(value, new ValidationContext(value, null, null), results, true);
			return results.Except(ValidationResult.Success);
		}

		IEnumerable<ValidationResult> validateEnumeration(IEnumerator enumerator, int idx) {
			if (!enumerator.MoveNext())
				return Enumerable.Empty<ValidationResult>();
			return validateObj(enumerator.Current).Select(cloneWithPrefix("[{0}].".Fmt(idx)))
				.Concat(validateEnumeration(enumerator, idx + 1));
		}


		Func<ValidationResult, ValidationResult> cloneWithPrefix(string prefix) {
			return r => new ValidationResult(r.ErrorMessage, r.MemberNames.Select(n => prefix + n));
		}
	}

	public class CompositeValidationResult : ValidationResult
	{
		public IEnumerable<ValidationResult> Results { get; private set; }

		public CompositeValidationResult(IEnumerable<ValidationResult> errors) : this(errors, Enumerable.Empty<string>()) { }

		public CompositeValidationResult(IEnumerable<ValidationResult> results, IEnumerable<string> memberNames, string prefix = null) : base(createErrorMessage(results, prefix), memberNames) {
			Results = results;
		}

		static string createErrorMessage(IEnumerable<ValidationResult> results, string passedPrefix) {
			var prefix = passedPrefix.IfNotNull(x => x + ": ", String.Empty);
			return results.Except(ValidationResult.Success).Select(x => prefix + x.ErrorMessage).Join(Environment.NewLine);
		}
	}
}