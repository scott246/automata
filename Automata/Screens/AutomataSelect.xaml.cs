using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace Automata
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class AutomataSelect : Window
	{
		readonly string username = "";
		List<AutomataData> automataList = new List<AutomataData>();
		private int selectedRow = -1;
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
			var automata = Operations.Select("SELECT automata_name, automata_desc, enabled, created_date, updated_date FROM automata ORDER BY updated_date DESC;");
			if (automata == null)
			{
				//MessageBox.Show("Error retrieving automata.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				new Message("Error retrieving automata", "Error", false, false).ShowDialog();
				return;
			}
			automataList = new List<AutomataData>();
			for (var i = 0; i < automata.Count; i++)
			{
				automataList.Add(new AutomataData()
				{
					Enabled = automata[i][2],
					Name = automata[i][0],
					Description = automata[i][1],
					CreatedTime = automata[i][3],
					UpdatedTime = automata[i][4],
				});
			}
			AutomataGrid.ItemsSource = automataList;
		}

		void OnRowSelect(object sender, RoutedEventArgs e)
		{
			DataGridRow r = (DataGridRow)sender;
			selectedRow = r.GetIndex();
			ToggleButton.Visibility = Visibility.Visible;
			EditButton.Visibility = Visibility.Visible;
			DeleteButton.Visibility = Visibility.Visible;
		}

		void OnRowDeselect(object sender, RoutedEventArgs e)
		{
			selectedRow = -1;
			ToggleButton.Visibility = Visibility.Hidden;
			EditButton.Visibility = Visibility.Hidden;
			DeleteButton.Visibility = Visibility.Hidden;
		}

		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			List<string> existingAutomataNames = new List<string>();
			foreach (AutomataData ad in automataList)
			{
				existingAutomataNames.Add(ad.Name);
			}
			NewAutomata asn = new NewAutomata(existingAutomataNames.ToArray());
			asn.ShowDialog();
			string newAutomataName = asn.automataName;
			string newAutomataDesc = asn.automataDesc;
			if (newAutomataName != "")
			{
				var result = Operations.Insert(
				string.Format("INSERT INTO automata(automata_name, automata_desc, enabled, created_date, updated_date) VALUES (\"{0}\", \"{1}\", true, \"{2}\", \"{3}\");",
				newAutomataName,
				newAutomataDesc,
				Operations.GetDateTime(),
				Operations.GetDateTime()));
				if (result != 0)
				{
					//MessageBox.Show("Error creating automata (" + result + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					new Message("Error creating automata (" + result + ").", "Error", false, false).ShowDialog();
					return;
				}
				new AutomataEdit(username, newAutomataName).Show();
				Close();
			}
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			TextBlock t = AutomataGrid.Columns[1].GetCellContent(AutomataGrid.Items[selectedRow]) as TextBlock;
			new AutomataEdit(username, t.Text).Show();
			Close();
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			Message m = new Message("Are you sure you want to delete? This cannot be undone.", "Delete?", false, true);
			m.ShowDialog();
			int i = m.response;
			//DialogResult dr = MessageBox.Show("Are you sure you want to delete? This cannot be undone.", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			if (i == 1)
			{
				TextBlock t = AutomataGrid.Columns[1].GetCellContent(AutomataGrid.Items[selectedRow]) as TextBlock;
				int result = Operations.Delete(
				string.Format("DELETE FROM automata WHERE automata_name='{0}';", t.Text));
				if (result != 0)
				{
					//MessageBox.Show("Error deleting automata (" + result + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					new Message("Error deleting automata (" + result + ").", "Error", false, false).ShowDialog();
					return;
				}
				Dispatcher.BeginInvoke(new Action(() =>
				{
					AutomataGrid.UnselectAll();
					PopulateDataGrid();
					AutomataGrid.Items.Refresh();
				}));
			}
		}

		private void ToggleButton_Click(object sender, RoutedEventArgs e)
		{
			//toggle automata enabled
			var c = AutomataGrid.Columns[0].GetCellContent(AutomataGrid.Items[selectedRow]);
			ContentPresenter cp = (ContentPresenter)c;
			AutomataData ad = (AutomataData)cp.Content;
			ad.Enabled = !ad.Enabled;

			//get name of changed status' automata
			TextBlock t = AutomataGrid.Columns[1].GetCellContent(AutomataGrid.Items[selectedRow]) as TextBlock;
				
			//update DB
			var result = Operations.Update(
				string.Format("UPDATE automata SET enabled={0}, updated_date='{1}' WHERE automata_name='{2}';",
				ad.Enabled,
				Operations.GetDateTime(),
				t.Text));
			if (result != 0)
			{
				//MessageBox.Show("Error changing automata status (" + result + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				new Message("Error changing automata status (" + result + ").", "Error", false, false).ShowDialog();
			}

			Dispatcher.BeginInvoke(new Action(() =>
			{
				AutomataGrid.UnselectAll();
				AutomataGrid.Items.Refresh();
			}));
		}

		private void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			new Login().Show();
			Close();
		}

		private void FilterButton_Click(object sender, RoutedEventArgs e)
		{
			if (AutomataFilterInput.Visibility == Visibility.Hidden)
			{
				AutomataFilterInput.Visibility = Visibility.Visible;
				AutomataFilterCount.Visibility = Visibility.Visible;
				AutomataFilterCount.Content = AutomataGrid.Items.Count + " found";
				AutomataFilterInput.SelectAll();
				AutomataFilterInput.Focus();
			}
			else
			{
				AutomataFilterInput.Visibility = Visibility.Hidden;
				AutomataFilterCount.Visibility = Visibility.Hidden;
				AutomataFilterInput.Text = "";
				AutomataFilterCount.Content = "";
			}
		}

		private void AutomataFilterInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox tb = (TextBox)e.Source;
			string input = tb.Text;
			int count = 0;
			List<AutomataData> filteredList = new List<AutomataData>();
			if (input.Length == 0)
			{
				AutomataGrid.ItemsSource = automataList;
				AutomataFilterCount.Content = AutomataGrid.Items.Count + " found";
			}
			else
			{
				foreach (var item in automataList)
				{
					
					if (item.Name.Contains(input))
					{
						count++;
						filteredList.Add(item);
					}
				}
				AutomataFilterCount.Content = count + " found";
				AutomataGrid.ItemsSource = filteredList;
			}
		}

		private void HelpButton_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/scott246/automata/wiki");
		}
	}

	public class AutomataData
	{
		public bool Enabled { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime CreatedTime { get; set; }
		public DateTime UpdatedTime { get; set; }
	}
}
