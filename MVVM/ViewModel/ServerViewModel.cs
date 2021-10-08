using RLFreestyle_v3.Core;
using RLFreestyle_v3.MVVM.Client;
using RLFreestyle_v3.MVVM.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.ViewModel
{
	class ServerViewModel : ObservableObject
	{
		public string IP { get; set; }
		public string Port { get; set; }
		public HostViewModel HostVM { get; set; }
		public ClientViewModel ClientVM { get; set; }
		public Command CloseCommand { get; set; }
		public Command HostViewCommand { get; set; }
		public Command ClientViewCommand { get; set; }
		private object currentView;
		public object CurrentView
		{
			get { return currentView; }
			set
			{
				currentView = value;
				OnPropertyChanged();
			}
		}

		public ServerViewModel()
		{
			HostVM = new HostViewModel();
			ClientVM = new ClientViewModel();
			currentView = null;

			CloseCommand = new Command(o =>
			{
				CurrentView = null;
				if(ServerStart.isRunning) ServerStart.Stop();
				if(ClientStart.isRunning) ClientStart.Stop();
			});
			HostViewCommand = new Command(o => 
			{
				CurrentView = HostVM;
				HostVM.SetConsole();
				ServerStart.Start();
			});
			ClientViewCommand = new Command(o =>
			{
				CurrentView = ClientVM;
				ClientVM.SetConsole();
				ClientStart.Start(IP,System.Convert.ToInt32(Port));
			});

			HostVM.CloseCommand = CloseCommand;
			ClientVM.CloseCommand = CloseCommand;
		}
	}
}
