import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import TodoList from './TodoList';
import Register from './Register';
import Login from './Login';

function App() {
  const isAuthenticated = () => !!localStorage.getItem("token");

  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/" element={isAuthenticated() ? <TodoList /> : <Navigate to="/login" />} />
      </Routes>
    </Router>
  );
}

export default App;