import { useState, useEffect } from "react";
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import "./CustomerOrderHistory.css";

const OrderHistory = () => {
  const [orders, setOrders] = useState([]);
  const [filteredOrders, setFilteredOrders] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [filterRestaurant, setFilterRestaurant] = useState("");
  const [filterDate, setFilterDate] = useState("");

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    const day = date.getDate();
    const month = date.getMonth() + 1;
    const year = date.getFullYear();
    const hours = date.getHours();
    const minutes = date.getMinutes();

    const formattedDay = day < 10 ? '0' + day : day;
    const formattedMonth = month < 10 ? '0' + month : month;
    const formattedHours = hours < 10 ? '0' + hours : hours;
    const formattedMinutes = minutes < 10 ? '0' + minutes : minutes;
    
    return `${formattedDay}-${formattedMonth}-${year} ${formattedHours}:${formattedMinutes}`;
  };

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const customerId = localStorage.getItem("customerId");
        const auth = localStorage.getItem("auth");
        const userObject = JSON.parse(auth);
        const key = userObject.token;
        const response = await fetch(
          `https://localhost:7157/api/Customer/ViewOrderHistoryForCustomer?customerId=${customerId}`,{
            headers: {
              Authorization: `Bearer ${key}`,
            },
          }
        );
        if (!response.ok) {
          throw new Error(`Error: ${response.statusText}`);
        }
        const data = await response.json();
        setOrders(data);
        setLoading(false);
      } catch (error) {
        setError(error.message);
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  const applyFilters = () => {
    let filtered = orders;

    if (filterRestaurant) {
      filtered = filtered.filter(order =>
        order.restaurantName.toLowerCase().includes(filterRestaurant.toLowerCase())
      );
    }

    if (filterDate) {
      filtered = filtered.filter(order =>
        formatDate(order.orderDate).includes(filterDate)
      );
    }

    setFilteredOrders(filtered);
  };

  useEffect(() => {
    applyFilters();
  }, [orders, filterRestaurant, filterDate]);

  return (
    <>
        <h2>Orders History</h2>
   
      <div className="filter-options">
        <input
          type="text"
          placeholder="Filter by restaurant name"
          value={filterRestaurant}
          onChange={(e) => setFilterRestaurant(e.target.value)}
        />
        <input
          type="text"
          placeholder="Filter by date (DD-MM-YYYY)"
          value={filterDate}
          onChange={(e) => setFilterDate(e.target.value)}
        />
      </div>
    <div className="customer-content">

      {loading && <p>Loading...</p>}
      {error && <p>Error: {error}</p>}
      {filteredOrders.length === 0 && !loading && !error && <p>No orders found.</p>}
      {filteredOrders.length > 0 && (
        <div className="order-cards-container">
          {filteredOrders.map((order) => (
            <div key={order.orderId} className="order-card">
              <div className="order-card-header">
              </div>
              <div className="order-card-content">
                <p><strong>RestaurantName:</strong>{order.restaurantName}</p>
                <p><strong> OrderDate:</strong>{formatDate(order.orderDate)}</p>
                <p><strong>Menu Item:</strong> {order.menuName?.[0]?.manuItemName}</p>
                <p><strong>Total Amount:</strong> ${order.menuName?.[0]?.quantity * order.price}</p>
                {/* Add more order details if needed */}
              </div>
            </div>
          ))}
        </div>
      )}
      <ToastContainer />
    </div>
    </>
  );
};

export default OrderHistory;
