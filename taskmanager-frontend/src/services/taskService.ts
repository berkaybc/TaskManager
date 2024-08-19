import axios from 'axios';
import { Task } from '../types';

const API_URL = 'http://localhost:5094/api/tasks'; // Replace with the actual URL and port of your API

export const getTasks = async (): Promise<Task[]> => {
  const response = await axios.get(API_URL);
  return response.data;
};

export const getTaskById = async (id: number): Promise<Task> => {
    console.log("id ", id);
  const response = await axios.get(`${API_URL}/${id}`);
  return response.data;
};

export const createTask = async (task: Task): Promise<Task> => {
  const response = await axios.post(API_URL, task);
  return response.data;
};

export const updateTask = async (task: Task): Promise<Task> => {
  const response = await axios.put(`${API_URL}/${task.taskId}`, task);
  return response.data;
};

export const deleteTask = async (id: number): Promise<void> => {
  await axios.delete(`${API_URL}/${id}`);
};
