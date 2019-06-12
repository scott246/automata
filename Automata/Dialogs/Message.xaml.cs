using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Automata
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Message : Window
	{
		public int response = -1;
		
		public Message(string prompt, string title, bool cancel, bool no)
		{
			InitializeComponent();
			PromptText.Text = prompt;
			Title = title;
			if (!cancel) Cancel.Visibility = Visibility.Hidden;
			if (!no) No.Visibility = Visibility.Hidden;
			if (!cancel && !no) AffirmativeButtonText.Content = "OK";
		}

		private void No_Click(object sender, RoutedEventArgs e)
		{
			response = 0;
			Close();
		}

		private void Yes_Click(object sender, RoutedEventArgs e)
		{
			response = 1;
			Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
