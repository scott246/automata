using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
		string[] existingAutomataNames;
		
		public AutomataSelectNew(string[] vs)
		{
			InitializeComponent();
			existingAutomataNames = vs;
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
			//ensure name contains only valid characters
			if (!(new Regex(@"^[a-zA-Z0-9-]*$").IsMatch(name)))
			{
				MessageBox.Show("Name must have only alphanumeric characters or a dash (-).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
	}
}
