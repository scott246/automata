using System;
using System.Collections;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Automata.Screens
{
	/// <summary>
	/// Interaction logic for AutomataEdit.xaml
	/// </summary>
	public partial class AutomataEdit : UserControl
	{
		public string script = "";
		public event EventHandler OnReturnToSelect;
		string username = "";
		public AutomataEdit(string name, string title)
		{
			InitializeComponent();
			AutomataNameDisplay.Text = title;
			username = name;
		}

		//TODO: remove after testing edit screen
		public AutomataEdit()
		{
			InitializeComponent();
			AutomataNameDisplay.Text = "test-automata";
			username = "test1";
		}

		int maxLC = 1;
		private void Script_textbox_KeyUp(object sender, KeyEventArgs e)
		{
			script = ScriptTextBox.Text;
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
			Status.Content = "Changed";
		}

		private void ReturnButton_Click(object sender, RoutedEventArgs e)
		{
			OnReturnToSelect?.Invoke(this, e);
		}

		private void RunButton_Click(object sender, RoutedEventArgs e)
		{
			//show stop button, run automata, disable code box
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			//save the automata and name
			Status.Content = "Saved";
		}

		private void HelpButton_Click(object sender, RoutedEventArgs e)
		{
			new Help().ShowDialog();
		}

		private void AutomataNameDisplay_KeyUp(object sender, KeyEventArgs e)
		{
			Console.WriteLine("name changed to " + AutomataNameDisplay.Text);
			Status.Content = "Changed";
		}

		private void StopButton_Click(object sender, RoutedEventArgs e)
		{
			//show run button, terminate running automata
		}
	}
}