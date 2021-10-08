using RLFreestyle_v3.MVVM.Model;
using RLFreestyle_v3.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace RLFreestyle_v3.MVVM.Client
{
	class ClientHandle
	{
		public static void Welcome(Packet _packet)
		{
			string _msg = _packet.ReadString();
			int _myIndex = _packet.ReadInt();

			Console.WriteLine($"Message from server: {_msg}");
			Client.instance.myIndex = _myIndex;
			ClientSend.WelcomeReceived();
		}
		public static void GetPoints(Packet _packet) { }

		public static void AddPlayer(Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					Player p = _packet.ReadPlayer();
					Console.WriteLine($"Add Player received; player: {p.Name}");
					Player.Players.Add(p);
				}));
			}
			catch
			{

			}
		}

		public static void UpdatePlayer(Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					int index = _packet.ReadInt();
					Player p = _packet.ReadPlayer();
					Console.WriteLine($"Update Player received; player: {Player.Players[index].Name}");
					Player.Players[index].Name = p.Name;
					Player.Players[index].Rank = p.Rank;
				}));
			}
			catch
			{

			}

		}

		public static void RemovePlayer(Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					Console.WriteLine($"Remove Player received; player: {Player.Players[_packet.ReadInt(false)].Name}");
					Player.Players[_packet.ReadInt(false)].Name = "";
					Player.Players[_packet.ReadInt(false)].Points = 0;
					Player.Players[_packet.ReadInt(false)].Shots.Clear();
					MainViewModel.MatchVM.CurrentView = null;
					MainViewModel.MatchVM.CurrentView = null;
					Player.Players.RemoveAt(_packet.ReadInt());
				}));
			}
			catch
			{

			}
		}

		public static void RemoveShot(Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					Console.WriteLine($"Remove Shot received; player: {Player.Players[_packet.ReadInt(false)].Name}");
					Player.Players[_packet.ReadInt()].Shots.RemoveAt(_packet.ReadInt());
				}));
			}
			catch
			{

			}
		}

		public static void AddShot(Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					int index = _packet.ReadInt(); 
					Console.WriteLine($"Add Shot received; player: {Player.Players[index].Name}");
					Shot s = _packet.ReadShot();
					Player.Players[index].Shots.Add(s);
				}));
			}
			catch
			{

			}
		}

		public static void SetXml(Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => { 
					ShotlistLoader.LoadXmlFromString(_packet.ReadString());
					Console.WriteLine($"Set xml received");
				})); 
			}
			catch
			{

			}
		}

		public static void ResetPlayer(Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					int index = _packet.ReadInt();
					Console.WriteLine($"Reset Player received; player: {Player.Players[index].Name}");
					Player.Players[index].Shots.Clear();
				}));
				
			}
			catch
			{

			}
		}

		public static void SetPlayerColor(Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					int index = _packet.ReadInt();
					int color = _packet.ReadInt();
					switch (color)
					{
						case Constants.BlueColorIndex:
							MainViewModel.MatchVM.PlayerBlue = Player.Players[index]; break;
						case Constants.OrangeColorIndex:
							MainViewModel.MatchVM.PlayerOrange = Player.Players[index]; break;
					}
					Console.WriteLine($"Set Color received; player: {Player.Players[index].Name}; colorIndex: {color}");
				}));
			}
			catch
			{

			}
		}

		internal static void SetLua(Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					Console.WriteLine("received set Lua");
					Player.ValidationLua = _packet.ReadString();
				}));
			}
			catch
			{

			}
		}

		internal static void SaveGame(Packet _packet)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() => {
					Console.WriteLine("received save Game");
					GameSaver.SaveGame(MainViewModel.MatchVM.PlayerBlue, MainViewModel.MatchVM.PlayerOrange);
					MainViewModel.MatchVM.ClearPlayerBlue.Execute(null);
					MainViewModel.MatchVM.ClearPlayerOrange.Execute(null);
					MainViewModel.MatchVM.GameCount++;
				}));
			}
			catch
			{

			}
		}
	}
}
