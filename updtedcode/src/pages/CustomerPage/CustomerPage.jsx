import React from "react";
import "./CustomerPage.css";
import SidebarCustomer from "../../components/SidebarCustomer/SidebarCustomer";
const CustomerPage = () => {
  return (
    <div className="Restaurant-page-header">
      <p className="Restaurant-title">Customer Portal</p>
      <SidebarCustomer />
    </div>
  );
};

export default CustomerPage;
