import React from 'react';
import './OrderConformation.css';
import { useLocation } from 'react-router-dom';

const OrderConfirmation = () => {
  const location = useLocation();
  const { formData } = location.state || {}; // Retrieve form data from location state

  const deliveryTime = 30; // Change this value to the actual delivery time
  const partnerName = localStorage.getItem('DeliveryPartnerName');

  return (
    <div className="order-confirmation">
      <h2>Order Placed Successfully!</h2>

      <div className="order-details-card">
        {formData && (
          <>
            <p className="title">Delivery Information:</p>
            <p><strong>Name:</strong> {formData.firstName}</p>
            <p><strong>Email:</strong> {formData.email}</p>
            <p><strong>Phone Number:</strong> {formData.phone}</p>
            <p><strong>State:</strong> {formData.state}</p>
            <p><strong>City:</strong> {formData.city}</p>
            <p><strong>Street:</strong> {formData.street}</p>
          </>
        )}
      </div>

      <p className="order-success-message">Thank you! Have a nice day.</p>
      <p className="order-success-message">Your Order Delivered in 30 Mins</p>

     

    </div>
  );
};

export default OrderConfirmation;
