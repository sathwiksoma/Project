namespace HotPotProject.Exceptions
{
    public class PaymentFailedException : ApplicationException
    {
        public PaymentFailedException()
        {

        }

        public override string Message => "Could not process the payment at the moment. Please try again";
    }
}
