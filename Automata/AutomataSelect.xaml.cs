using System;
using System.Windows;

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
			test.Content = user;
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
			new AutomataEdit(username, "test project title").Show();
			Close();
		}
	}
}
