import React, { useEffect } from 'react';
import { useQuery } from 'react-query';
import { getTasks } from '../services/taskService';
import { Card, CardContent, Typography, CircularProgress, Alert, Button, Box, Grid } from '@mui/material';
import { Task } from '../types';
import { useNavigate } from 'react-router-dom';

// Instead of using styled, you can directly apply styles using sx or classes

// Remove these lines
// const TaskListContainer = styled(Box)(({ theme }) => ({
//   maxWidth: 1200,
//   margin: 'auto',
//   padding: theme.spacing(3),
//   marginTop: theme.spacing(4),
// }));

// const TaskCard = styled(Card)(({ theme }) => ({
//   marginBottom: theme.spacing(2),
//   padding: theme.spacing(2),
//   backgroundColor: theme.palette.background.default,
//   boxShadow: theme.shadows[3],
//   borderRadius: theme.shape.borderRadius,
// }));

// Instead, use inline sx prop for simplicity
const TaskListView: React.FC = () => {
    const { data: tasks, isLoading, error } = useQuery<Task[], Error>('tasks', getTasks);
    const navigate = useNavigate();
  
    if (isLoading) return <CircularProgress />;
    if (error) return <Alert severity="error">Error loading tasks</Alert>;
  
    return (
      <Box sx={{ maxWidth: 1200, margin: 'auto', padding: 3, marginTop: 4 }}>
        <Box display="flex" justifyContent="space-between" alignItems="center" marginBottom={3}>
          <Typography variant="h4" component="h1">
            Task List
          </Typography>
          <Button variant="contained" color="primary" onClick={() => navigate('/tasks/new')}>
            Create New Task
          </Button>
        </Box>
        <Grid container spacing={3}>
          {tasks?.map((task) => (
            <Grid item xs={12} sm={6} md={4} key={task.taskId}>
              <Card
                sx={{
                  marginBottom: 2,
                  padding: 2,
                  backgroundColor: 'background.default',
                  boxShadow: 3,
                  borderRadius: 2,
                }}
              >
                <CardContent>
                  <Typography variant="h6" gutterBottom>
                    {task.title}
                  </Typography>
                  <Typography variant="body2" color="textSecondary" gutterBottom>
                    {task.description || 'No description available'}
                  </Typography>
                  <Typography variant="subtitle2" color={task.completed ? 'success.main' : 'error.main'}>
                    {task.completed ? 'Completed' : 'Not Completed'}
                  </Typography>
                  <Box marginTop={2}>
                    <Button
                      variant="outlined"
                      color="primary"
                      size="small"
                      onClick={() => navigate(`/tasks/${task.taskId}`)}
                      sx={{ marginRight: 1 }}
                    >
                      View Details
                    </Button>
                    <Button
                      variant="contained"
                      color="secondary"
                      size="small"
                      onClick={() => navigate(`/tasks/${task.taskId}/edit`)}
                      sx={{ marginRight: 1 }}
                    >
                      Edit
                    </Button>
                    <Button
                      variant="contained"
                      color="error"
                      size="small"
                      onClick={() => navigate(`/tasks/${task.taskId}/delete`)}
                    >
                      Delete
                    </Button>
                  </Box>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Box>
    );
  };
  
  export default TaskListView;
  