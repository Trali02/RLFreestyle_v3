using RLFreestyle_v3.Core;
using RLFreestyle_v3.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.ViewModel
{
	class ClientViewModel : ObservableObject
	{
		private StringWriterExt _sw;
		public string ConsoleOut { get; set; }
		public Command CloseCommand { get; set; }

		public ClientViewModel()
		{
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
