import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import "./MenusByRestaurant.css";
import { assets } from "../../assets/assets";

export default function MenusByRestaurant() {
  const { restaurantId } = useParams();
  const [menus, setMenusByRestaurant] = useState([]);
  const [image, setImage] = useState(false);
  const [newMenu, setNewMenuByRestaurant] = useState({
    name: "",
    type: "",
    price: "",
    description: "",
    cuisine: "",
    category: "",

    cookingTime: "",
    tasteInfo: "",
    itemImage: "",
    nutritionId: "",
    restaurantId: "",
  });
  const [showForm, setShowForm] = useState(false);
  const baseImageUrl = "https://localhost:7157"; // Define the base URL for your images


  useEffect(() => {
    if (restaurantId) {
      setNewMenuByRestaurant((prevState) => ({ ...prevState, restaurantId }));
      fetchMenusByRestaurant();
    } else {
      console.error("Restaurant ID is missing in URL parameters");
    }
  }, [restaurantId]);
  const fetchMenusByRestaurant = async () => {
    console.log("restaurantId " + restaurantId);
    try {
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;

      const response = await axios.get(
       ` https://localhost:7157/api/Customer/GetMenuByRestaurant?restaurantId=${restaurantId}`, {
        headers: {
          Authorization: `Bearer ${key}`,
        },

      }

      );
      const menusWithImages = response.data.map((menu) => ({
        ...menu,
        itemImageUrl: `${baseImageUrl}/${menu.itemImage}`,

      }));
      console.log(menusWithImages);
      setMenusByRestaurant(menusWithImages);

    } catch (error) {
      console.error("Error while fetching menus", error);
    }
  };


  const handleDeleteMenuByRestaurant = async (id) => {
    try {
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;
      await axios.delete(
        `https://localhost:7157/api/Restaurant/DeleteMenuItem?menuId=${id}`, {
        headers: {
          Authorization: `Bearer ${key}`,
        },
      }
      );
      fetchMenusByRestaurant();
    } catch (error) {
      console.error("Error while Deleting Menus ", error);
    }
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
     // Ensure that the value is a non-negative number before updating the state
  if (name === "price" && parseFloat(value) < 0) {
    // Don't update the state if the value is negative
    return;
  }

    setNewMenuByRestaurant({ ...newMenu, [name]: value });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    const formData = new FormData();
    formData.append("name", newMenu.name)
    formData.append("type", newMenu.type)
    formData.append("price", newMenu.price)
    formData.append("description", newMenu.description)
    formData.append("cuisine", newMenu.cuisine)
    formData.append("category", newMenu.category)

    formData.append("cookingTime", newMenu.cookingTime)
    formData.append("tasteInfo", newMenu.tasteInfo)
    formData.append("itemImage", image)
    formData.append("nutritionId", newMenu.nutritionId)
    formData.append("restaurantId", newMenu.restaurantId)


    try {
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;
      
      const response = await axios.post(
        "https://localhost:7157/api/Restaurant/AddMenuItem",
        formData,
        
      );
      console.log("New menu added:", response.data);
      fetchMenusByRestaurant();
      resetForm();
    } catch (error) {
      console.error("Error while adding new menu:", error);
    }
  };

  const resetForm = () => {
    setNewMenuByRestaurant({
      name: "",
      type: "",
      price: "",
      description: "",
      cuisine: "",
      category: "",

      cookingTime: "",
      tasteInfo: "",
      itemImage: "",
      nutritionId: "",
      restaurantId: "",
    });
    setShowForm(false);
  };



  return (
    <div className="container">
      <h2>Manage Menus</h2>
      <button onClick={() => setShowForm(true)}>Add Menu Item</button>
      {showForm && (
        <div>
          <h2>Add New Menu</h2>
          <form onSubmit={handleSubmit}>
            <div>
              <label>Name:</label>
              <input
                type="text"
                name="name"
                value={newMenu.name}
                onChange={handleInputChange}
                required
              />
            </div>
            <div>
              <label>Type:</label>
              <select
                    className="form-select form-select-lg mb-3"
                    aria-label="Default select example"
                    name="type"
                    value={newMenu.type}
                    onChange={handleInputChange}
                    required
                  >
                    <option selected>select type</option>
                    <option value="Veg">Veg</option>
                    <option value="Non Veg">Non-Veg</option>
                  </select>
            </div>
            <div>
              <label>Price:</label>
              <input className="input-1"
                    type="number"
                    name="price"
                    value={newMenu.price}
                    onChange={handleInputChange}
                    pattern="[0-9]*"  // Only allow numbers
  
                    required
                  />
            </div>
            <div>
              <label>Description:</label>
              <input
                type="text"
                name="description"
                value={newMenu.description}
                onChange={handleInputChange}
                required
              />
            </div>
            <div>
              <label>Cuisine:</label>
              <select
                    className="form-select form-select-lg mb-3"
                    aria-label="Default select example"
                    name="cuisine"
                    
                    value={newMenu.cuisine}
                    onChange={handleInputChange}
                    required
                  >
               <option value="">Select cuisine</option> {/* Default option with selected attribute */}

      <option value="Indian Cuisine">Indian Cuisine</option>
    <option value="Chinese Cuisine">Chinese Cuisine</option>
    <option value="Japanese Cuisine">Japanese Cuisine</option>
    <option value="Italian Cuisine">Italian Cuisine</option>
    <option value="Georgian Cuisine">Georgia Cuisine</option>



                  </select>
            </div>
            <div>
              <label>Category:</label>
              <input
                type="text"
                name="category"
                value={newMenu.category}
                onChange={handleInputChange}
                required
              />
            </div>
            <div>
              <label>CookingTime:</label>
              <input
                type="text"
                name="cookingTime"
                value={newMenu.cookingTime}
                onChange={handleInputChange}
                required
              />
            </div>
            <div>
              <label>TasteInfo:</label>
              <input
                type="text"
                name="tasteInfo"
                value={newMenu.tasteInfo}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="image-upload">
              <label className="Item-Image">ItemImage:</label>
              <img src={newMenu.itemImage ? URL.createObjectURL(image) : assets.upload_area} alt="" className="preview-image" />
              <input
                onChange={(e) => {
                  setNewMenuByRestaurant({ ...newMenu, itemImage: e.target.files[0].name });
                  setImage(e.target.files[0]);
                }}
                className="input-1"
                type="file"
                name="itemImage"
                
              />
            </div>
            <div>
              <label>NutritionId:</label>
              <input
                type="text"
                name="nutritionId"
                value={newMenu.nutritionId}
                onChange={handleInputChange}
                required
              />
            </div>
            <div>
              <label>RestaurantId:</label>
              <input
                type="text"
                name="restaurantId"
                value={newMenu.restaurantId}
                onChange={handleInputChange}
                required
              />
            </div>
            <button type="submit">Add Menu</button>
          </form>
        </div>
      )}
      <h2>Menus</h2>
      <table className="table table-striped">
          <thead>
            <tr>
              <th scope="col">ItemImage</th>
              <th scope="col">Name</th>
              <th scope="col">Type</th>
              <th scope="col">Price</th>
              <th scope="col">Description</th>
              <th scope="col">Cuisine</th>
              <th scope="col">Category</th>
              <th scope="col">CookingTime</th>
              <th scope="col">TasteInfo</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody>
            {menus.map((menu) => (
              <tr key={menu.menuId}>
                <td>
                  <img
                    src={menu.itemImageUrl}
                    alt={menu.name}
                    style={{ width: "100px", height: "100px" }}
                  />
                </td>
                <td>{menu.name}</td>
                <td>{menu.type}</td>
                <td>{menu.price}</td>
                <td>{menu.description}</td>
                <td>{menu.cuisine}</td>
                <td>{menu.category}</td>
                <td>{menu.cookingTime}</td>
                <td>{menu.tasteInfo}</td>
                <td>
                  <button
                    className="delete-button"
                    onClick={() => handleDeleteMenuByRestaurant(menu.menuId)}
                  >
                    X
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
    </div>
  );
};