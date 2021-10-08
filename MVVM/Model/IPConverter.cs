using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RLFreestyle_v3.MVVM.Model
{
	public class IPConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{

			if (!Regex.Match(System.Convert.ToString(values[0]), @"(?:\d{1,3}\.){3}\d{1,3}$").Success) return false;
			if (!String.IsNullOrWhiteSpace(System.Convert.ToString(values[1])))
			{
				if (System.Convert.ToInt32(values[1]) < 0) return false;
			}
			else return false;

			return true;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
