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

namespace Automata
{
	/// <summary>
	/// Interaction logic for AutomataEdit.xaml
	/// </summary>
	public partial class AutomataEdit : Window
	{
		string username = "";
		public AutomataEdit(string name, string title)
		{
			InitializeComponent();
			author.Content = name;
			project_name.Content = title;
			username = name;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			new AutomataSelect(username).Show();
			Close();
		}
	}
}
