using RLFreestyle_v3.MVVM.Model;
using RLFreestyle_v3.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RLFreestyle_v3.MVVM.View
{
	/// <summary>
	/// Interaction logic for MatchView.xaml
	/// </summary>
	public partial class MatchView : UserControl
	{
		public MatchView()
		{
			InitializeComponent();
		}

		private void PlayerList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			((MatchViewModel)this.DataContext).PlayerList_PreviewMouseLeftButtonDown(sender, e);
		}

		private void Blue_Drop(object sender, DragEventArgs e)
		{
			((MatchViewModel)this.DataContext).Blue_Drop(sender, e);
		}
		private void Orange_Drop(object sender, DragEventArgs e)
		{
			((MatchViewModel)this.DataContext).Orange_Drop(sender, e);
		}
	}
}
