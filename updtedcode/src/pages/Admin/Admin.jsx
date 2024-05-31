import React, { useState } from "react";
import "./Admin.css";
import { Routes, Route, useNavigate } from "react-router-dom";
import AllOrders from "../AllOrders/AllOrders";
import AllRestaurants from "../AllRestaurants/AllRestaurants";
import AllMenus from "../AllMenus/AllMenus";

const Admin = () => {
  const navigate = useNavigate();
  const [showAllOrders, setShowAllOrders] = useState(true);
  const [showAllRestaurants, setShowAllRestaurants] = useState(false);
  const [showAllMenus, setShowAllMenus] = useState(false);

  const handleAllOrdersClick = () => {
    setShowAllOrders(true);
    setShowAllRestaurants(false);
    setShowAllMenus(false);
  };

  const handleAllRestaurantsClick = () => {
    setShowAllOrders(false);
    setShowAllRestaurants(true);
    setShowAllMenus(false);
  };

  const handleAllMenusClick = () => {
    setShowAllOrders(false);
    setShowAllRestaurants(false);
    setShowAllMenus(true);
  };
  return (
    <div className="admin-page-header">
      <p className="admin-title">Admin Portal</p>
      <hr />
      <div className="parent">
        <div className="child1">
          <div className="sidebar-options">
            <div className="sidebar-option">
              <button onClick={handleAllOrdersClick}>Orders</button>
            </div>
            <div className="sidebar-option">
              <button onClick={handleAllRestaurantsClick}>Restaurants</button>
            </div>
            <div className="sidebar-option">
              <button onClick={handleAllMenusClick}>Menu</button>
            </div>
          </div>
        </div>
        <div className="child2">
            {showAllOrders && <AllOrders />}
            {showAllRestaurants && <AllRestaurants />}
            {showAllMenus && <AllMenus />}
        </div>
      </div>
    </div>
  );
};

export default Admin;