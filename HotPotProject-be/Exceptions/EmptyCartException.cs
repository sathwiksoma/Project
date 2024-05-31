namespace HotPotProject.Exceptions
{
    public class EmptyCartException : ApplicationException
    {
        public EmptyCartException()
        {

        }

        public override string Message => "Your cart is empty";
    }
}
