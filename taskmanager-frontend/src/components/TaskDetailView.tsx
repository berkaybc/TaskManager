import React, { useEffect } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useMutation, useQuery } from 'react-query';
import { createTask, getTaskById, updateTask } from '../services/taskService';
import {
  Button,
  TextField,
  Box,
  CircularProgress,
  Alert,
  Paper,
  Typography,
  Switch,
  FormControlLabel,
} from '@mui/material';
import { Task } from '../types';
import { useNavigate, useParams } from 'react-router-dom';
import { styled } from '@mui/system';

type TaskFormInput = {
  title: string;
  description?: string;
  completed: boolean;
};

// Custom styled components
const FormContainer = styled(Paper)(({ theme }) => ({
  maxWidth: 600,
  margin: 'auto',
  marginTop: theme.spacing(4),
  padding: theme.spacing(4),
  borderRadius: theme.shape.borderRadius,
}));

const TaskFormView: React.FC = () => {
  const { register, handleSubmit, setValue } = useForm<TaskFormInput>();
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    if (id) {
      console.log(`Fetching task with id: ${id}`);
    }
  }, [id]);

  const isEditMode = !!id;

  const { data: task, isLoading, error } = useQuery<Task, Error>(['task', id], () => getTaskById(Number(id)), {
    enabled: isEditMode, // Only fetch if in edit mode
    onSuccess: (task) => {
      setValue('title', task.title);
      setValue('description', task.description || '');
      setValue('completed', task.completed);
    },
  });

  const mutation = useMutation(isEditMode ? updateTask : createTask, {
    onSuccess: () => {
      navigate('/');
    },
  });

  const onSubmit: SubmitHandler<TaskFormInput> = (data) => {
    const taskData: Task = {
      taskId: isEditMode ? Number(id) : 0,
      title: data.title,
      description: data.description,
      completed: data.completed,
    };
    mutation.mutate(taskData);
  };

  if (isEditMode && (isLoading || error)) {
    return isLoading ? <CircularProgress /> : <Alert severity="error">Error loading task</Alert>;
  }

  return (
    <FormContainer elevation={3}>
      <Typography variant="h5" gutterBottom>
        {isEditMode ? 'Update Task' : 'Create New Task'}
      </Typography>
      <Box component="form" onSubmit={handleSubmit(onSubmit)} sx={{ mt: 2 }}>
        <TextField
          label="Title"
          variant="outlined"
          fullWidth
          margin="normal"
          {...register('title', { required: true })}
        />
        <TextField
          label="Description"
          variant="outlined"
          fullWidth
          margin="normal"
          {...register('description')}
        />
        <FormControlLabel
          control={<Switch {...register('completed')} color="primary" />}
          label="Completed"
          sx={{ marginTop: 2 }}
        />
        <Button type="submit" variant="contained" color="primary" fullWidth sx={{ mt: 3 }}>
          {isEditMode ? 'Update Task' : 'Create Task'}
        </Button>
      </Box>
    </FormContainer>
  );
};

export default TaskFormView;
