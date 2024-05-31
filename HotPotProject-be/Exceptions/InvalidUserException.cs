namespace HotPotProject.Exceptions
{
    public class InvalidUserException:ApplicationException
    {
        public InvalidUserException()
        {

        }

        public override string Message => "Invalid Username or password";
    }
}
