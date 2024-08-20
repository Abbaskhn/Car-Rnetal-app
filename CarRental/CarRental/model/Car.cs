using System.ComponentModel.DataAnnotations;

namespace application.model
{
    public class Car
    {
        [Key]
        public int carid {  get; set; }
        public int Model {  get; set; }

        public string CarName { get; set; }
        public int Rentalprice { get; set; }
        public string CarPhoto { get; set; }
      
    }
}
