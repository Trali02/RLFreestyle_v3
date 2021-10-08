using Microsoft.Win32;
using RLFreestyle_v3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.ViewModel
{
	class StreamViewModel : ObservableObject
	{
		private string bluePointsPath;
		public string BluePointsPath
		{
			get
			{
				return bluePointsPath;
			}
			set
			{
				bluePointsPath = value;
				MainViewModel.AddUpdateAppSettings("BluePointsPath", bluePointsPath);
				OnPropertyChanged();
			}
		}
		private string orangePointsPath;
		public string OrangePointsPath
		{
			get
			{
				return orangePointsPath;
			}
			set
			{
				orangePointsPath = value;
				MainViewModel.AddUpdateAppSettings("OrangePointsPath", orangePointsPath);
				OnPropertyChanged();
			}
		}
		public Command OpenBluePoints { get; set; }
		public Command OpenOrangePoints { get; set; }

		public StreamViewModel()
		{
			OpenBluePoints = new Command(o =>
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "txt| *.txt";
				if(openFileDialog.ShowDialog() == true) BluePointsPath = openFileDialog.FileName;
			});
			OpenOrangePoints = new Command(o =>
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "txt| *.txt";
				if (openFileDialog.ShowDialog() == true) OrangePointsPath = openFileDialog.FileName;
			});
		}

	}
}
