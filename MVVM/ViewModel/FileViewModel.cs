using Microsoft.Win32;
using RLFreestyle_v3.Core;
using RLFreestyle_v3.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RLFreestyle_v3.MVVM.ViewModel
{
	class FileViewModel:ObservableObject
	{
		public ObservableCollection<Trick> Tricklist { get; set; }
		public ObservableCollection<Rank> Ranklist { get; set; }
		public Command OpenShotlist { get; set; }
		public Command OpenLua { get; set; }

		private string shotlistpath;
		public string ShotlistPath
		{
			get
			{
				return shotlistpath;
			}
			set
			{
				shotlistpath = value;
				MainViewModel.AddUpdateAppSettings("XmlPath", shotlistpath);
				ShotlistLoader.LoadXmlFromFile(shotlistpath);
				if(Client.ClientStart.isRunning)
				{
					using(Packet _packet = new Packet((int)ClientPackets.setXml))
					{
						_packet.Write(ShotlistLoader.getData());
						Client.ClientSend.SendTCPData(_packet);
					}
				}
				if (Server.ServerStart.isRunning)
				{
					using (Packet _packet = new Packet((int)ServerPackets.setXml))
					{
						_packet.Write(ShotlistLoader.getData());
						Server.ServerSend.SendTCPDataToAll(_packet);
					}
				}
				OnPropertyChanged();
			}
		}
		private string luapath;
		public string LuaPath
		{
			get
			{
				return luapath;
			}
			set
			{
				luapath = value;
				MainViewModel.AddUpdateAppSettings("LuaPath", luapath);
				OnPropertyChanged();
			}
		}
		private string luascript;
		public string LuaScript
		{
			get
			{
				return luascript;
			}
			set
			{
				luascript = value;
				Player.ValidationLua = luascript;
				if(Server.ServerStart.isRunning)
				{
					using(Packet _packet = new Packet((int)ServerPackets.setLua))
					{
						_packet.Write(luascript);
						Server.ServerSend.SendTCPDataToAll(_packet);
					}
				}
				if(Client.ClientStart.isRunning)
				{
					using (Packet _packet = new Packet((int)ClientPackets.setLua))
					{
						_packet.Write(luascript);
						Client.ClientSend.SendTCPData(_packet);
					}
				}
				OnPropertyChanged();
			}
		}

		public FileViewModel()
		{
			Tricklist = Trick.Available;
			Ranklist = Rank.Ranks;


			OpenShotlist = new Command(o =>
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "xml| *.xml";
				if (openFileDialog.ShowDialog() == true) ShotlistPath = openFileDialog.FileName;
			});
			OpenLua = new Command(o =>
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "lua| *.lua";
				if (openFileDialog.ShowDialog() == true)
				{
					LuaPath = openFileDialog.FileName;
					LuaScript = File.ReadAllText(LuaPath);
				}
			});
		}
	}
}
