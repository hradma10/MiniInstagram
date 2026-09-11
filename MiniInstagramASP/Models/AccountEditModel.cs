using System.ComponentModel.DataAnnotations;
using MiniInstagramASP.utility;

public class AccountEditModel
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    [LettersOnly]
    [StringLength(UserValidation.FirstNameMaxLength)]
    public string FirstName { get; set; }

    [Required]
    [LettersOnly]
    [StringLength(UserValidation.LastNameMaxLength)]
    public string LastName { get; set; }

    [Required]
    [Range(UserValidation.AgeMin, UserValidation.AgeMax, ErrorMessage = "Age must be between 0 and 200.")]
    public int Age { get; set; }

    [Required]
    [LettersOnly]
    [StringLength(UserValidation.UsernameMaxLength)]
    public string Username { get; set; }

    public string Description { get; set; }
}