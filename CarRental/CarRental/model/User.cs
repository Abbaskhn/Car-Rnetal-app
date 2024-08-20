using System.ComponentModel.DataAnnotations;

namespace application.model
{
    public class User
    {

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }


    }
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UserVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
    public class login
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}
