import React, { useContext } from 'react';
import './FoodItem.css';
import { assets } from '../../assets/assets'
import { toast,ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { StoreContext } from '../../context/StoreContext';
import axios from 'axios';

const FoodItem = ({ id, name, price, description, image, restaurant }) => {
    const { cartItems, addToCart, removeFromCart } = useContext(StoreContext);

    const handleAddToCart = () => {
        // Retrieve customerId from sessionStorage
        const customerId = localStorage.getItem('customerId');
        console.log('Retrieved Customer ID:', customerId); // Log the retrieved customerId
        
      // Proceed with adding items to the cart using customerId
      // Make sure customerId is not null or undefined
      if (!customerId) {
        console.error('Customer ID is missing from sessionStorage.');
        toast.error("Please Sign in with your Credentials ");
        return;
    }
    
    addToCart(id); // Add the food item id to the cart
    console.log('itemid',id);
      // Prepare data for adding items to the cart
     
      // Make POST request to backend API to add items to the cart
      const auth = localStorage.getItem("auth");
        const userObject = JSON.parse(auth);
        const key = userObject.token;
      const response = axios.post(`https://localhost:7157/api/Customer/AddToCart?userId=${customerId}&menuItemId=${id}`,{
        headers: {
          Authorization: `Bearer ${key}`,
        },
      })
      .then(response => {
        console.log(response);
        // Handle successful response
        const cartId = response.data;
        console.log(cartId);
        localStorage.setItem('cartId', cartId);
      })
      


  
    };
    const handleRemoveCartItemQuantity = async (itemId) => {
        try {
          const cartId = localStorage.getItem('cartId');
          console.log(cartId);
         
      
          // Make DELETE request to remove quantity of item from the cart
          const response = await axios.put(`https://localhost:7157/api/Customer/DecreaseCartItemQuantity?cartId=${cartId}`
           
        );
          
          // Handle success response
          console.log(response.data);
          
          // Update the UI to reflect the removed quantity
          removeFromCart(itemId);
          toast.success("Quantity removed from the cart successfully.");
        } catch (error) {
          // Handle error
          console.error('Error removing quantity from cart:', error);
          toast.error("Failed to remove quantity from the cart.");
        }
      }
      

    return (
        <div className='food-item'>
            <div className="food-item-img-container">
                <img className='food-item-image' src={image} alt="" />
                {!cartItems[id] ?
                    <img className='add' onClick={handleAddToCart} src={assets.add_icon_white} alt="" />
                    :
                    <div className='food-item-counter'>
                        <img onClick={() => handleRemoveCartItemQuantity(id)} src={assets.remove_icon_red} alt="" />

                        <p>{cartItems[id]}</p>
                        <img onClick={handleAddToCart} src={assets.add_icon_green} alt="" />
                    </div>
                }
            </div>
            <div className="food-item-info">
                <div className="food-item-name-rating">
                    <p>{name}</p>
                </div>
                    <p>{restaurant}</p>
                <p className="food-item-desc">{description}</p>
                <p className="food-item-price">{price}</p>
                <button className="add-to-cart-btn" onClick={handleAddToCart}>Add to Cart</button>

            <ToastContainer/>
            </div>
        </div>

    );
};

export default FoodItem;
