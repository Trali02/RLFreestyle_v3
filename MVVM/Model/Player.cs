using RLFreestyle_v3.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;
using RLFreestyle_v3.MVVM.ViewModel;
using System.IO;

namespace RLFreestyle_v3.MVVM.Model
{
	class Player : ObservableObject
	{
		private double points;
		private string name;

		public static ObservableCollection<Player> Players { get; set; }
		public ObservableCollection<Shot> Shots { get; set; }
		public Rank Rank { get; set; }
		public double Points 
		{
			get
			{
				return points;
			}
			set
			{
				points = value;
				setStreamOutput();
				OnPropertyChanged();
			}
		}
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
				OnPropertyChanged();
			}
		}
		public static string ValidationLua { get; set; }

		public Player(string name, Rank rank)
		{
			this.Name = name;
			this.Rank = rank;
			this.Shots = new ObservableCollection<Shot>();

			Shots.CollectionChanged += OnCollectionChanged;
		}

		public Player(Player _player)
		{
			this.Name = new string(_player.Name);
			this.Rank = new Rank(_player.Rank);
			this.Points = _player.Points;
			this.Shots = new ObservableCollection<Shot>();
			foreach(Shot shot in _player.Shots)
			{
				Shots.Add(new Shot(shot));
			}
		}
		public double pointCalculator(Shot s)
		{
			try
			{
				Lua lua = new Lua();

				int duplicates = 0;

				foreach(Shot shot in Shots)
				{
					if (shot.Name.Equals(s.Name)) duplicates++;
				}

				lua["ShotDuplicates"] = duplicates;
				lua["ShotValue"] = s.Value;
				lua["Multiplier"] = Rank.Multiplier;

				return (double)lua.DoString(ValidationLua)[0];
			} catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return Rank.Multiplier;
			}
		}

		public void setStreamOutput()
		{
			if(Equals(MainViewModel.MatchVM.PlayerBlue) && File.Exists(MainViewModel.StreamVM.BluePointsPath))
			{
				File.WriteAllText(MainViewModel.StreamVM.BluePointsPath, roundPoints(Points).ToString());
			}
			if (Equals(MainViewModel.MatchVM.PlayerOrange) && File.Exists(MainViewModel.StreamVM.OrangePointsPath))
			{
				File.WriteAllText(MainViewModel.StreamVM.OrangePointsPath, roundPoints(Points).ToString());
			}
		}

		private int roundPoints(double _points)
		{
			if(_points - ((int)_points) < 0.5)
			{
				return (int)_points;
			} 
			else
			{
				return (int)(_points + 1);
			}
		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Points = 0;
			foreach(Shot s in Shots)
			{
				Points += s.Value;
			}
		}
	}
}
