import React from 'react';
import { useMutation, useQuery } from 'react-query';
import { deleteTask, getTaskById } from '../services/taskService';
import {
  Button,
  Card,
  CardContent,
  Typography,
  CircularProgress,
  Alert,
  Box,
  Divider,
} from '@mui/material';
import { Task } from '../types';
import { useNavigate, useParams } from 'react-router-dom';
import { styled } from '@mui/system';

// Custom styled components
const DeleteCard = styled(Card)(({ theme }) => ({
  maxWidth: 600,
  margin: 'auto',
  marginTop: theme.spacing(4),
  padding: theme.spacing(3),
  borderRadius: theme.shape.borderRadius,
}));

const ButtonContainer = styled(Box)(({ theme }) => ({
  display: 'flex',
  justifyContent: 'flex-end',
  marginTop: theme.spacing(3),
  gap: theme.spacing(2),
}));

const TaskDeleteView: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const { data: task, isLoading, error } = useQuery<Task, Error>(['task', id], () => getTaskById(Number(id)));

  const mutation = useMutation(() => deleteTask(Number(id)), {
    onSuccess: () => {
      navigate('/');
    },
  });

  if (isLoading) return <CircularProgress />;
  if (error) return <Alert severity="error">Error loading task</Alert>;

  return (
    <DeleteCard>
      <CardContent>
        <Typography variant="h4" color="textPrimary" gutterBottom>
          Confirm Deletion
        </Typography>
        <Divider />
        <Box mt={2}>
          <Typography variant="h6" color="textSecondary">
            Are you sure you want to delete the following task?
          </Typography>
          <Typography variant="h5" mt={2}>
            {task?.title}
          </Typography>
          <Typography variant="body2" color="textSecondary" mt={1}>
            {task?.description || 'No description available'}
          </Typography>
        </Box>
        <ButtonContainer>
          <Button
            variant="contained"
            color="error"
            onClick={() => mutation.mutate()}
          >
            Delete
          </Button>
          <Button
            variant="outlined"
            color="primary"
            onClick={() => navigate('/')}
          >
            Cancel
          </Button>
        </ButtonContainer>
      </CardContent>
    </DeleteCard>
  );
};

export default TaskDeleteView;
