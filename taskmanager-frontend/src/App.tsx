import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import TaskListView from './components/TaskListView';
import TaskDetailView from './components/TaskDetailView';
import TaskFormView from './components/TaskFormView';
import TaskDeleteView from './components/TaskDeleteView';

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<TaskListView />} />
        <Route path="/tasks/new" element={<TaskFormView />} />
        <Route path="/tasks/:id" element={<TaskDetailView />} />
        <Route path="/tasks/:id/edit" element={<TaskFormView />} />
        <Route path="/tasks/:id/delete" element={<TaskDeleteView />} />
      </Routes>
    </Router>
  );
};

export default App;
