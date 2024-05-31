namespace HotPotProject.Exceptions
{
    public class NoMenuAvailableException : ApplicationException
    {
        public NoMenuAvailableException()
        {

        }
        public NoMenuAvailableException(string v)
        {

        }

        public override string Message => "No menu available at the moment to display";
    }
}
