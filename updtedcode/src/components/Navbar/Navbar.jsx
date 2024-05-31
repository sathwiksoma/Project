import React, { useContext, useState } from "react";
import "./Navbar.css";
import { assets } from "../../assets/assets";
import { Link } from "react-router-dom";
import { StoreContext } from "../../context/StoreContext";
import "bootstrap/dist/css/bootstrap.min.css";
import { useAuth } from "../../context/Auth";
import Admin from "../../pages/Admin/Admin";
import { toast } from "react-toastify";

const Navbar = ({ setShowLogin, restaurantFilter, handleRestaurantSearch }) => {
  const [menu, setMenu] = useState("home");
  const { getTotalCartAmount } = useContext(StoreContext);
  const [auth, setAuth] = useAuth();
  const handleLogout = () => {
    setAuth({
      ...auth,
      user: null,
      token: "",
    });
    localStorage.removeItem("auth");
    localStorage.clear();
    toast.success("Log out successful");
    window.location.href = `http://localhost:5173/`;

  };

  const handleCustomer = () => {
    window.location.href = `http://localhost:5173/Customer`;
  };

  return (
    <div className="navbar">
      <Link to="./">
        {" "}
        <img src={assets.logo} alt="" className="logo" />
      </Link>
      <ul className="navbar-menu">
        <Link
          to="/"
          onClick={() => setMenu("home")}
          className={menu === "home" ? "active" : ""}
          style={{ color: 'black', textDecoration: 'none' }} 
        >
          home
        </Link>
        <a
          href="#explore-menu"
          onClick={() => setMenu("menu")}
          className={menu === "menu" ? "active" : ""}
          style={{ color: 'black', textDecoration: 'none' }} 

        >
          menu
        </a>
        <a
          href="#app-download"
          onClick={() => setMenu("mobile App")}
          className={menu === "mobile App" ? "active" : ""}
          style={{ color: 'black', textDecoration: 'none' }} 

        >
          mobile App
        </a>
        <a
          href="#footer"
          onClick={() => setMenu("contact us")}
          className={menu === "contact us" ? "active" : ""}
          style={{ color: 'black', textDecoration: 'none' }} 

        >
          contact us
        </a>
      </ul>
      <div className="navbar-right">
        <div className="navbar-search-icon-img">
          <img src={assets.search_icon} alt="" />
          <div className="navbar-search-restaurant">
            <input
              type="text"
              placeholder="search by restaurant name"
              value={restaurantFilter}
              onChange={handleRestaurantSearch}
            />
          </div>
        </div>

        <div className="navbar-searh-icon">
          <Link to="./cart">
            {" "}
            <img src={assets.basket_icon} alt="" />
          </Link>
          <div className={getTotalCartAmount() === 0 ? "" : "dot"}></div>
        </div>

        {!auth?.user ? (
          <>
            {/* <li className="nav-item">
                    <NavLink to="/register" className="nav-link" href="#">
                      Register
                    </NavLink>
                  </li> */}
            <button onClick={() => setShowLogin(true)}>sign in</button>
          </>
        ) : (
          <>
            <h4>Hi {auth?.user}</h4>
            {auth.role === "Customer" ? (
              <div>
                <button onClick={handleCustomer}>
                  <img src={assets.profile_icon}></img>
                </button>
              </div>
            ) : (
              ""
            )}
            <button onClick={handleLogout}>sign Out</button>
          </>
        )}
      </div>
    </div>
  );
};

export default Navbar;