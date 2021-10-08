using RLFreestyle_v3.MVVM.Model;
using RLFreestyle_v3.MVVM.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace RLFreestyle_v3
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			Rank.Ranks = new ObservableCollection<Rank>();
			Trick.Available = new ObservableCollection<Trick>();
			Player.Players = new ObservableCollection<Player>();
			InitializeComponent();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			((MainViewModel)this.DataContext).Window_Closing(sender, e);
		}
	}
}
