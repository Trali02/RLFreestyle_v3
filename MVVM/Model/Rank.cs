using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.Model
{
	public class Rank
	{
		public string Name { get; set; }
		public double Multiplier { get; set; }
		public static ObservableCollection<Rank> Ranks { get; set; }
		public Rank(string Name,double Multiplier)
		{
			this.Name = Name;
			this.Multiplier = Multiplier;
			//Ranks.Add(this);
		}
		public Rank(Rank _rank)
		{
			this.Name = new string(_rank.Name);
			this.Multiplier = _rank.Multiplier;
		}
		override public string ToString()
		{
			return Name;
		}
	}
}
