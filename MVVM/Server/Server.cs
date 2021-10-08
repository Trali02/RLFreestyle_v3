using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using RLFreestyle_v3.MVVM.Model;

namespace RLFreestyle_v3.MVVM.Server
{
	class Server
	{
		public static List<Client> clients = new List<Client>();
		public static string xmlData;

		public static int Port { get; private set; }
		public const int dataBufferSize = 4096;
		public delegate void PacketHandler(int _fromCient, Packet _packet);
		public static Dictionary<int, PacketHandler> packetHandlers;
		private static TcpListener server;
		
		public static void Start(int _port)
		{
			Port = _port;
			Console.WriteLine("Server Starting...");
			InitServerData();
			server = new TcpListener(IPAddress.Any, Port);

			try
			{
				IPAddress address = IPAddress.Parse(new WebClient().DownloadString("https://ipinfo.io/ip"));
				Console.WriteLine(address);
			}
			catch { }
			
			server.Start();
			server.BeginAcceptTcpClient(new AsyncCallback(ConnectCallback), null);

			Console.WriteLine($"Server started on Port {Port}");
		}
		public static void Stop()
		{
			try
			{
				foreach (Client c in clients) c.Disconnect();
				if(server != null) server.Stop();
			} catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		private static void ConnectCallback(IAsyncResult AR)
		{
			try
			{
				TcpClient _client = server.EndAcceptTcpClient(AR);
				server.BeginAcceptTcpClient(new AsyncCallback(ConnectCallback), null);
				Console.WriteLine($"Incoming Connection from {_client.Client.RemoteEndPoint}. Trying to Connect as {clients.Count}...");

				Client _c = new Client(clients.Count);
				_c.tcp.Connect(_client);
				_c.connected = true;
				clients.Add(_c);
				ServerSend.Welcome(_c.id, "Connection Successful!");
			} catch(Exception e)
			{

			}
		}

		private static void InitServerData()
		{
			packetHandlers = new Dictionary<int, PacketHandler>()
			{
				{(int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
				{(int)ClientPackets.getPoints, ServerHandle.GetPoints },
				{(int)ClientPackets.getPlayers, ServerHandle.GetPlayers },
				{(int)ClientPackets.addPlayer, ServerHandle.AddPlayer },
				{(int)ClientPackets.updatePlayer, ServerHandle.UpdatePlayer },
				{(int)ClientPackets.removePlayer, ServerHandle.RemovePlayer },
				{(int)ClientPackets.removeShot, ServerHandle.RemoveShot },
				{(int)ClientPackets.addShot, ServerHandle.AddShot },
				{(int)ClientPackets.setXml, ServerHandle.SetXml },
				{(int)ClientPackets.resetPlayer, ServerHandle.ResetPlayer },
				{(int)ClientPackets.setPlayerColor, ServerHandle.SetPlayerColor },
				{(int)ClientPackets.setLua, ServerHandle.SetLua },
				{(int)ClientPackets.saveGame, ServerHandle.SaveGame }
			};
		}
	}
}
