import React from "react";
import "./RestaurantPage.css";
import SidebarRestaurant from "../../components/SidebarRestaurant/SidebarRestaurant";
const RestaurantPage = () => {
  return (
    <div className="Restaurant-page-header">
      <p className="Restaurant-title">Restaurant Portal</p>
      <SidebarRestaurant />
    </div>
  );
};

export default RestaurantPage;
