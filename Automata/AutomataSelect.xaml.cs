using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using CheckBox = System.Windows.Controls.CheckBox;

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
		readonly List<AutomataData> automataList = new List<AutomataData>();
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
			var automata = DBOps.Select("SELECT automata_name, automata_desc, enabled FROM automata;");
			if (automata == null)
			{
				System.Windows.Forms.MessageBox.Show("Error retrieving automata.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			for (var i = 0; i < automata.Count; i++)
			{
				automataList.Add(new AutomataData()
				{
					Enabled = automata[i][2],
					Name = automata[i][0],
					Description = automata[i][1],
				});
			}
			AutomataGrid.ItemsSource = automataList;
		}

		void OnRowSelect(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("row selected");
			//get row index
			DataGridRow r = (DataGridRow)sender;
			int rowindex = r.GetIndex();

			//change checkbox enabled status
			CheckBox c = AutomataGrid.Columns[0].GetCellContent(AutomataGrid.Items[rowindex]) as CheckBox;
			c.IsChecked = !c.IsChecked;

			//ensure text boxes cannot be changed
			AutomataGrid.Columns[1].GetCellContent(AutomataGrid.Items[rowindex]).IsEnabled = false;
			AutomataGrid.Columns[2].GetCellContent(AutomataGrid.Items[rowindex]).IsEnabled = false;

			//get name of changed status' automata
			TextBlock t = AutomataGrid.Columns[1].GetCellContent(AutomataGrid.Items[rowindex]) as TextBlock;

			//update DB
			var result = DBOps.Update(string.Format("UPDATE automata SET enabled={0} WHERE automata_name='{1}';", c.IsChecked, t.Text));
			if (result != 0)
			{
				System.Windows.Forms.MessageBox.Show("Error changing automata status (" + result + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			Dispatcher.BeginInvoke(new Action(() =>
			{
				AutomataGrid.UnselectAll();
			}));
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			new AutomataEdit(username, "test project title").Show();
			Close();
		}

		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			List<string> existingAutomataNames = new List<string>();
			foreach (AutomataData ad in automataList)
			{
				existingAutomataNames.Add(ad.Name);
			}
			AutomataSelectNew asn = new AutomataSelectNew(existingAutomataNames.ToArray());
			asn.ShowDialog();
			string newAutomataName = asn.automataName;
			string newAutomataDesc = asn.automataDesc;
			if (newAutomataName != "")
			{
				//TODO: fix autoincrement to use all numbers before using a new one
				var result = DBOps.Insert(
				string.Format("INSERT INTO automata(automata_name, automata_desc, enabled) VALUES (\"{0}\", \"{1}\", true);",
				newAutomataName,
				newAutomataDesc));
				if (result != 0)
				{
					System.Windows.Forms.MessageBox.Show("Error creating automata (" + result + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				new AutomataEdit(username, newAutomataName).Show();
				Close();
			}
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
