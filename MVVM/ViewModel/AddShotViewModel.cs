using RLFreestyle_v3.Core;
using RLFreestyle_v3.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RLFreestyle_v3.MVVM.ViewModel
{
	class AddShotViewModel
	{
		public Command CloseView { get; set; }
		public Command AddShot { get; set; }
		public Command RemoveTrick { get; set; }
		public Trick SelectedTrick { get; set; }
		public Trick SelectedTrickFromList { get; set; }
		public ObservableCollection<Trick> TricksAvailable { get; set; }
		public ObservableCollection<Trick> TricksAdded { get; set; }
		public AddShotViewModel()
		{
			TricksAvailable = Trick.Available;
			TricksAdded = new ObservableCollection<Trick>();
			RemoveTrick = new Command(o =>
			{
				TricksAdded.Remove(SelectedTrick);
			});
		}
		public Shot generateShot()
		{
			if (TricksAdded.Count <= 0) return null;
			string name = "";
			double value = 0;
			foreach(Trick t in TricksAdded)
			{
				name += t.Name + " ";
				value += t.Value;
			}
			return new Shot(name, value);
		}
		//drag and drop code:
		public void TricksList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			ListBox source = (ListBox)sender;
			object data = GetDataFromListBox(source, e.GetPosition(source));

			if (data != null)
			{
				DragDrop.DoDragDrop(source, data, DragDropEffects.Move);
			}
		}
		private static object GetDataFromListBox(ListBox source, Point point)
		{
			UIElement element = source.InputHitTest(point) as UIElement;
			if (element != null)
			{
				object data = DependencyProperty.UnsetValue;
				while (data == DependencyProperty.UnsetValue)
				{
					data = source.ItemContainerGenerator.ItemFromContainer(element);

					if (data == DependencyProperty.UnsetValue)
					{
						element = VisualTreeHelper.GetParent(element) as UIElement;
					}

					if (element == source)
					{
						return null;
					}
				}
				if (data != DependencyProperty.UnsetValue)
				{
					return data;
				}
			}
			return null;
		}
		public void Trick_Drop(object sender, DragEventArgs e)
		{
			Trick data = (Trick)e.Data.GetData(typeof(Trick));
			TricksAdded.Add(data);
		}
		public void Trick_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				TricksAdded.Add(SelectedTrickFromList);
			}
			if(e.Key == Key.Delete)
			{
				TricksAdded.RemoveAt(TricksAdded.Count - 1);
			}
		}
	}
}
