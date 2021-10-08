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
	/// Interaction logic for AddShotView.xaml
	/// </summary>
	public partial class AddShotView : UserControl
	{
		public AddShotView()
		{
			InitializeComponent();
		}

		private void TricksList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			((AddShotViewModel)this.DataContext).TricksList_PreviewMouseLeftButtonDown(sender, e);
		}
		private void Trick_Drop(object sender, DragEventArgs e)
		{
			((AddShotViewModel)this.DataContext).Trick_Drop(sender, e);
		}

		private void Trick_KeyDown(object sender, KeyEventArgs e)
		{
			((AddShotViewModel)this.DataContext).Trick_KeyDown(sender, e);
		}
	}
}
