import * as signalR from '@microsoft/signalr';

const connection = new signalR.HubConnectionBuilder()
  .withUrl('http://localhost:5094/hubs/notification')
  .withAutomaticReconnect()
  .build();

export const startSignalRConnection = async () => {
  try {
    await connection.start();
    console.log('SignalR Connected.');
  } catch (err) {
    console.log('Error while establishing SignalR connection: ', err);
    setTimeout(startSignalRConnection, 5000);
  }
};

export const onTaskUpdate = (callback: (message: string) => void) => {
  connection.on('ReceiveMessage', callback);
};
