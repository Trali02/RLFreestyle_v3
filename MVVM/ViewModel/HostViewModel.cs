using RLFreestyle_v3.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RLFreestyle_v3.MVVM.Server;
using RLFreestyle_v3.MVVM.Model;

namespace RLFreestyle_v3.MVVM.ViewModel
{
	class HostViewModel : ObservableObject
	{
		private StringWriterExt _sw;
		public string ConsoleOut { get; set; }
		
		public Command CloseCommand { get; set; }
		public HostViewModel()
		{
			//Console.SetError(_sw);
		}
		public void SetConsole()
		{
			_sw = new StringWriterExt(true);
			_sw.Flushed += (s, a) => {
				ConsoleOut = _sw.ToString();
				OnPropertyChanged("ConsoleOut");
			};
			Console.SetOut(_sw);
		}
	}
}
