using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RLFreestyle_v3.MVVM.Model
{
	class ShotlistLoader
	{
		private static XElement xmlData;
		public static void LoadXmlFromFile(string filepath)
		{
			try
			{
				XElement xml = XElement.Load(filepath);
				ReadData(xml);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
		public static void LoadXmlFromString(string xmldata)
		{
			try
			{
				XElement xml = XElement.Parse(xmldata);
				ReadData(xml);
			}
			catch (Exception e) { 
				Console.WriteLine(e); 
			}
		}
		public static string getData()
		{
			if (xmlData != null)
			{
				//Console.WriteLine(xmlData.ToString());
				return xmlData.ToString();
			}
			return "";
		}
		private static void ReadData(XElement _xmldata)
		{
			xmlData = _xmldata;

			Trick.Available.Clear();
			Rank.Ranks.Clear();
			//xmldata = xmldata.Element("root");//might break
			XElement setups = _xmldata.Element("Setups");
			XElement tricks = _xmldata.Element("Tricks");
			XElement multipliers = _xmldata.Element("Multipliers");

			foreach(XElement x in setups.Elements("Setup"))
			{
				Trick t = new Trick(x.Attribute("name").Value,Double.Parse(x.Attribute("points").Value, System.Globalization.CultureInfo.InvariantCulture));
				Trick.Available.Add(t);
			}
			foreach(XElement x in tricks.Elements("Trick"))
			{
				Trick t = new Trick(x.Attribute("name").Value,Double.Parse(x.Attribute("points").Value, System.Globalization.CultureInfo.InvariantCulture));
				Trick.Available.Add(t);
			}
			foreach (XElement x in multipliers.Elements("Rank"))
			{
				Rank r = new Rank(x.Attribute("name").Value, Double.Parse(x.Attribute("value").Value, System.Globalization.CultureInfo.InvariantCulture));
				Rank.Ranks.Add(r);
			}
		}
	}
}
