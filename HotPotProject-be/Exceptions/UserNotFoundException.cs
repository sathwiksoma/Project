namespace HotPotProject.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {

        }

        public override string Message => "User not found";
    }
}
