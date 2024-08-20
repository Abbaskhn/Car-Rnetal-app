using System.ComponentModel.DataAnnotations;

namespace application.model
{
    public class Vendor
    {
        [Key]
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Email {  get; set; }
        public int Phone { get; set; }
        public  string Company {  get; set; }
    }
}
