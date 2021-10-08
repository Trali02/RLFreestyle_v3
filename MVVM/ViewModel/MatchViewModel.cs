using RLFreestyle_v3.Core;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using RLFreestyle_v3.MVVM.Model;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RLFreestyle_v3.MVVM.ViewModel
{
	class MatchViewModel : ObservableObject
	{
		private object currentView;
		private Player playerBlue;
		private Player playerOrange;
		public ObservableCollection<Player> Players { get; set; }

		public Player SelectedPlayer { get; set; }
		public Shot SelectedBlueShot { get; set; }
		public Shot SelectedOrangeShot { get; set; }
		public Command AddPlayer { get; set; }
		public Command RemovePlayer { get; set; }
		public Command AddShotBlue { get; set; }
		public Command AddShotOrange { get; set; }
		public Command RemoveShotOrange { get; set; }
		public Command RemoveShotBlue { get; set; }
		public Command ClearPlayerBlue { get; set; }
		public Command ClearPlayerOrange { get; set; }
		public Command SaveGame { get; set; }


		public Player PlayerBlue 
		{ 
			get
			{
				return playerBlue;
			}
			set
			{
				playerBlue = value;
				playerBlue.setStreamOutput();
				OnPropertyChanged();
			}
		}
		public Player PlayerOrange
		{ 
			get
			{
				return playerOrange;
			}
			set
			{
				playerOrange = value;
				playerOrange.setStreamOutput();
				OnPropertyChanged();
			}
		}

		public object CurrentView {
			get
			{
				return currentView;
			}
			set
			{
				currentView = value;
				OnPropertyChanged();
			}
		}

		public int GameCount
		{
			get
			{
				return GameSaver.GameCount;
			}
			set
			{
				GameSaver.GameCount = value;
				OnPropertyChanged();
			}
		}

		public AddPlayerViewModel AddPlayerVM { get; set; }
		public AddShotViewModel AddShotVM { get; set; }
		public MatchViewModel() 
		{
			Players = Player.Players;
			
			CurrentView = null;

			AddPlayer = new Command(o =>
			{
				AddPlayerVM = new AddPlayerViewModel();
				AddPlayerVM.CloseView = new Command(o =>
				{
					CurrentView = null;
				});
				AddPlayerVM.AddPlayer = new Command(o =>
				{
					string Name = AddPlayerVM.Name;
					Rank rank = AddPlayerVM.SelectedRank;
					Player newPlayer = new Player(Name, rank);
					CurrentView = null;
					if(Server.ServerStart.isRunning)
					{
						using (Packet _packet = new Packet((int)ServerPackets.addPlayer)) { 
							_packet.Write(newPlayer);
							_packet.SetBytes();
							_packet.SkipId();
							Server.ServerHandle.AddPlayer(-1,_packet);
						}
					} 
					else 
					{
						if(Client.ClientStart.isRunning)
						{
							using(Packet _packet = new Packet((int)ClientPackets.addPlayer))
							{
								_packet.Write(newPlayer);
								Client.ClientSend.SendTCPData(_packet);
							}
						}
						Players.Add(newPlayer);
					}
				});
				CurrentView = AddPlayerVM;
			});

			AddShotBlue = new Command(o => {
				AddShotVM = new AddShotViewModel();
				AddShotVM.CloseView = new Command(o =>
				{
					CurrentView = null;
				});
				AddShotVM.AddShot = new Command(o =>
				{
					Shot s = AddShotVM.generateShot();
					s.Value *= PlayerBlue.pointCalculator(s);
					CurrentView = null;
					if (Server.ServerStart.isRunning)
					{
						using (Packet _packet = new Packet((int)ServerPackets.addShot))
						{
							_packet.Write(Player.Players.IndexOf(PlayerBlue));
							_packet.Write(s);
							_packet.SetBytes();
							_packet.SkipId();
							Server.ServerHandle.AddShot(-1, _packet);
						}
					}
					else
					{
						if (Client.ClientStart.isRunning)
						{
							using (Packet _packet = new Packet((int)ClientPackets.addShot))
							{
								_packet.Write(Player.Players.IndexOf(PlayerBlue));
								_packet.Write(s);
								Client.ClientSend.SendTCPData(_packet);
							}
						}
						PlayerBlue.Shots.Add(s);
					}
				});
				CurrentView = AddShotVM;
			});

			AddShotOrange = new Command(o => {
				AddShotVM = new AddShotViewModel();
				AddShotVM.CloseView = new Command(o =>
				{
					CurrentView = null;
				});
				AddShotVM.AddShot = new Command(o => 
				{
					Shot s = AddShotVM.generateShot();
					s.Value *= PlayerOrange.pointCalculator(s);
					CurrentView = null;
					if (Server.ServerStart.isRunning)
					{
						using (Packet _packet = new Packet((int)ServerPackets.addShot))
						{
							_packet.Write(Player.Players.IndexOf(PlayerOrange));
							_packet.Write(s);
							_packet.SetBytes();
							_packet.SkipId();
							Server.ServerHandle.AddShot(-1, _packet);
						}
					}
					else
					{
						if (Client.ClientStart.isRunning)
						{
							using (Packet _packet = new Packet((int)ClientPackets.addShot))
							{
								_packet.Write(Player.Players.IndexOf(PlayerOrange));
								_packet.Write(s);
								Client.ClientSend.SendTCPData(_packet);
							}
						}
						PlayerOrange.Shots.Add(s);
					}
				});
				CurrentView = AddShotVM;
			});

			RemoveShotBlue = new Command(o =>
			{
				if(Server.ServerStart.isRunning)
				{
					using(Packet _packet = new Packet((int)ServerPackets.removeShot))
					{
						_packet.Write(Player.Players.IndexOf(PlayerBlue));
						_packet.Write(PlayerBlue.Shots.IndexOf(SelectedBlueShot));
						_packet.SetBytes();
						_packet.SkipId();
						Server.ServerHandle.RemoveShot(-1, _packet);
					}
				} 
				else
				{
					if (Client.ClientStart.isRunning)
					{
						using (Packet _packet = new Packet((int)ClientPackets.removeShot))
						{
							_packet.Write(Player.Players.IndexOf(PlayerBlue));
							_packet.Write(PlayerBlue.Shots.IndexOf(SelectedBlueShot));
							Client.ClientSend.SendTCPData(_packet);
						}
					}
					PlayerBlue.Shots.Remove(SelectedBlueShot);
				}
			});

			RemoveShotOrange = new Command(o =>
			{
				if (Server.ServerStart.isRunning)
				{
					using (Packet _packet = new Packet((int)ServerPackets.removeShot))
					{
						_packet.Write(Player.Players.IndexOf(PlayerOrange));
						_packet.Write(PlayerOrange.Shots.IndexOf(SelectedOrangeShot));
						_packet.SetBytes();
						_packet.SkipId();
						Server.ServerHandle.RemoveShot(-1, _packet);
					}
				}
				else
				{
					if (Client.ClientStart.isRunning)
					{
						using (Packet _packet = new Packet((int)ClientPackets.addShot))
						{
							_packet.Write(Player.Players.IndexOf(PlayerOrange));
							_packet.Write(PlayerOrange.Shots.IndexOf(SelectedOrangeShot));
							Client.ClientSend.SendTCPData(_packet);
						}
					}
					PlayerOrange.Shots.Remove(SelectedOrangeShot);
				}
			});

			ClearPlayerBlue = new Command(o =>
			{
				if (Server.ServerStart.isRunning)
				{
					using (Packet _packet = new Packet((int)ServerPackets.resetPlayer))
					{
						_packet.Write(Player.Players.IndexOf(PlayerBlue));
						_packet.SetBytes();
						_packet.SkipId();
						Server.ServerHandle.ResetPlayer(-1, _packet);
					}
				}
				else
				{
					if (Client.ClientStart.isRunning)
					{
						using (Packet _packet = new Packet((int)ClientPackets.resetPlayer))
						{
							_packet.Write(Player.Players.IndexOf(PlayerBlue));
							Client.ClientSend.SendTCPData(_packet);
						}
					}
					PlayerBlue.Shots.Clear();
				}
			});
			
			ClearPlayerOrange = new Command(o =>
			{
				if (Server.ServerStart.isRunning)
				{
					using (Packet _packet = new Packet((int)ServerPackets.resetPlayer))
					{
						_packet.Write(Player.Players.IndexOf(PlayerOrange));
						_packet.SetBytes();
						_packet.SkipId();
						Server.ServerHandle.ResetPlayer(-1, _packet);
					}
				}
				else
				{
					if (Client.ClientStart.isRunning)
					{
						using (Packet _packet = new Packet((int)ClientPackets.resetPlayer))
						{
							_packet.Write(Player.Players.IndexOf(PlayerOrange));
							Client.ClientSend.SendTCPData(_packet);
						}
					}
					PlayerOrange.Shots.Clear();
				}
			});

			RemovePlayer = new Command(o =>
			{
				SelectedPlayer.Shots.Clear();
				SelectedPlayer.Points = 0;
				SelectedPlayer.Name = "";
				if (Server.ServerStart.isRunning)
				{
					using (Packet _packet = new Packet((int)ServerPackets.removePlayer))
					{
						_packet.Write(Player.Players.IndexOf(SelectedPlayer));
						_packet.SetBytes();
						_packet.SkipId();
						Server.ServerHandle.RemovePlayer(-1, _packet);
					}
					CurrentView = null;
				}
				else
				{
					if (Client.ClientStart.isRunning)
					{
						using (Packet _packet = new Packet((int)ClientPackets.removePlayer))
						{
							_packet.Write(Player.Players.IndexOf(SelectedPlayer));
							Client.ClientSend.SendTCPData(_packet);
						}
					}
					CurrentView = null;
					Players.Remove(SelectedPlayer);
				}
				SelectedPlayer = null;
			});

			SaveGame = new Command(o =>
			{
				try
				{
					var confirmResult = MessageBox.Show("Saving Game, The Players will be cleared!!! Are you sure You want to proceed?","Confirm Save!!",MessageBoxButton.YesNo);
					if(confirmResult == MessageBoxResult.Yes)
					{
						GameSaver.SaveGame(PlayerBlue, PlayerOrange);
						ClearPlayerBlue.Execute(null);
						ClearPlayerOrange.Execute(null);
						GameCount++;

						if(Server.ServerStart.isRunning)
						{
							using(Packet _packet = new Packet((int)ServerPackets.saveGame))
							{
								Server.ServerSend.SendTCPDataToAll(_packet);
							}
						} else if(Client.ClientStart.isRunning)
						{
							using (Packet _packet = new Packet((int)ClientPackets.saveGame))
							{
								Client.ClientSend.SendTCPData(_packet);
							}
						}
					}
				}
				catch
				{

				}
			});
		}

		//drag and drop code:
		public void PlayerList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			ListBox source = (ListBox)sender;
			object data = GetDataFromListBox(source, e.GetPosition(source));

			if (data != null)
			{
				DragDrop.DoDragDrop(source, data, DragDropEffects.Move);
			}
		}
		private static object GetDataFromListBox(ListBox source, Point point)
		{
			UIElement element = source.InputHitTest(point) as UIElement;
			if (element != null)
			{
				object data = DependencyProperty.UnsetValue;
				while (data == DependencyProperty.UnsetValue)
				{
					data = source.ItemContainerGenerator.ItemFromContainer(element);

					if (data == DependencyProperty.UnsetValue)
					{
						element = VisualTreeHelper.GetParent(element) as UIElement;
					}

					if (element == source)
					{
						return null;
					}
				}
				if (data != DependencyProperty.UnsetValue)
				{
					return data;
				}
			}
			return null;
		}
		public void Blue_Drop(object sender, DragEventArgs e)
		{
			Player data = (Player)e.Data.GetData(typeof(Player));
			if (Server.ServerStart.isRunning)
			{
				using (Packet _packet = new Packet((int)ServerPackets.setPlayerColor))
				{
					_packet.Write(Player.Players.IndexOf(data));
					_packet.Write(Constants.BlueColorIndex);
					_packet.SetBytes();
					_packet.SkipId();
					Server.ServerHandle.SetPlayerColor(-1, _packet);
				}
			}
			else
			{
				if (Client.ClientStart.isRunning)
				{
					using (Packet _packet = new Packet((int)ClientPackets.setPlayerColor))
					{
						_packet.Write(Player.Players.IndexOf(data));
						_packet.Write(Constants.BlueColorIndex);
						Client.ClientSend.SendTCPData(_packet);
					}
				}
				PlayerBlue = data;
			}
		}
		public void Orange_Drop(object sender, DragEventArgs e)
		{
			Player data = (Player)e.Data.GetData(typeof(Player));
			if (Server.ServerStart.isRunning)
			{
				using (Packet _packet = new Packet((int)ServerPackets.setPlayerColor))
				{
					_packet.Write(Player.Players.IndexOf(data));
					_packet.Write(Constants.OrangeColorIndex);
					_packet.SetBytes();
					_packet.SkipId();
					Server.ServerHandle.SetPlayerColor(-1, _packet);
				}
			}
			else
			{
				if (Client.ClientStart.isRunning)
				{
					using (Packet _packet = new Packet((int)ClientPackets.setPlayerColor))
					{
						_packet.Write(Player.Players.IndexOf(data));
						_packet.Write(Constants.OrangeColorIndex);
						Client.ClientSend.SendTCPData(_packet);
					}
				}
				PlayerOrange = data;
			}
		}
	}
}
