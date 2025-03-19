import axios from 'axios';

const apiUrl = "https://server-m62b.onrender.com";

const service = {
  // שליפת כל המשימות
  getTasks: async () => {
    try {
      const result = await axios.get(`${apiUrl}/`); // עדכון לכתובת נכונה
      return result.data;
    } catch (error) {
      console.error('Error fetching tasks:', error);
      return [];
    }
  },

  // הוספת משימה חדשה
  addTask: async (name) => {
    if (!name) {
      console.error('Task name is required!');
      return null;
    }
    try {
      const result = await axios.post(`${apiUrl}/addItem`, { name, isComplete: false }); // שונה מ- /items ל- /addItem
      console.log('Task added:', result.data);
      return result.data;
    } catch (error) {
      console.error('Error adding task:', error);
      return null;
    }
  },

  // עדכון סטטוס משימה (השלמה / לא הושלמה)
  setCompleted: async (id, isComplete) => {
    try{
      const response = await axios.put(`${apiUrl}/updateItem/${id}`,{
        isComplete:isComplete
      });
      console.log('Task updated:', response.data);
      return response.data;
    }catch(error){
      console.error('Error updating task:', error);
    }
  },

  // מחיקת משימה לפי ID
  deleteTask: async (id) => {
    try {
      const result = await axios.delete(`${apiUrl}/removeItem/${id}`); // שונה מ- /items/{id} ל- /removeItem/{id}
      console.log('Task deleted:', result.data);
      return result.data;
    } catch (error) {
      console.error('Error deleting task:', error);
      return null;
    }
  }
};

export default service;
//אפיון של המערכת אני רוצה שכל אחד ירותץ לי בנפרד לכן אנ עושה את זה 
