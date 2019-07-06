using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Automata.Screens
{
	/// <summary>
	/// Interaction logic for AutomataEdit.xaml
	/// </summary>
	public partial class AutomataEdit : UserControl
	{
		public string script = "";
		public event EventHandler OnReturnToSelect;
		public bool enabled = true;

		public AutomataEdit(string title, bool isEnabled)
		{
			InitializeComponent();
			AutomataNameDisplay.Text = title;
			enabled = isEnabled;
			SaveStatus.Content = "Saved";
			EnabledStatus.Content = stringEnabled(isEnabled);
			if (isEnabled)
			{
				Run();
			}
			else
			{
				Stop();
			}
		}

		//TODO: remove after testing edit screen
		public AutomataEdit()
		{
			InitializeComponent();
			AutomataNameDisplay.Text = "test-automata";
			enabled = true;
			SaveStatus.Content = "Saved";
			EnabledStatus.Content = stringEnabled(true);
		}

		public string stringEnabled(bool isEnabled)
		{
			return isEnabled == true ? "Enabled" : "Disabled";
		}

		public void Run()
		{
			Message m = new Message("This will enable the automata. Are you sure?", "Running", false, true);
			m.ShowDialog();
			int response = m.response;
			if (response == 1)
			{
				ScriptTextBox.IsEnabled = false;
				EnabledStatus.Content = stringEnabled(true);
				//update database, run automata
				RunButton.Visibility = Visibility.Hidden;
				EditButton.Visibility = Visibility.Visible;
				SaveButton.Visibility = Visibility.Hidden;
			}
		}

		public void Stop()
		{
			Message m = new Message("This will disable the automata. Are you sure?", "Editing", false, true);
			m.ShowDialog();
			int response = m.response;
			if (response == 1)
			{
				ScriptTextBox.IsEnabled = true;
				EnabledStatus.Content = stringEnabled(false);
				//update database, stop automata
				RunButton.Visibility = Visibility.Visible;
				EditButton.Visibility = Visibility.Hidden;
				SaveButton.Visibility = Visibility.Visible;
			}
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
			SaveStatus.Content = "Changed";
		}

		private void ReturnButton_Click(object sender, RoutedEventArgs e)
		{
			OnReturnToSelect?.Invoke(this, e);
		}

		private void RunButton_Click(object sender, RoutedEventArgs e)
		{
			//show stop button, run automata, disable code box
			Run();
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			Stop();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			//save the automata and name
			SaveStatus.Content = "Saved";
		}

		private void HelpButton_Click(object sender, RoutedEventArgs e)
		{
			new Help().ShowDialog();
		}

		private void AutomataNameDisplay_KeyUp(object sender, KeyEventArgs e)
		{
			SaveStatus.Content = "Changed";
		}
	}
}