import React, { useContext, useState } from 'react';
import { toast,ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import './PlaceOrder.css';
import axios from 'axios';
import { StoreContext } from '../../context/StoreContext';
import { useNavigate } from 'react-router-dom';

const PlaceOrder = () => {
  const { getTotalCartAmount } = useContext(StoreContext);
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    street: '',
    city: '',
    state: '',
    zip: '',
    country: '',
    phone: ''
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleProceedToPayment = async (event) => {
    event.preventDefault();

    try {
      // Validate form fields
      const isFormValid = Object.values(formData).every(value => value.trim() !== '');
      if (!isFormValid) {
        toast.error('Please fill out all the fields.');
        return;
      }

      const customerId = localStorage.getItem('customerId');
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;
      const response = await axios.post(`https://localhost:7157/api/Customer/PlaceOrderForAll?customerId=${customerId}&paymentMode=online`, formData,{
        headers: {
          Authorization: `Bearer ${key}`,
        },
      });
      console.log(response.data);
     
      const partnerName = response.data.partnerName;
      localStorage.setItem('DeliveryPartnerName', partnerName);

      navigate('/OrderConformation', { state: { formData } });
    } catch (error) {
      console.error('Error placing order:', error);
      if(error.response && error.response.status === 500)
      {
        toast.error(' Add Items in cart Before proced to payment.');
      }
      else{
      toast.error('Error placing order. Please try again later.');
      }
    }
  };

  return (
    <form className='place-order' onSubmit={handleProceedToPayment}>
      <div className="place-order-left">
        <p className="title">Delivery Information</p>
        <div className="multi-field">
          <input type="text" name="firstName" placeholder='First Name' value={formData.firstName} onChange={handleChange} />
          <input type="text" name="lastName" placeholder='Last Name' value={formData.lastName} onChange={handleChange} />
        </div>
        <input type="email" name="email" placeholder='Email address' value={formData.email} onChange={handleChange} />
        <input type="text" name="street" placeholder='Street' value={formData.street} onChange={handleChange} />
        <div className="multi-field">
          <input type="text" name="city" placeholder='City' value={formData.city} onChange={handleChange} />
          <input type="text" name="state" placeholder='State' value={formData.state} onChange={handleChange} />
        </div>
        <div className="multi-field">
          <input type="text" name="zip" placeholder='Zip code' value={formData.zip} onChange={handleChange} />
          <input type="text" name="country" placeholder='Country' value={formData.country} onChange={handleChange} />
        </div>
        <input type="text" name="phone" placeholder='Phone' value={formData.phone} onChange={handleChange} />
      </div>
      <div className="place-order-right"></div>
      <div className="cart-total">
        <h2>Cart Totals</h2>
        <div>
          <div className="cart-total-details">
            <p>Subtotal</p>
            <p>{getTotalCartAmount()}</p>
          </div>
          <hr />
          <div className="cart-total-details">
            <p>Delivery Fee</p>
            <p>{getTotalCartAmount() === 0 ? 0 : 2}</p>
          </div>
          <hr />
          <div className="cart-total-details">
            <b>Total</b>
            <b>{getTotalCartAmount() === 0 ? 0 : getTotalCartAmount() + 2}</b>
          </div>
        </div>
        <button type="submit">PROCEED TO PAYMENT</button>
      </div>
    </form>
  );
};

export default PlaceOrder;
