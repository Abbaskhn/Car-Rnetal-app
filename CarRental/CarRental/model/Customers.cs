using System.ComponentModel.DataAnnotations;

namespace application.model
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email {  get; set; }
        public string Address { get; set; }
        public int Phone {  get; set; }
    }
}
