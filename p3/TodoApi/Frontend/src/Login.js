import React, { useState } from 'react';
import service from './service'; 
import './Auth.css';

function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
await service.login(username, password);
      alert("התחברת בהצלחה!");
      window.location.href = "/"; 
    } catch (error) {
      alert("שם משתמש או סיסמה שגויים");
    }
  };

  return (
          <div className="auth-container">.
    <form onSubmit={handleSubmit}>
      <h2>התחברות</h2>
      <input type="text" placeholder="שם משתמש" onChange={e => setUsername(e.target.value)} />
      <input type="password" placeholder="סיסמה" onChange={e => setPassword(e.target.value)} />
      <button type="submit">כניסה</button>
    </form></div>
  );
}

export default Login;