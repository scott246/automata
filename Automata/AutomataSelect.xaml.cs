using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using CheckBox = System.Windows.Controls.CheckBox;
using DataGridCell = System.Windows.Controls.DataGridCell;
using TextBox = System.Windows.Controls.TextBox;

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
		void OnRowSelect(object sender, RoutedEventArgs e)
		{
			//TODO: get this to not fire 3 times
			//get row index
			DataGridRow r = DataGridRow.GetRowContainingElement((DataGridCell)sender);
			int rowindex = r.GetIndex();

			//change checkbox enabled status
			CheckBox c = AutomataGrid.Columns[0].GetCellContent(AutomataGrid.Items[rowindex]) as CheckBox;
			c.IsChecked = !c.IsChecked;

			//get name of changed status' automata
			Console.WriteLine(AutomataGrid.Columns[1].GetCellContent(AutomataGrid.Items[rowindex]));
			TextBlock t = AutomataGrid.Columns[1].GetCellContent(AutomataGrid.Items[rowindex]) as TextBlock;

			//update DB
			var result = DBOps.Update(string.Format("UPDATE automata SET enabled={0} WHERE automata_name='{1}';", c.IsChecked, t.Text));
			if (result != 0)
			{
				System.Windows.Forms.MessageBox.Show("Error changing automata status (" + result + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			//TODO: find a way to unselect row after checking box
			//AutomataGrid.UnselectAll();
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
				System.Windows.Forms.MessageBox.Show("Error creating automata (" + result + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
