import React, { useState } from 'react';
import service from './service';
import { useNavigate } from 'react-router-dom';
import './Auth.css';

function Register() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await service.register(username, password);
            alert("נרשמת בהצלחה! כעת תוכל להתחבר.");
            navigate('/login');
        } catch (error) {
            alert("ההרשמה נכשלה. ייתכן ששם המשתמש כבר קיים.");
        }
    };

    return (
      <div className="auth-container">.
        <div style={{ padding: '20px' }}>
            <h2>הרשמה למשתמש חדש</h2>
            <form onSubmit={handleSubmit}>
                <input type="text" placeholder="בחר שם משתמש" onChange={e => setUsername(e.target.value)} required /><br/>
                <input type="password" placeholder="בחר סיסמה" onChange={e => setPassword(e.target.value)} required /><br/>
                <button type="submit">הירשם</button>
            </form>
        </div></div>
    );
}

export default Register;