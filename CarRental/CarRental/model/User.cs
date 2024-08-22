using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public int RoleId { get; set; }  // Foreign key reference to Role

    [ForeignKey("RoleId")]
    public Role Role { get; set; }  // Navigation property
  }
  public class Role
  {
    [Key]
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
    public int RoleId { get; set; }  // Use RoleId instead of Role string
  }

  public class login
  {
    public string UserName { get; set; }
    public string Password { get; set; }
  }

}
