import React, { useState, useEffect } from "react";
import axios from "axios";
import "./AllMenus.css";
import { assets } from "../../assets/assets";

export default function AllMenus() {
  const [menus, setMenus] = useState([]);
  const [image, setImage] = useState(false);
  const [newMenu, setNewMenu] = useState({
    name: "",
    type: "",
    price: "",
    description: "",
    cuisine: "",
    category:"",
    cookingTime: "",
    tasteInfo: "",
    itemImage: "",
    nutritionId: "",
    restaurantId: "",
  });

  const [showForm, setShowForm] = useState(false);
  const baseURL = "https://localhost:7157"; // Base URL for images


  useEffect(() => {
    fetchAllMenus();
  }, []);

  const fetchAllMenus = async () => {
    try {
      const auth = localStorage.getItem("auth");
        const userObject = JSON.parse(auth);
        const key = userObject.token;
      const response = await axios.get(
        "https://localhost:7157/api/Restaurant/GetAllMenus",{
          headers: {
            Authorization: `Bearer ${key}`,
          },
        }
      );
      console.log(response.data);
      // Modify the response data to include the full image URLs
      const menusWithImages = response.data.map(menu => ({
        ...menu,
        itemImageUrl: `${baseURL}/${menu.itemImage}` // Assuming `assets` is the base URL for your images
      }));
      console.log(menusWithImages);
      setMenus(menusWithImages);
    } catch (error) {
      console.error("Error while fetching menus", error);
    }
  };
  const handleDeleteMenu = async (id) => {
    console.log("id " + id);
    try {
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;
      const response = await axios.delete(
        `https://localhost:7157/api/Restaurant/DeleteMenuItem?menuId=${id}`,{
          headers: {
            Authorization: `Bearer ${key}`,
          },
        }
      );
      fetchAllMenus();
    } catch (error) {
      console.error("Error while Deleting Menus " + error);
    }
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
       // Ensure that the value is a non-negative number before updating the state
  if (name === "price" && parseFloat(value) < 0) {
    // Don't update the state if the value is negative
    return;
  }
    setNewMenu({ ...newMenu, [name]: value });
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
    formData.append("itemImage", newMenu.itemImage)
    formData.append("nutritionId", newMenu.nutritionId)
    formData.append("restaurantId", newMenu.restaurantId)







    try {
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;
      const response = await axios.post(
        "https://localhost:7157/api/Restaurant/AddMenuItem",
        formData,
        {
          headers: {
            Authorization: `Bearer ${key}`,
            "Content-Type": "application/json" // Specify content type as JSON
          }
        },
      );
      console.log("New menu added:", response.data);
      fetchAllMenus();
      resetForm();
    } catch (error) {
      console.error("Error while adding new menu:", error);
    }
  };

  const resetForm = () => {
    setNewMenu({
      name: "",
      type: "",
      price: "",
      description: "",
      cuisine: "",
      category:"",
      cookingTime: "",
      tasteInfo: "",
      itemImage: "",
      nutritionId: "",
      restaurantId: "",
    });
    setShowForm(false);
  };

  return (
    <>
      <h1 className="manage-menu">Manage Menu</h1>
      <div className="menus-container">
        <button className="add-menu-btn" onClick={() => setShowForm(!showForm)}>
          {showForm ? "Hide Form" : "Add Menu Item"}
        </button>
        {showForm && (
          <div className="form">
            <div>
              <h2>Add New Menu</h2>
              <form onSubmit={handleSubmit}>
                <div>
                  <label className="label-1">Name:</label>
                  <input className="input-1"
                    type="text"
                    name="name"
                    value={newMenu.name}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div>
                  <label className="label-1">Type:</label>
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
                    <option value="Non-Veg">Non-Veg</option>
                  </select>
                </div>
                <div>
                  <label className="label-1">Price:</label>
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
                  <label className="label-1">Description:</label>
                  <input className="input-1"
                    type="text"
                    name="description"
                    value={newMenu.description}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div>
                  <label className="label-1">Cuisine:</label>
                  
                  <select
                    className="form-select form-select-lg mb-3"
                    aria-label="Default select example"
                    name="cuisine"
                    type="text"
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
                  <label className="label-1">Category:</label>
                  <input className="input-1"
                    type="text"
                    name="category"
                    value={newMenu.category}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div>
                  <label className="label-1">CookingTime:</label>
                  <input className="input-1"
                    type="text"
                    name="cookingTime"
                    value={newMenu.cookingTime}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div>
                  <label className="label-1">TasteInfo:</label>
                  <input className="input-1"
                    type="text"
                    name="tasteInfo"
                    value={newMenu.tasteInfo}
                    onChange={handleInputChange}
                    required
                  />
                  </div>
                  <div className="image-upload">
                    <label className="Item-Image">ItemImage:</label>
                    <img src={newMenu.itemImage ?URL.createObjectURL(image):assets.upload_area} alt="" className="preview-image" />
                    <input
                      onChange={(e) => {
                        setNewMenu({ ...newMenu, itemImage: e.target.files[0].name });
                        setImage(e.target.files[0]);
                      }}
                      className="input-1"
                      type="file"
                      name="itemImage"
                      required
                    />
                  </div>


                  <div>
                    <label className="label-1">NutritionId:</label>
                    <input className="input-1"
                      type="text"
                      name="nutritionId"
                      value={newMenu.nutritionId}
                      onChange={handleInputChange}
                      required
                    />
                  </div>
                  <div>
                    <label className="label-1">RestaurantId:</label>
                    <input className="input-1"
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
          </div>
        )}
        <h2 className="header-all-menu">All Menu</h2>
        <table className="table table-striped">
          <thead>
            <tr>
              <th scope="col">MenuId</th>
              <th scope="col">ItemImage</th>
              <th scope="col">Name</th>
              <th scope="col">Type</th>
              <th scope="col">Price</th>
              <th scope="col">Description</th>
              <th scope="col">Cuisine</th>
              <th scope="col">Category</th>
              <th scope="col">CookingTime</th>
              <th scope="col">TasteInfo</th>
              <th scope="col">RestaurantId</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody>
            {menus.map((menu) => (
              <tr key={menu.menuId}>
                <td>{menu.menuId}</td>
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
                <td>{menu.restaurantId}</td>
                <td>
                  <button
                    className="delete-button"
                    onClick={() => handleDeleteMenu(menu.menuId)}
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