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
			UsernameBox.Focus();
			this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
		}

		private void HandleEsc(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
				Close();
		}

		public string CheckLogin()
		{
			var persistentUsers = DBOps.Select(
				string.Format(
				"SELECT uname FROM users WHERE machineName='{0}' AND persist=true;",
				Environment.MachineName));
			if (persistentUsers != null && persistentUsers.Count > 0)
			{
				return persistentUsers[0][0];
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
			if (username.Length == 0 && PasswordBox.Password.Length == 0)
			{
				ErrorText.Text = "To register, type a username and password in the boxes above.";
				UsernameBox.SelectAll();
				UsernameBox.Focus();
			}
			else if (username.Length == 0)
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
				var addUserResult = DBOps.Insert(
					string.Format(
					"INSERT INTO users(uname, pw, persist) VALUES (\"{0}\", \"{1}\", false);",
					username,
					password));
				int code = addUserResult;
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
					ErrorText.Text = "Error registering user. (" + code + ")";
				}
			}
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string username = UsernameBox.Text;
			string password = HashAndSalt(PasswordBox.Password);
			bool persist = (bool)PersistLoginCheckBox.IsChecked;

			string query;
			if (persist)
			{
				query = "UPDATE users SET persist=true, machineName='{0}' WHERE uname='{1}';";
			}
			else
			{
				query = "UPDATE users SET persist=false, machineName='{0}' WHERE uname='{1}';";
			}
			var usersWithCreds = DBOps.Select(
				string.Format(
				"SELECT uname FROM users WHERE uname='{0}' AND pw='{1}';",
				username,
				password));
			if (usersWithCreds.Count > 0)
			{
				int code = DBOps.Update(string.Format(query, Environment.MachineName, username));
				if (code == 0)
				{
					as1 = new AutomataSelect(username);
					as1.Show();
					Close();
				}
				else
				{
					ErrorText.Text = "Error logging in. (" + code + ")";
				}
			}
			else
			{
				ErrorText.Text = "Incorrect username or password."; 
			}
		}
	}
}
