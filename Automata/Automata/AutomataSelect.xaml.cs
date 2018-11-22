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
	public partial class AutomataSelect : Window
	{
		string username = "";
		public AutomataSelect(string user)
		{
			InitializeComponent();
			test.Content = "Welcome, " + user;
			username = user;
		}

		private void Logout(object sender, RoutedEventArgs e)
		{
			int code = DBOps.Update(
				string.Format(
				"UPDATE users SET persist=false WHERE uname='{0}';",
				username));
			if (code == 0)
			{
				new Login().Show();
				Close();
			}
			else
			{
				Console.WriteLine("Error logging out. (" + code + ")");
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Automata.Actions.FileCreate.CreateFile(@"C:\Users\ndsco\Test Folder\TestFile.txt");
		}
	}
}
