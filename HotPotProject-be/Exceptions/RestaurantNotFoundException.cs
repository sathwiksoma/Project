namespace HotPotProject.Exceptions
{
    public class RestaurantNotFoundException : ApplicationException
    {
        public RestaurantNotFoundException()
        {
        }

        public RestaurantNotFoundException(string v)
        {

        }

        public override string Message => "No Restaurant found";
    }
}
