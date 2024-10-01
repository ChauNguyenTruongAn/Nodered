﻿
using System.Diagnostics;
using System.Net.WebSockets;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MAUI_IOT.Models
{

    public partial class BaseSensor : ObservableObject
    {

        private ClientWebSocket clientWebSocket;
        [ObservableProperty]
        private string _receivedData = string.Empty;
        public BaseSensor()
        {
            clientWebSocket = new ClientWebSocket();
        }

        //hàm kết nối node-red bằng websocket
        public async Task ConnectAsync(Uri uri)
        {
            try
            {
                // Kiểm tra nếu WebSocket ở trạng thái không thể sử dụng lại
                if (clientWebSocket.State == WebSocketState.Closed || clientWebSocket.State == WebSocketState.Aborted)
                {
                    clientWebSocket.Dispose();
                    clientWebSocket = new ClientWebSocket();
                }
                //kết nối 
                await clientWebSocket.ConnectAsync(uri, CancellationToken.None);

                if (clientWebSocket.State == WebSocketState.Open)
                {
                    Debug.WriteLine("Connected successfully.");

                    // Đọc dữ liệu
                    await ReceiveData();
                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }


        //hàm đọc dữ liệu từ node-red thông qua websocket
        public async Task ReceiveData()
        {
            byte[] buffer = new byte[1024];
            while (clientWebSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                    //message = "{" + message + "}";
                    Console.WriteLine($"Received: {message}");

                    ReceivedData = message; // Update ReceivedData property, khi update thì nõ sẽ update trong setter, mà trong setter sẽ kích hoạt PropertyChanged 
                }
            }
        }

        public async Task CloseAsync()
        {
            if (clientWebSocket.State == WebSocketState.Open)
            {
                await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
        }
    }
}