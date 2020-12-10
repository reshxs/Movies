using FluentValidation;
using Movies.Data;

namespace Movies.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserValidator : AbstractValidator<User>
    {
        
    }
}
