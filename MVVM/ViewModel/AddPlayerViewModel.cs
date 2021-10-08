using RLFreestyle_v3.Core;
using RLFreestyle_v3.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.ViewModel
{
	class AddPlayerViewModel
	{
		public Command CloseView { get; set; }
		public Command AddPlayer { get; set; }
		public ObservableCollection<Rank> Ranks { get; set; }
		public Rank SelectedRank { get; set; }
		public string Name { get; set; } 
		public bool CanCreate { get; set; }


		public AddPlayerViewModel() {
			Ranks = Rank.Ranks;
		}
	}
}
