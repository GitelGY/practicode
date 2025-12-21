import axios from 'axios';

// axios.defaults.baseURL = "http://localhost:5047";
axios.defaults.baseURL = process.env.REACT_APP_API_URL;

axios.interceptors.response.use(
  response => response,
  error => {
    console.error('API Error:', {
      message: error.message,
      status: error.response?.status,
      url: error.config?.url
    });
    return Promise.reject(error);
  }
);

export default {
  register: async (username, password) => {
    const result = await axios.post("/register", { username, password });
    return result.data;
  },

  login: async (username, password) => {
    const result = await axios.post("/login", { username, password });
    if (result.data.token) {
      localStorage.setItem("token", result.data.token); 
    }
    return result.data;
  },
  getTasks: async () => {
    const result = await axios.get("/Item");
    return result.data;
  },

  addTask: async (name) => {
    const result = await axios.post("/Item", { name: name, isComplete: false });
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    const result = await axios.put(`/Item/${id}`, { isComplete: isComplete });
    return result.data;
  },

  deleteTask: async (id) => {
    const result = await axios.delete(`/Item/${id}`);
    return result.data;
  }
};

axios.interceptors.request.use(config => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

axios.interceptors.response.use(
  response => response,
  error => {
    if (error.response && error.response.status === 401) {
      console.warn("לא מורשה - עובר לדף התחברות");
      localStorage.removeItem("token");
      window.location.href = "/Login"; 
    }
    return Promise.reject(error);
  }
);