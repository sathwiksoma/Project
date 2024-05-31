import React, { useState } from "react";
import { Form, Button, Alert } from "react-bootstrap";
import "./CustomerUpdateDetails.css";

const UserDetailsForm = () => {
  const [id, setId] = useState(0);
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [phone, setPhone] = useState("");
  const [userName, setUserName] = useState("");
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    try {
      const customerId = localStorage.getItem("customerId");
      const auth = localStorage.getItem("auth");
      const userObject = JSON.parse(auth);
      const key = userObject.token;
      setId(customerId);
      const response = await fetch(
        "https://localhost:7157/api/Customer/UpdateCustomerDetails",
        {
          method: "PUT",
          headers: {

            Authorization: `Bearer ${key}`,

            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            id: customerId,
            name,
            email,
            phone,
            userName,
          }),
        }
      );
      if (!response.ok) {
        throw new Error(`Error: ${response.statusText}`);
      }
      setSuccess(true);
      setName("");
      setEmail("");
      setPhone("");
      setUserName("");
    } catch (error) {
      setError(error.message);
    }
  };

  return (
    <div className="customer-content">
      <Form onSubmit={handleSubmit}>
        {error && <Alert variant="danger">{error}</Alert>}
        {success && (
          <Alert variant="success">User details submitted successfully!</Alert>
        )}
        <Form.Group controlId="formName">
          <Form.Label>Name</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </Form.Group>
        <Form.Group controlId="formEmail">
          <Form.Label>Email address</Form.Label>
          <Form.Control
            type="email"
            placeholder="Enter email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </Form.Group>
        <Form.Group controlId="formPhone">
          <Form.Label>Phone number</Form.Label>
          <Form.Control
            type="tel"
            placeholder="Enter phone number"
            value={phone}
            onChange={(e) => setPhone(e.target.value)}
            required
          />
        </Form.Group>
        <Form.Group controlId="formUsername">
          <Form.Label>Username</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter username"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
            required
          />
        </Form.Group>
        <Button variant="primary" type="submit">
          Submit
        </Button>
      </Form>
    </div>
  );
};

export default UserDetailsForm;
