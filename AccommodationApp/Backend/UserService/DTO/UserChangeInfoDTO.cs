using Users.Model;

namespace Users.DTO
{
    public class UserChangeInfoDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }

        public Address Address { get; set; }

        public UserChangeInfoDTO() { }
    }
}
