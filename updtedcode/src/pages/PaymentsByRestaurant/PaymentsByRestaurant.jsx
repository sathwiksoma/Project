import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./PaymentsByRestaurant.css";
import axios from "axios";

const PaymentsByRestaurant = () => {
  const { restaurantId } = useParams();
  const [payments, setPaymentsByRestaurant] = useState([]);
  const [filteredPayments, setFilteredPayments] = useState([]);
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
    const fetchPaymentsByRestaurant = async () => {
      try {
        const auth = localStorage.getItem("auth");
        const userObject = JSON.parse(auth);
        const key = userObject.token;
        const response = await axios.get(
          `https://localhost:7157/api/Restaurant/GetAllPaymentsByRestaurants?restaurantId=${restaurantId}`, {
            headers: {
              Authorization: `Bearer ${key}`,
            },
          }
        );
        setPaymentsByRestaurant(response.data);
      } catch (error) {
        console.error("Error while fetching payments", error);
      }
    };
    if (restaurantId) {
      fetchPaymentsByRestaurant();
    } else {
      console.error("Restaurant ID is missing in URL parameters");
    }
  }, [restaurantId]);

  useEffect(() => {
    applyFilters();
  }, [payments, dateFilter]);

  const applyFilters = () => {
    let filtered = payments;

    if (dateFilter) {
      filtered = filtered.filter(payment => formatDate(payment.date).includes(dateFilter));
    }

    setFilteredPayments(filtered);
  };

  return (
    <div className="container">
      <h2>Payments</h2>
      <div className="filter-options">
        <input
          type="text"
          placeholder="Filter by date (DD-MM-YYYY)"
          value={dateFilter}
          onChange={(e) => setDateFilter(e.target.value)}
        />
      </div>
      <div className="payment-cards-container">
        {filteredPayments.map((payment) => (
          <div key={payment.paymentId} className="payment-card">
            <div className="payment-card-header">
            </div>
            <div className="payment-card-content">
              <p><strong>Payment ID:</strong> {payment.paymentId}</p>
              <p><strong>Payment Mode:</strong> {payment.paymentMode}</p>
              <p><strong>Amount:</strong> {payment.amount}</p>
              <p><strong>Status:</strong> {payment.status}</p>
              <p><strong>Payment Date:</strong> {formatDate(payment.date)}</p>
              <p><strong>Order ID:</strong> {payment.orderId}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default PaymentsByRestaurant;
