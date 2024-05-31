namespace HotPotProject.Exceptions
{
    public class NoCustomerAddressFoundException: Exception
    {
        public NoCustomerAddressFoundException()
        {

        }

        public NoCustomerAddressFoundException(string message) : base(message)
        {

        }
    }
}
