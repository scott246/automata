using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Automata
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class Login : Window
	{
		AutomataSelect as1;
		public Login()
		{
			var name = "";
			if (!(name = CheckLogin()).Equals(""))
			{
				as1 = new AutomataSelect(name);
				as1.Show();
				Close();
			}
			InitializeComponent();
		}

		public string CheckLogin()
		{
			string query = "SELECT uname FROM users WHERE machineName='{0}' AND persist=true;";
			string queryToSelect = string.Format(query, Environment.MachineName);
			var data = DBOps.Select(queryToSelect);
			if (data.Count > 0)
			{
				return data[0][0];
			}
			return "";
		}

		private string HashAndSalt(string pw)
		{
			var prf = KeyDerivationPrf.HMACSHA256;
			byte[] salt = Encoding.ASCII.GetBytes(SecretsManagement.EnterSecretsVault("salt"));
			const int iterCount = 10000;
			const int numBytesRequested = 256 / 8;
			var subkey = KeyDerivation.Pbkdf2(pw, salt, prf, iterCount, numBytesRequested);
			return SecretsManagement.EnterSecretsVault("salt2") + Convert.ToBase64String(subkey);
		}

		public void Show_Help(object sender, RoutedEventArgs e)
		{
			ErrorText.Text = "Existing users type username and password and click Login."+Environment.NewLine+"New users type desired username and password and click Register.";
		}

		private void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			string username = UsernameBox.Text;
			string password = HashAndSalt(PasswordBox.Password);
			if (username.Length == 0)
			{
				ErrorText.Text = "Username cannot be blank.";
				UsernameBox.SelectAll();
				UsernameBox.Focus();
			}
			else if (PasswordBox.Password.Length < 8)
			{
				ErrorText.Text = "Password must be at least 8 characters.";
				PasswordBox.SelectAll();
				PasswordBox.Focus();
			}
			else
			{
				string query = "INSERT INTO users(uname, pw, loggedin, persist) VALUES (\"{0}\", \"{1}\", false, false);";
				string queryToInsert = string.Format(query, username, password);
				var insertResult = DBOps.Insert(queryToInsert);
				int code = insertResult.First().Key;
				string message = insertResult.First().Value;
				if (code == 0)
				{
					ErrorText.Text = "Registration successful! Please login to access application.";
				}
				else if (code == 1062)
				{
					ErrorText.Text = "Username taken.";
				}
				else
				{
					ErrorText.Text = "Server error. (" + code + ": " + message + ")";
				}
			}
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string username = UsernameBox.Text;
			string password = HashAndSalt(PasswordBox.Password);
			bool persist = (bool)PersistLoginCheckBox.IsChecked;

			string query = "SELECT uid FROM users WHERE uname='{0}' AND pw='{1}';";
			string query2;
			if (persist)
			{
				query2 = "UPDATE users SET loggedIn=true, persist=true, machineName='{0}' WHERE uname='{1}';";
			}
			else
			{
				query2 = "UPDATE users SET loggedIn=true, persist=false, machineName='{0}' WHERE uname='{1}';";
			}
			string queryToSelect = string.Format(query, username, password);
			string queryToUpdate = string.Format(query2, Environment.MachineName, username);
			var selectResult = DBOps.Select(queryToSelect);
			if (selectResult.Count > 0)
			{
				int uid = Convert.ToInt32(selectResult[0][0]);
				var code = DBOps.Update(queryToUpdate);
				if (code.Keys.First() == 0)
				{
					string query3 = "INSERT INTO logins (uid, tstamp) VALUES ({0}, '{1}');";
					string queryToInsert = string.Format(query3, uid, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					var insertResult = DBOps.Insert(queryToInsert);
					if (insertResult.First().Key == 0)
					{
						as1 = new AutomataSelect(username);
						as1.Show();
						Close();
					}
					else
					{
						ErrorText.Text = "Server error. (" + insertResult.Keys.First() + ": " + insertResult.Values.First() + ")";
					}
				}
				else
				{
					ErrorText.Text = "Server error. (" + code.Keys.First() + ": " + code.Values.First() + ")";
				}
			}
			else
			{
				ErrorText.Text = "Incorrect username or password."; 
			}
		}
	}
}
