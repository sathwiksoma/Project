import React, { useState, useEffect } from "react";
import axios from "axios";
import "./AllRestaurants.css";

export default function AllRestaurants() {
  const [restaurants, setRestaurants] = useState([]);
  const [newRestaurant, setNewRestaurant] = useState({
    restaurantName: "",
    phone: "",
    email: "",
    cityId: "",
    restaurantImage: "",
  });
  const [showForm, setShowForm] = useState(false); // State variable to toggle the form visibility

  useEffect(() => {
    fetchAllRestaurants();
  }, []);

  const fetchAllRestaurants = async () => {
    try {
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;
      const response = await axios.get(
        "https://localhost:7157/api/Restaurant/GetAllRestaurants",{
          headers: {
            Authorization: `Bearer ${key}`,
          },
        }
      );
      setRestaurants(response.data);
    } catch (error) {
      console.error("Error while fetching restaurants", error);
    }
  };
  const handleDeleteRestaurant = async (id) => {
    console.log("id " + id);
    try {
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;
      const response = await axios.delete(
        `https://localhost:7157/api/Restaurant/DeleteRestaurant?restaurantId=${id}`,{
          headers: {
            Authorization: `Bearer ${key}`,
          },
        }
      );
      fetchAllRestaurants();
    } catch (error) {
      console.error("Error while Deleting Restaurants " + error);
    }
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setNewRestaurant({ ...newRestaurant, [name]: value });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    try {
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;
      const response = await axios.post(
        "https://localhost:7157/api/Restaurant/AddRestaurant",
        newRestaurant,{ headers: {
          Authorization: `Bearer ${key}`,
        },}
      );
      console.log("New restaurant added:", response.data);
      // Fetch all restaurants again to update the list
      fetchAllRestaurants();
      // Reset the form fields
      setNewRestaurant({
        restaurantName: "",
        phone: "",
        email: "",
        cityId: "",
        restaurantImage: "",
      });
      // Hide the form after submission
      setShowForm(false);
    } catch (error) {
      console.error("Error while adding new restaurant:", error);
    }
  };

  return (
    <>
      <div className="container">
        <h1>Manage Restaurants</h1>
        <button onClick={() => setShowForm(!showForm)}>
          {showForm ? "Hide Form" : "Add Restaurant"}
        </button>
        {showForm && (
          <div className="form">
            <div>
              <h4>Add New Restaurant</h4>

              <form onSubmit={handleSubmit}>
                <div>
                  <label>Restaurant Name:</label>
                  <input
                    type="text"
                    name="restaurantName"
                    value={newRestaurant.restaurantName}
                    onChange={handleInputChange}
                    required
                  />
                </div>

                <div>
                  <label>Phone:</label>
                  <input
                    type="text"
                    name="phone"
                    value={newRestaurant.phone}
                    onChange={handleInputChange}
                    required
                  />
                </div>

                <div>
                  <label>Email:</label>
                  <input
                    type="email"
                    name="email"
                    value={newRestaurant.email}
                    onChange={handleInputChange}
                    required
                  />
                </div>

                <div>
                  <label>City ID:</label>
                  <input
                    type="text"
                    name="cityId"
                    value={newRestaurant.cityId}
                    onChange={handleInputChange}
                    required
                  />
                </div>

                <div>
                  <label>Restaurant Image:</label>
                  <input
                    type="text"
                    name="restaurantImage"
                    value={newRestaurant.restaurantImage}
                    onChange={handleInputChange}
                  />
                </div>

                <button type="submit">Submit</button>
              </form>
            </div>
          </div>
        )}

        <h2>All Restaurants</h2>
        <table className="table-left-restaurant">
          <thead>
            <tr>
              <th>Restaurant ID</th>
              <th>Restaurant Name</th>
              <th>Phone</th>
              <th>Email</th>
              <th>RestaurantImage</th>
              <th>Remove</th>
            </tr>
          </thead>
          <tbody>
            {restaurants.map((restaurant) => (
              <tr key={restaurant.restaurantId}>
                <td>{restaurant.restaurantId}</td>
                <td>{restaurant.restaurantName}</td>
                <td>{restaurant.phone}</td>
                <td>{restaurant.email}</td>
                <td>{restaurant.restaurantImage}</td>
                <td>
                  <button
                    className="delete-button"
                    onClick={() =>
                      handleDeleteRestaurant(restaurant.restaurantId)
                    }
                  >
                    X
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </>
  );
}
