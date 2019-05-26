using System;
using System.Windows;
using System.Windows.Input;

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

		int maxLC = 1;
		private void Script_textbox_KeyUp(object sender, KeyEventArgs e)
		{
			int linecount = script_textbox.GetLastVisibleLineIndex() + 1;
			if (linecount != maxLC)
			{
				line_number_textbox.Clear();
				for (int i = 1; i < linecount + 1; i++)
				{
					line_number_textbox.AppendText(Convert.ToString(i) + "\n");
				}
				maxLC = linecount;
			}
		}

		private void Help_Clicked(object sender, RoutedEventArgs e)
		{
			new AutomataHelp().Show();
		}
	}
}
