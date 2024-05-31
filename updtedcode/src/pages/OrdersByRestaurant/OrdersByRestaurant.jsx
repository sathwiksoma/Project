import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import "./OrdersByRestaurant.css";

const OrdersByRestaurant = () => {
  const { restaurantId } = useParams();
  const [orders, setOrdersByRestaurant] = useState([]);
  const [filteredOrders, setFilteredOrders] = useState([]);
  const [statusFilter, setStatusFilter] = useState("");
  const [dateFilter, setDateFilter] = useState("");

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    const day = date.getDate();
    const month = date.getMonth() + 1; // Month indexes are 0-based
    const year = date.getFullYear();
    const hours = date.getHours();
    const minutes = date.getMinutes();

    // Add leading zeros if needed
    const formattedDay = day < 10 ? '0' + day : day;
    const formattedMonth = month < 10 ? '0' + month : month;
    const formattedHours = hours < 10 ? '0' + hours : hours;
    const formattedMinutes = minutes < 10 ? '0' + minutes : minutes;
    
    return `${formattedDay}-${formattedMonth}-${year} ${formattedHours}:${formattedMinutes}`;
  };

  useEffect(() => {
    const fetchOrdersByRestaurant = async () => {
      try {
        const auth = localStorage.getItem("auth");
        const userObject = JSON.parse(auth);
        const key = userObject.token;
        const response = await axios.get(
          `https://localhost:7157/api/Restaurant/GetAllOrdersByRestaurant?restaurantId=${restaurantId}`, {
            headers: {
              Authorization: `Bearer ${key}`,
            },
          }
        );
        setOrdersByRestaurant(response.data);
      } catch (error) {
        console.error("Error while fetching orders", error);
      }
    };
    if (restaurantId) {
      fetchOrdersByRestaurant();
    } else {
      console.error("Restaurant ID is missing in URL parameters");
    }
  }, [restaurantId]);

  useEffect(() => {
    applyFilters();
  }, [orders, statusFilter, dateFilter]);

  const applyFilters = () => {
    let filtered = orders;

    if (statusFilter) {
      filtered = filtered.filter(order => order.status.toLowerCase().includes(statusFilter.toLowerCase()));
    }

    if (dateFilter) {
      filtered = filtered.filter(order => formatDate(order.orderDate).includes(dateFilter));
    }

    setFilteredOrders(filtered);
  };

  const handleChangeOrderStatus = async (orderId, newStatus) => {
    try {
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;
      await axios.put(
        `https://localhost:7157/api/Restaurant/ChangeOrderStatus?orderId=${orderId}&newStatus=${newStatus}`, null, {
          headers: {
            Authorization: `Bearer ${key}`,
          },
        }
      );
      fetchOrdersByRestaurant();
    } catch (error) {
      console.error("Error while changing order status:", error);
    }
  };

  return (
    <div className="container">
      <h2>Orders</h2>
      <div className="filter-options">
        <input
          type="text"
          placeholder="Filter by status"
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
        />
        <input
          type="text"
          placeholder="Filter by date (DD-MM-YYYY)"
          value={dateFilter}
          onChange={(e) => setDateFilter(e.target.value)}
        />
      </div>
      <div className="order-cards-container">
        {filteredOrders.map((order) => (
          <div key={order.orderId} className="order-card">
            <div className="order-card-header">
            </div>
            <div className="order-card-content">
              <p><strong>Order ID:</strong> {order.orderId}</p>
              <p><strong>Order Date:</strong> {formatDate(order.orderDate)}</p>
              <p><strong>Amount:</strong> {order.amount}</p>
              <p><strong>Status:</strong> {order.status}</p>
              <button
                className="change-status"
                onClick={() => handleChangeOrderStatus(order.orderId, "Delivered")}
              >
                Change Status
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default OrdersByRestaurant;
