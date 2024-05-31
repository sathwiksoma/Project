namespace HotPotProject.Exceptions
{
    public class RestaurantOwnerNotFoundException : ApplicationException
    {
        public RestaurantOwnerNotFoundException()
        {
        }

        public RestaurantOwnerNotFoundException(string v)
        {

        }


        public override string Message => "No RestaurantOwner found";
    }
}
