import React, { useState } from 'react'
import Navbar from './components/Navbar/Navbar'
import { Route, Routes } from 'react-router-dom'
import Home from './pages/Home/Home'
import Cart from './pages/Cart/Cart'
import PlaceOrder from './pages/PlaceOrder/PlaceOrder'
import Footer from './components/Footer/Footer'
import LoginPopup from './components/LoginPopup/LoginPopup'
import AllOrders from './pages/AllOrders/AllOrders'
import Admin from './pages/Admin/Admin'
import Sidebar from './components/Sidebar/Sidebar'
import AllRestaurants from './pages/AllRestaurants/AllRestaurants'
import AllMenus from './pages/AllMenus/AllMenus'
import RestaurantPage from './pages/RestaurantPage/RestaurantPage'
import OrderConformation from './pages/OrderConformation/OrderConformation'
import CustomerPage from './pages/CustomerPage/CustomerPage'



const App = () => {
  const [showLogin,setShowLogin] = useState(false);
  const [filterRestaurant, setFilterRestaurant] = useState("");
  const handleSearchByRestaurant = (e) => {
    console.log(e.target.value);
    setFilterRestaurant(e.target.value);
  };

  return (
    <>
    {showLogin?<LoginPopup setShowLogin={setShowLogin}/>:<></>}
    <div className='app'>
      <Navbar setShowLogin={setShowLogin}
      restaurantFilter={filterRestaurant}
      handleRestaurantSearch={handleSearchByRestaurant}/>
      <Routes>
        <Route path='/' element={<Home restaurant={filterRestaurant}/>} />
        <Route path='/cart' element={<Cart/>} />
        <Route path='/order' element={<PlaceOrder/>}/>
        <Route path='/AllOrders' element={<AllOrders/>}/>
        <Route path='/Admin' element={<Admin/>}/>
        <Route path='/AllRestaurants' element={<AllRestaurants/>}/>
        <Route path='/AllMenus' element={<AllMenus/>}/>
        <Route path="/RestaurantPage/:restaurantId" element={<RestaurantPage />} />
        <Route path='/OrderConformation' element={<OrderConformation/>}/>
        <Route path="/Customer" element={<CustomerPage />} />
 
        
      </Routes>
    </div>
   
    <Footer/>
    </>
  )
}

export default App