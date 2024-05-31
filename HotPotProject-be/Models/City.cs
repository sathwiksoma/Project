using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }

        [ForeignKey("StateId")]
        public State? State { get; set; }

        public City()
        {

        }

        public City(int cityId, string name, int stateId)
        {
            CityId = cityId;
            Name = name;
            StateId = stateId;
        }

        public City(string name, int stateId)
        {
            Name = name;
            StateId = stateId;
        }
    }
}
