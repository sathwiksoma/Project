import React, { useState, useEffect } from "react";

function UserDetails() {
  const [userData, setUserData] = useState(null); // Stores fetched user data// Tracks loading state
  const [error, setError] = useState(null); // Stores any errors

  // Fetch user data on component mount
  useEffect(() => {
    const fetchUserData = async () => {
      setError(null);

      try {
        const customerId = localStorage.getItem("customerId");
        const auth = localStorage.getItem("auth");
        const userObject = JSON.parse(auth);
        const key = userObject.token;
        const response = await fetch(
          `https://localhost:7157/api/Customer/GetCustomerDetails?customerId=${customerId}`,{
            headers: {
              Authorization: `Bearer ${key}`,
            },
          }
        );

        if (!response.ok) {
          throw new Error(`Error fetching data: ${response.statusText}`);
        }

        const data = await response.json();
        setUserData(data);
      } catch (err) {
        setError(err.message);
      }
    };

    fetchUserData();
  }, []); // Re-fetch on customerId change

  // Display loading indicator, error message, or user data
  return (
    <div className="customer-content">
      <div>
        <h2>User Details</h2>

        <div>
          {error ? (
            <p className="error">Error: {error}</p>
          ) : userData ? (
            <div>
              <p>Name: {userData.name}</p>
              <p>Email: {userData.email}</p>
              <p>Phone: {userData.phone}</p>
              <p>Username: {userData.userName}</p>
            </div>
          ) : (
            <p>No user data found.</p>
          )}
        </div>
      </div>
    </div>
  );
}

export default UserDetails;
