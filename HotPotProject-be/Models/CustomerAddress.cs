using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class CustomerAddress
    {

        [Key]
        public int AddressId { get; set; }
        public int? CustomerId { get; set; }
        public string? HouseNumber { get; set; }
        public string? BuildingName { get; set; }
        public string? Locality { get; set; }
        public int? CityId { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; }
        public string? LandMark { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        public CustomerAddress()
        {

        }

        public CustomerAddress(int userId, string? houseNumber, string? buildingName, string? locality, int cityId, string? landMark)
        {
            CustomerId = userId;
            HouseNumber = houseNumber;
            BuildingName = buildingName;
            Locality = locality;
            CityId = cityId;
            LandMark = landMark;
        }

        public CustomerAddress(int addressId, int userId, string? houseNumber, string? buildingName, string? locality, int cityId, string? landMark)
        {
            AddressId = addressId;
            CustomerId = userId;
            HouseNumber = houseNumber;
            BuildingName = buildingName;
            Locality = locality;
            CityId = cityId;
            LandMark = landMark;
        }
    }
}
