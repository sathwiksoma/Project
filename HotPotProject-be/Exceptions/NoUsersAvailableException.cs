namespace HotPotProject.Exceptions
{
    public class NoUsersAvailableException : ApplicationException
    {
        public NoUsersAvailableException()
        {

        }

        public override string Message => "No users available at the moment to display";
    }
}
