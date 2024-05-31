import React, { useState } from "react";
import "./SidebarRestaurant.css";
import OrdersByRestaurant from "../../pages/OrdersByRestaurant/OrdersByRestaurant";
import MenusByRestaurant from "../../pages/MenusByRestaurant/MenusByRestaurant";
import PaymentsByRestaurant from "../../pages/PaymentsByRestaurant/PaymentsByRestaurant";
const SidebarRestaurant = () => {
  const [showOrdersbyRestaurant, setShowOrdersbyRestaurant] = useState(false);
  const [showMenusbyRestaurant, setShowMenusbyRestaurant] = useState(false);
  const [showPaymentsbyRestaurant, setShowPaymentsbyRestaurant] =
    useState(false);

  const handleOrdersbyRestaurantClick = () => {
    setShowOrdersbyRestaurant(true);

    setShowMenusbyRestaurant(false);
    setShowPaymentsbyRestaurant(false);
  };
  const handleMenusbyRestaurantClick = () => {
    setShowOrdersbyRestaurant(false);

    setShowMenusbyRestaurant(true);
    setShowPaymentsbyRestaurant(false);
  };
  const handlePaymentsbyRestaurantClick = () => {
    setShowOrdersbyRestaurant(false);

    setShowMenusbyRestaurant(false);
    setShowPaymentsbyRestaurant(true);
  };

  return (
    <>
      <hr />
      <div className="parent">
        <div className="child1">
          <div className="sidebar-options">
            <div className="sidebar-option">
              <button onClick={handleMenusbyRestaurantClick}>Menus</button>
            </div>
            <div className="sidebar-option">
              <button onClick={handleOrdersbyRestaurantClick}>Orders</button>
            </div>
            <div className="sidebar-option">
              <button onClick={handlePaymentsbyRestaurantClick}>
                Payments
              </button>
            </div>
          </div>
        </div>
        <div className="child2">
          <div className="content">
            {showOrdersbyRestaurant && <OrdersByRestaurant />}
            {showMenusbyRestaurant && <MenusByRestaurant />}
            {showPaymentsbyRestaurant && <PaymentsByRestaurant />}
          </div>
        </div>
      </div>
    </>
  );
};

export default SidebarRestaurant;
