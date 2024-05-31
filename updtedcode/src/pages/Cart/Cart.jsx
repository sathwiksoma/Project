import React, { useContext } from "react";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./Cart.css";
import { StoreContext } from "../../context/StoreContext";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const Cart = () => {
  const { cartItems, foodList, removeFromCartAll, getTotalCartAmount } =
    useContext(StoreContext);
  const navigate = useNavigate();
  const baseURL = "https://localhost:7157"; // Base URL for images

  const handleProceedToCheckout = async () => {
    try {
      console.log("ProceedToCheckOut Called");
      if (Object.keys(cartItems).length > 0) {
        navigate("/order");
      } else {
        toast.error(
          "Please add items to the cart before proceeding to checkout."
        );
      }
    } catch (error) {
      console.error("Error proceeding to checkout:", error);
    }
  };

  const handleRemoveCartItem = async (itemId) => {
    try {
      console.log(itemId);
      const cartId = localStorage.getItem("cartId");
      console.log(cartId);
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.key;

      // Make DELETE request to remove item from the cart
      const response = await axios.delete(
        `https://localhost:7157/api/Customer/DeleteCartItem?cartId=${cartId}`,
        {
          headers: {
            Authorization: `Bearer ${key}`,
            
          }
        },
      );

      console.log(response.data);
      removeFromCartAll(itemId);
    } catch (error) {
      console.error("Error removing item from cart:", error);
      toast.error("Failed to remove item from the cart.");
    }
  };

  return (
    <div className="cart">
      <div className="cart-items">
        <div className="cart-items-title">
          <p>Id</p>
          <p>Items</p>
          <p>Title</p>
          <p>Price</p>
          <p>Quantity</p>
          <p>Total</p>
          <p>Remove</p>
        </div>
        <br />
        <hr />
        {foodList.map((item, index) => {
          if (cartItems[item.menuId] > 0) {
            return (
              
              <div key={item.menuId}>
                <div className="cart-items-title cart-items-item">
                  <p>{item.menuId}</p>
                  <img
                    src={`${baseURL}/${item.itemImage}`}
                    alt={item.name}
                    className="cart-item-image"
                  />


                  <p>{item.name}</p>
                  <p>{item.price}</p>
                  <p>{cartItems[item.menuId]}</p>
                  <p>{item.price * cartItems[item.menuId]}</p>
                  <p
                    onClick={() => handleRemoveCartItem(item.menuId)}
                    className="cross"
                  >
                    x
                  </p>
                </div>
                <hr />
              </div>
            );
          }
        })}
      </div>
      <div className="cart-bottom">
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
          <button onClick={handleProceedToCheckout}>PROCEED TO CHECKOUT</button>
        </div>
        <div className="cart-promocode">
          <div>
            <p>If you have a promo code, enter it here</p>
            <div className="cart-promocode-input">
              <input type="text" placeholder="promo code" />
              <button>Submit</button>
            </div>
          </div>
        </div>
      </div>
      <ToastContainer />
    </div>
  );
};

export default Cart;