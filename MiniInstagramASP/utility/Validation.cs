using System.ComponentModel.DataAnnotations;

namespace MiniInstagramASP.utility
{
    public static class UserValidation
    {
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
        public const int UsernameMaxLength = 30;
        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 100;
        public const int AgeMin = 0;
        public const int AgeMax = 200;

        public const string LettersOnlyPattern = "^[A-Za-zÁČĎÉĚÍŇÓŘŠŤÚŮÝŽáčďéěíňóřšťúůýž]+$";
        public const string LettersOnlyErrorMessage = "Only letters A–Z (and accents) are allowed.";
        
        public const string LettersAndNumbersPattern = "^[A-Za-zÁČĎÉĚÍŇÓŘŠŤÚŮÝŽáčďéěíňóřšťúůýž0-9]+$";
        public const string LettersAndNumbersErrorMessage = "Only letters A–Z, accented letters, and numbers are allowed.";
    }

    public class LettersOnlyAttribute : RegularExpressionAttribute
    {
        public LettersOnlyAttribute() 
            : base(UserValidation.LettersOnlyPattern)
        {
            ErrorMessage = UserValidation.LettersOnlyErrorMessage;
        }
    }
    
    public class LettersAndNumbersOnlyAttribute : RegularExpressionAttribute
    {
        public LettersAndNumbersOnlyAttribute() 
            : base(UserValidation.LettersAndNumbersPattern)
        {
            ErrorMessage = UserValidation.LettersAndNumbersErrorMessage;
        }
    }
}
