using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace Automata
{
	public class AutomataData
	{
		public bool Enabled { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class AutomataSelect : Window
	{
		readonly string username = "";
		public AutomataSelect(string user)
		{
			InitializeComponent();
			UsernameDisplay.Content = user;
			username = user;
			PopulateDataGrid();
		}

		//TODO: delete below constructor and change App.xaml to redirect to login screen
		public AutomataSelect()
		{
			InitializeComponent();
			UsernameDisplay.Content = "test1";
			username = "test1";
			PopulateDataGrid();
		}

		private void PopulateDataGrid()
		{
			var automataList = DBOps.Select("SELECT automata_name, automata_desc, enabled FROM automata;");
			if (automataList == null)
			{
				System.Windows.Forms.MessageBox.Show("Error retrieving automata.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			List<AutomataData> automata = new List<AutomataData>();
			for (var i = 0; i < automataList.Count; i++)
			{
				automata.Add(new AutomataData()
				{
					Enabled = automataList[i][2],
					Name = automataList[i][0],
					Description = automataList[i][1],
				});
			}
			AutomataGrid.ItemsSource = automata;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			new AutomataEdit(username, "test project title").Show();
			Close();
		}

		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			AutomataSelectNew asn = new AutomataSelectNew();
			asn.ShowDialog();
			string newAutomataName = asn.automataName;
			string newAutomataDesc = asn.automataDesc;
			var result = DBOps.Insert(
				string.Format("INSERT INTO automata(automata_name, automata_desc, enabled) VALUES (\"{0}\", \"{1}\", true);", 
				newAutomataName, 
				newAutomataDesc));
			if (result != 0)
			{
				System.Windows.Forms.MessageBox.Show("Error creating automata.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
			new AutomataEdit(username, newAutomataName).Show();
			Close();
		}

		private void OpenButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ToggleButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			new Login().Show();
			Close();
		}
	}
}
