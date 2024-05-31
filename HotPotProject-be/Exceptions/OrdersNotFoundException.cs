namespace HotPotProject.Exceptions
{
    public class OrdersNotFoundException : ApplicationException
    {
        public OrdersNotFoundException()
        {

        }
        public OrdersNotFoundException( string v)
        {
        }
        public override string Message => "No orders available to show at the moment";
    }
}
