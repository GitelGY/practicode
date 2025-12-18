import React, { useEffect, useState } from 'react';
import service from './service';
import { useNavigate } from 'react-router-dom';
import './TodoList.css';

function TodoList() {
    const [tasks, setTasks] = useState([]);
    const [newTaskName, setNewTaskName] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        loadTasks();
    }, []);

    const loadTasks = async () => {
        try {
            const data = await service.getTasks();
            setTasks(data);
        } catch (error) {
            console.error("טעינת המשימות נכשלה", error);
        }
    };

    const handleAddTask = async (e) => {
        e.preventDefault();
        if (!newTaskName.trim()) return;

        try {
            await service.addTask(newTaskName);
            setNewTaskName(''); 
            loadTasks(); 
        } catch (error) {
            alert("שגיאה בהוספת המשימה");
        }
    };

    const handleToggleStatus = async (id, currentStatus) => {
        try {
            await service.setCompleted(id, !currentStatus);
            loadTasks(); 
        } catch (error) {
            console.error("עדכון המשימה נכשל", error);
        }
    };

    const handleDeleteTask = async (id) => {
        try {
            await service.deleteTask(id);
            setTasks(prevTasks => prevTasks.filter(task => task.id !== id));
        } catch (error) {
            console.error("מחיקת המשימה נכשלה", error);
            alert("לא ניתן למחוק את המשימה. נסה שוב.");
        }
    };

    const handleLogout = () => {
        localStorage.removeItem("token");
        navigate('/login');
    };

    return (
        <div className="todo-container">
            <div className="todo-header">
                <h2>הרשימה שלי</h2>
                <button className="logout-btn" onClick={handleLogout}>התנתק</button>
            </div>

            <form className="add-task-form" onSubmit={handleAddTask}>
                <input 
                    type="text" 
                    placeholder="מה צריך לעשות היום?" 
                    value={newTaskName}
                    onChange={(e) => setNewTaskName(e.target.value)}
                    required
                />
                <button type="submit" className="add-btn">הוסף</button>
            </form>

            <div className="task-list">
                {tasks.length === 0 ? (
                    <p className="empty-msg">אין משימות ברשימה...</p>
                ) : (
                    tasks.map(task => (
                        <div key={task.id} className={`todo-item ${task.isComplete ? 'completed' : ''}`}>
                            <div className="task-content">
                                <input 
                                    type="checkbox" 
                                    checked={task.isComplete} 
                                    onChange={() => handleToggleStatus(task.id, task.isComplete)}
                                />
                                <span className="task-name">{task.name}</span>
                            </div>
                            <button 
                                className="delete-btn" 
                                onClick={() => handleDeleteTask(task.id)}
                            >
                                מחק
                            </button>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
}

export default TodoList;