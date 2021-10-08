using Microsoft.Win32;
using RLFreestyle_v3.Core;
using RLFreestyle_v3.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.ViewModel
{
	class TurneyViewModel : ObservableObject
	{
		public string SaveFilePath { 
			get
			{
				return GameSaver.SaveFile;
			}
			set
			{
				GameSaver.SaveFile = value;
				OnPropertyChanged("SaveFilePath");
			}
		}
		public Command OpenSaveFile { get; set; }

		public TurneyViewModel()
		{
			OpenSaveFile = new Command(o =>
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "xml| *.xml";
				if (openFileDialog.ShowDialog() == true) SaveFilePath = openFileDialog.FileName;
			});
			
		}
	}
}
