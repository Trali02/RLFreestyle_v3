using RLFreestyle_v3.MVVM.Model;
using RLFreestyle_v3.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace RLFreestyle_v3.MVVM.Server
{
	class ServerHandle
	{
		public static void WelcomeReceived(int _fromClient, Packet _packet)
		{
			int _clientIdCheck = _packet.ReadInt();
			int _id = _fromClient;
			if(_clientIdCheck == _id && Server.clients[_clientIdCheck].tcp.socket != null)
			{
				Console.WriteLine($"{Server.clients[_clientIdCheck].tcp.socket.Client.RemoteEndPoint} Connected as {_id}!");
				using (Packet packet = new Packet((int)ServerPackets.setXml))
				{
					packet.Write(ShotlistLoader.getData());
					ServerSend.SendTCPData(_fromClient, packet);
				}

				foreach(Player p in Player.Players)
				{
					using(Packet packet = new Packet((int)ServerPackets.addPlayer))
					{
						packet.Write(p);
						ServerSend.SendTCPData(_fromClient, packet);
					}
				}
			}
			else
			{
				Console.WriteLine("Something went horribly wrong!!!");
			}
		}
		public static void GetPoints(int _fromClient, Packet packet)
		{
			Console.WriteLine($"Get Points received from {_fromClient}");
		}
		public static void GetPlayers(int _fromClient, Packet packet)
		{
			Application.Current.Dispatcher.Invoke(new Action(() => { 
				Console.WriteLine($"Get Players received from {_fromClient}");
				foreach (Player p in Player.Players)
				{
					Packet playerPacket = new Packet((int)ServerPackets.addPlayer);
					playerPacket.Write(p);
					ServerSend.SendTCPData(_fromClient, playerPacket);
				}
			}));
		}
		public static void AddPlayer(int _fromClient, Packet packet)
		{
			Application.Current.Dispatcher.Invoke(new Action(() => {
				Player p = packet.ReadPlayer();
				Console.WriteLine($"Add Player received from {_fromClient}; player: {p.Name}");
				Player.Players.Add(p);
				ServerSend.SendTCPDataToAll(_fromClient, packet);
			}));
		}
		public static void UpdatePlayer(int _fromClient, Packet packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					Console.WriteLine($"Update Player received from {_fromClient}; player: {Player.Players[packet.ReadInt(false)].Name}");
					int index = packet.ReadInt();
					Player p = packet.ReadPlayer();
					Player.Players[index].Name = p.Name;
					Player.Players[index].Rank = p.Rank;
					ServerSend.SendTCPDataToAll(_fromClient, packet);
				}));
			} catch(IndexOutOfRangeException e)
			{
				Console.WriteLine(e);
			}
			
		}
		public static void RemovePlayer(int _fromClient, Packet packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					Console.WriteLine($"Remove Player received from {_fromClient}; player: {Player.Players[packet.ReadInt(false)].Name}");
					Player.Players[packet.ReadInt(false)].Name = "";
					Player.Players[packet.ReadInt(false)].Points = 0;
					Player.Players[packet.ReadInt(false)].Shots.Clear();
					MainViewModel.MatchVM.CurrentView = null;
					Player.Players.RemoveAt(packet.ReadInt());
					ServerSend.SendTCPDataToAll(_fromClient, packet);
				}));
			} catch(IndexOutOfRangeException e)
			{
				Console.WriteLine(e);
			}
			
		}

		internal static void SaveGame(int _fromCient, Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					GameSaver.SaveGame(MainViewModel.MatchVM.PlayerBlue, MainViewModel.MatchVM.PlayerOrange);
					MainViewModel.MatchVM.ClearPlayerBlue.Execute(null);
					MainViewModel.MatchVM.ClearPlayerOrange.Execute(null);
					MainViewModel.MatchVM.GameCount++;
					ServerSend.SendTCPDataToAll(_fromCient, _packet);
				}));
			}
			catch
			{

			}
		}

		internal static void SetLua(int _fromCient, Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					Player.ValidationLua = _packet.ReadString();
					ServerSend.SendTCPDataToAll(_fromCient,_packet);
				}));
			}
			catch
			{

			}
		}

		public static void RemoveShot(int _fromClient, Packet packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					int index = packet.ReadInt();
					int shotIndex = packet.ReadInt();
					Console.WriteLine($"Remove Shot received from {_fromClient}; player: {Player.Players[index].Name}; shot: {Player.Players[index].Shots[index].Name}");
					Player.Players[index].Shots.RemoveAt(shotIndex);
					ServerSend.SendTCPDataToAll(_fromClient, packet);
				}));
			} catch(IndexOutOfRangeException e)
			{
				Console.WriteLine(e);
			}
			
		}
		public static void AddShot(int _fromClient, Packet packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					int index = packet.ReadInt();
					Console.WriteLine($"Add Shot received from {_fromClient}; player: {Player.Players[index].Name}");
					Player.Players[index].Shots.Add(packet.ReadShot());
					ServerSend.SendTCPDataToAll(_fromClient, packet);
				}));
			} catch(IndexOutOfRangeException e)
			{
				Console.WriteLine(e);
			}
			
		}
		public static void SetXml(int _fromClient, Packet packet) {
			Application.Current.Dispatcher.Invoke(new Action(() => {
				Console.WriteLine($"Set Xml received from {_fromClient}");
				ShotlistLoader.LoadXmlFromString(packet.ReadString());
				ServerSend.SendTCPDataToAll(_fromClient, packet);
			}));
		}
		public static void ResetPlayer(int _fromClient, Packet packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					int index = packet.ReadInt();
					Console.WriteLine($"Reset Player received from {_fromClient}; player: {Player.Players[index].Name}");
					Player.Players[index].Shots.Clear();
					ServerSend.SendTCPDataToAll(_fromClient, packet);
				}));
			} catch(IndexOutOfRangeException e)
			{
				Console.WriteLine(e);
			}
		}
		public static void SetPlayerColor(int _fromClient, Packet packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					int index = packet.ReadInt();
					int color = packet.ReadInt();
					switch(color)
					{
						case Constants.BlueColorIndex:
							MainViewModel.MatchVM.PlayerBlue = Player.Players[index]; break;
						case Constants.OrangeColorIndex:
							MainViewModel.MatchVM.PlayerOrange = Player.Players[index]; break;
					}
					Console.WriteLine($"Set Color received from {_fromClient}; player: {Player.Players[index].Name}; colorIndex: {color}");
					ServerSend.SendTCPDataToAll(_fromClient, packet);
				}));
			}catch(Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
