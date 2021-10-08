using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.Model
{
	public class Trick
	{
		public string Name { get; set; }
		public double Value { get; set; }
		public static ObservableCollection<Trick> Available { get; set; }
		public Trick(string Name,double Value)
		{
			this.Name = Name;
			this.Value = Value;
			//Available.Add(this);
		}
		public override string ToString()
		{
			return Name;
		}
	}
}
