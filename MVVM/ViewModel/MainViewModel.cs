using RLFreestyle_v3.Core;
using RLFreestyle_v3.MVVM.Client;
using RLFreestyle_v3.MVVM.Model;
using RLFreestyle_v3.MVVM.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RLFreestyle_v3.MVVM.ViewModel
{
	class MainViewModel : ObservableObject
	{
		private bool enabled;
		public bool Enabled { 
			get {
				return enabled;
			} 
			set { 
				enabled = value;
				OnPropertyChanged();
			} 
		}
		private ObservableCollection<Trick> TrickList { get; set; }
		private ObservableCollection<Rank> RankList { get; set; }
		public Command MatchViewCommand { get; set; }
		public Command ServerViewCommand { get; set; }
		public Command FileViewCommand { get; set; }
		public Command StreamViewCommand { get; set; }
		public Command TurneyViewCommand { get; set; }
		public static MatchViewModel MatchVM { get; set; }
		public static ServerViewModel ServerVM { get; set; }
		public static FileViewModel FileVM { get; set; }
		public static StreamViewModel StreamVM { get; set; }
		public static TurneyViewModel TurneyVM { get; set; }
		private object currentView;

		public object CurrentView
		{
			get { return currentView; }
			set { 
				currentView = value;
				OnPropertyChanged();
			}
		}

		public MainViewModel()
		{
			MatchVM = new MatchViewModel();
			ServerVM = new ServerViewModel();
			FileVM = new FileViewModel();
			StreamVM = new StreamViewModel();
			TurneyVM = new TurneyViewModel();

			TrickList = Trick.Available;
			RankList = Rank.Ranks;

			Enabled = false;

			TrickList.CollectionChanged += OnCollectionChanged;
			RankList.CollectionChanged += OnCollectionChanged;

			try
			{
				StreamVM.BluePointsPath = ReadSetting("BluePointsPath");
				StreamVM.OrangePointsPath = ReadSetting("OrangePointsPath");
				FileVM.LuaPath = ReadSetting("LuaPath");
				FileVM.LuaScript = File.ReadAllText(ReadSetting("LuaPath"));
				FileVM.ShotlistPath = ReadSetting("XmlPath");
			}
			catch
			{

			}
			


			CurrentView = FileVM;

			MatchViewCommand = new Command(o =>
			{
				CurrentView = MatchVM;
			});
			ServerViewCommand = new Command(o =>
			{
				CurrentView = ServerVM;
			});
			FileViewCommand = new Command(o =>
			{
				CurrentView = FileVM;
			});
			StreamViewCommand = new Command(o =>
			{
				CurrentView = StreamVM;
			});
			TurneyViewCommand = new Command(o =>
			{
				CurrentView = TurneyVM;
			});
		}
		public static string ReadSetting(string key)
		{
			try
			{
				var appSettings = ConfigurationManager.AppSettings;
				string result = appSettings[key] ?? "Not Found";
				return result;
			}
			catch (ConfigurationErrorsException)
			{
				Console.WriteLine("Error reading app settings");
			}
			return null;
		}

		public static void AddUpdateAppSettings(string key, string value)
		{
			try
			{
				var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				var settings = configFile.AppSettings.Settings;
				if (settings[key] == null)
				{
					settings.Add(key, value);
				}
				else
				{
					settings[key].Value = value;
				}
				configFile.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
			}
			catch (ConfigurationErrorsException)
			{
				Console.WriteLine("Error writing app settings");
			}
		}
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Enabled = TrickList.Count > 0 && RankList.Count > 0;
		}
		public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (ServerStart.isRunning) ServerStart.Stop();
			if (ClientStart.isRunning) ClientStart.Stop();
		}
	}
}
