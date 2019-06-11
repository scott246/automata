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
			AutomataNameDisplay.Content = title;
			username = name;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			
		}

		int maxLC = 1;
		private void Script_textbox_KeyUp(object sender, KeyEventArgs e)
		{
			int linecount = ScriptTextBox.GetLastVisibleLineIndex() + 1;
			if (linecount != maxLC)
			{
				LineNumberTextBox.Clear();
				for (int i = 1; i < linecount + 1; i++)
				{
					LineNumberTextBox.AppendText(Convert.ToString(i) + "\n");
				}
				maxLC = linecount;
			}
		}

		private void Help_Clicked(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/scott246/automata/wiki");
		}

		private void ReturnButton_Click(object sender, RoutedEventArgs e)
		{
			new AutomataSelect(username).Show();
			Close();
		}

		private void RunButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void HelpButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
