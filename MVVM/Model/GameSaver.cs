using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RLFreestyle_v3.MVVM.Model
{
	class GameSaver
	{
		public static int GameCount { get; set; }
		public static string SaveFile { get; set; }
		public static ObservableCollection<Game> History { get; set; }

		/// <summary>
		/// Saves the game into an xml file
		/// </summary>
		/// <param name="blue">Player blue</param>
		/// <param name="orange">Player orange</param>
		/// <returns>returns true if the save was successful</returns>
		public static bool SaveGame(Player blue,Player orange)
		{
			if (History == null) History = new ObservableCollection<Game>();
			if (!File.Exists(SaveFile)) return false ;

			History.Add(new Game(blue, orange,GameCount));
			XElement xml = new XElement("Games", History.Select(x => new XElement("Game",new XAttribute("Id",x.Id),new XElement("Blue",new XAttribute("Name",x.Blue.Name),new XAttribute("Points",x.Blue.Points),new XAttribute("Rank",x.Blue.Rank.Name)),new XElement("Orange",new XAttribute("Name",x.Orange.Name),new XAttribute("Points",x.Orange.Points),new XAttribute("Rank",x.Orange.Rank.Name)))));
			xml.Save(SaveFile);
			return true;
		}
	}
}
