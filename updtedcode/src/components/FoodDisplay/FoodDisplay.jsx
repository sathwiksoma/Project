import React, { useContext, useState, useEffect } from "react";
import "./FoodDisplay.css";
import { StoreContext } from "../../context/StoreContext";
import FoodItem from "../FoodItem/FoodItem";
import axios from "axios";

const FoodDisplay = ({ category, restaurant }) => {
  const [menus, setMenus] = useState([]);
  const baseURL="https://localhost:7157"

  useEffect(() => {
    fetchAllMenus();
  }, []);

  const fetchAllMenus = async () => {
    try {
      const response = await axios.get(
        "https://localhost:7157/api/Restaurant/GetAllMenus"
      );
      console.log(response.data);
      // // Modify the response data to include the full image URLs
      const menusWithImages = response.data.map((menu) => ({
        ...menu,
        itemImage: `${baseURL}/${menu.itemImage}`, // Assuming assets is the base URL for your images
      }));
      console.log(menusWithImages);
      setMenus(menusWithImages);
    } catch (error) {
      console.error("Error while fetching menus", error);
    }
  };

  // Filter menus based on category and restaurant
  const filteredMenus = menus.filter((menu) => {
    if (category === "All" || menu.restaurant.restaurantName=== category) {
      if (!restaurant) {
        return true; // Include all menus if restaurant is not provided
      } else {
        return menu.restaurant.restaurantName === restaurant;
      }
    }
    return false;
  });

  return (
    <div className="food-display" id="food-display">
      <h2>Top dishes near you</h2>
      <div className="food-display-list">
        {filteredMenus.map((menu, index) => (
          <FoodItem
            key={index}
            id={menu.menuId}
            name={menu.name}
            restaurant={menu.restaurant.restaurantName}
            description={menu.description}
            price={menu.price}
            image={menu.itemImage} // Just pass the image URL as a string
          />
        ))}
      </div>
    </div>
  );
};

export default FoodDisplay;