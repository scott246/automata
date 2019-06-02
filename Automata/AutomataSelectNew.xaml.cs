using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Automata
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class AutomataSelectNew : Window
	{
		public string automataName = "";
		public string automataDesc = "";
		readonly string[] existingAutomataNames;
		
		public AutomataSelectNew(string[] vs)
		{
			InitializeComponent();
			existingAutomataNames = vs;
			NameCharRemaining.Content = Operations.AutomataMaxNameLength;
			DescCharRemaining.Content = Operations.AutomataMaxDescLength;
		}

		private void Create_Click(object sender, RoutedEventArgs e)
		{
			var name = NameBox.Text;
			var desc = DescriptionBox.Text;

			//ensure name exists
			if (name.Length == 0)
			{
				MessageBox.Show("Name cannot be blank.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			//ensure name can fit in db
			if (name.Length > Operations.AutomataMaxNameLength)
			{
				MessageBox.Show("Maximum name size is " + Operations.AutomataMaxNameLength + " characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			//ensure name contains only valid characters
			if (!(new Regex(@"^[a-zA-Z0-9-]*$").IsMatch(name)))
			{
				MessageBox.Show("Name must have only alphanumeric characters or a dash (-).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			//ensure description can fit in db
			if (desc.Length > Operations.AutomataMaxDescLength)
			{
				MessageBox.Show("Maximum description size is " + Operations.AutomataMaxDescLength + " characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			//ensure name is not a duplicate
			foreach (string existingName in existingAutomataNames)
			{
				if (existingName == name)
				{
					MessageBox.Show("Name must be unique.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
			}
			automataName = name;
			automataDesc = desc;
			Close();
		}

		private void OnNameBoxChanged(object sender, System.Windows.Input.KeyEventArgs e)
		{
			NameCharRemaining.Content = (Operations.AutomataMaxNameLength - Convert.ToInt32(NameBox.Text.Length)).ToString();
		}

		private void OnDescBoxChanged(object sender, System.Windows.Input.KeyEventArgs e)
		{
			DescCharRemaining.Content = (Operations.AutomataMaxDescLength - Convert.ToInt32(DescriptionBox.Text.Length)).ToString();
		}
	}
}
