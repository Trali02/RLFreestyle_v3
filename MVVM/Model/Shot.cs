using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.Model
{
	public class Shot
	{
		public string Name { get; set; }
		public double Value { get; set; }
		public Shot(string Name, double Value)
		{
			this.Name = Name;
			this.Value = Value;
		}
		public Shot(Shot _shot)
		{
			this.Name = new string(_shot.Name);
			this.Value = _shot.Value;
		}
		public override string ToString()
		{
			return Name;
		}
	}
}
