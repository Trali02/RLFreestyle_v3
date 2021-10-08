using RLFreestyle_v3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.Model
{
	class Game : ObservableObject
	{
		public Player Blue;
		public Player Orange;
		public int Id;

		public Game(Player blue,Player orange,int id)
		{
			Id = id;
			Blue = new Player(blue);
			Orange = new Player(orange);
		}

		public Player Winner()
		{
			if (Blue.Points > Orange.Points) return Blue;
			else return Orange;
		}

		public void ClearShots()
		{
			Blue.Shots.Clear();
			Orange.Shots.Clear();
		}
	}
}
