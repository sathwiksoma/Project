namespace HotPotProject.Exceptions
{
    public class PaymentsNotFoundException : ApplicationException
    {
        public PaymentsNotFoundException()
        {

        }

        public override string Message => "No Payment records available at the moment";
    }
}
