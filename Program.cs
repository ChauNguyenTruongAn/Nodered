using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MAUI_IOT.Models;

class Program
{
    public static async Task Main(string[] args)
    {
        BaseSensor baseSensor = new BaseSensor();
        await baseSensor.ConnectAsync(new Uri("wss://113.161.84.132:8081/ABCD/data"));
    }
}