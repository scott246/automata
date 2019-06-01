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
using System.Windows.Shapes;

namespace Automata
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class AutomataSelectNew : Window
	{
		public string automataName = "";
		public string automataDesc = "";
		public AutomataSelectNew()
		{
			InitializeComponent();
		}

		private void Create_Click(object sender, RoutedEventArgs e)
		{
			//TODO: name error checking - no duplicates, not blank
			automataName = NameBox.Text;
			automataDesc = DescriptionBox.Text;
			Close();
		}
	}
}
