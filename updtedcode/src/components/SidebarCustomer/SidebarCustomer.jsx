import { useState } from "react";
import "./SidebarCustomer.css";
import CustomerDetails from "../../pages/CustomerDetails/CustomerDetails";
import CustomerUpdateDetails from "../../pages/CustomerUpdateDetails/CustomerUpdateDetails";
import CustomerOrderHistory from "../../pages/CustomerOrderHistory/CustomerOrderHistory";
const SidebarRestaurant = () => {
  const [showCustomerDetails, setShowCustomerDetails] = useState(false);
  const [showCustomerUpdate, setShowCustomerUpdate] = useState(false);
  const [showCustomerOrderHistory, setShowCustomerOrderHistory] =
    useState(false);

  const handleShowCustomerDetailsClick = () => {
    setShowCustomerDetails(true);

    setShowCustomerUpdate(false);
    setShowCustomerOrderHistory(false);
  };
  const handleShowCustomerUpdateClick = () => {
    setShowCustomerDetails(false);

    setShowCustomerUpdate(true);
    setShowCustomerOrderHistory(false);
  };
  const handleShowCustomerOrderHistoryClick = () => {
    setShowCustomerDetails(false);

    setShowCustomerUpdate(false);
    setShowCustomerOrderHistory(true);
  };

  return (
    <>
      <hr />
      <div className="parent">
        <div className="child1">
          <div className="sidebar-options">
            <div className="sidebar-option">
              <button onClick={handleShowCustomerDetailsClick}>
                Customer Details
              </button>
            </div>
            <div className="sidebar-option">
              <button onClick={handleShowCustomerUpdateClick}>
                Customer Details Update
              </button>
            </div>
            <div className="sidebar-option">
              <button onClick={handleShowCustomerOrderHistoryClick}>
                Customer Order History
              </button>
            </div>
          </div>
        </div>
        <div className="child2">
          <div className="content">
            {showCustomerDetails && <CustomerDetails />}
            {showCustomerUpdate && <CustomerUpdateDetails />}
            {showCustomerOrderHistory && <CustomerOrderHistory />}
          </div>
        </div>
      </div>
    </>
  );
};

export default SidebarRestaurant;
