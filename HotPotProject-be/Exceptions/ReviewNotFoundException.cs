namespace HotPotProject.Exceptions
{
    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException()
        {

        }

        public override string Message => "No review found";
    }
}
