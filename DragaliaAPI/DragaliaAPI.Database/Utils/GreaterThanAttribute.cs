using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DragaliaAPI.Database.Utils;

public class GreaterThanAttribute(string OtherProperty) : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        IComparable? firstVal = value as IComparable;
        IComparable? secondVal = GetSecondComparable(validationContext);

        if (firstVal == null || secondVal == null)
        {
            return new ValidationResult(
                string.Format(
                    "one or more of {0}, {1} are null",
                    validationContext.DisplayName,
                    OtherProperty
                )
            );
        }

        if (firstVal.CompareTo(secondVal) < 1)
        {
            return new ValidationResult(
                string.Format(
                    "{0} must be strictly greater than {1}",
                    validationContext.DisplayName,
                    OtherProperty
                )
            );
        }

        return ValidationResult.Success;
    }

    protected IComparable? GetSecondComparable(ValidationContext validationContext)
    {
        PropertyInfo? propertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
        if (propertyInfo != null)
        {
            object? secondValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
            return secondValue as IComparable;
        }

        return null;
    }
}
