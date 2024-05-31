namespace HotPotProject.Exceptions
{
    public class NoDeliveryPartnerFoundException : ApplicationException
    {
        public NoDeliveryPartnerFoundException()
        {

        }

        public override string Message => "No delivery partner found";
    }
}
