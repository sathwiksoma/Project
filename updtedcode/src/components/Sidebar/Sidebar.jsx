import React, { useState } from 'react';
import './Sidebar.css';
import AllOrders from '../../pages/AllOrders/AllOrders';
import AllRestaurants from '../../pages/AllRestaurants/AllRestaurants';
import AllMenus from '../../pages/AllMenus/AllMenus';

const Sidebar = ({activeItem, handleAllMenusClick, handleAllRestaurantsClick, handleAllOrdersClick}) => {
  const navItems = ["MENU", "RESTAURANTS", "ORDERS"] 
  
  return (
    <div className='sidebar'>
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

      <div className='content'>
        {showAllOrders && <AllOrders />}
        {showAllRestaurants && <AllRestaurants />}
        {showAllMenus && <AllMenus />}
      </div>
    </div>
  );
};

export default Sidebar;