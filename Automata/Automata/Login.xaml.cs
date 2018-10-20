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
		AutomataSelect as1 = new AutomataSelect();
		public Login()
		{
			InitializeComponent();
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
			ErrorText.Content = "Existing users type username and password and click Login."+Environment.NewLine+"New users type desired username and password and click Register.";
		}

		private void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			string username = UsernameBox.Text;
			string password = HashAndSalt(PasswordBox.Password);
			if (username.Length == 0)
			{
				ErrorText.Content = "Username cannot be blank.";
				UsernameBox.SelectAll();
				UsernameBox.Focus();
			}
			else if (PasswordBox.Password.Length < 8)
			{
				ErrorText.Content = "Password must be at least 8 characters.";
				PasswordBox.SelectAll();
				PasswordBox.Focus();
			}
			else
			{
				//attempt registration
				string query = "INSERT INTO users(uname, pw, loggedin, persist) VALUES (\"{0}\", \"{1}\", false, false);";
				string queryToInsert = string.Format(query, username, password);
				ErrorText.Content = "Loading...";
				var insertResult = DBOps.Insert(queryToInsert);
				int code = insertResult.First().Key;
				string message = insertResult.First().Value;
				if (code == 0)
				{
					ErrorText.Content = "Registration successful! Please login to access application.";
				}
				else if (code == 1062)
				{
					ErrorText.Content = "Username taken.";
				}
				else
				{
					ErrorText.Content = "Server error. (" + code + ": " + message + ")";
				}
			}
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string username = UsernameBox.Text;
			string password = HashAndSalt(PasswordBox.Password);

			//attempt login
			string query = "SELECT * FROM users WHERE uname='{0}' AND pw='{1}';";
			string queryToSelect = string.Format(query, username, password);
			ErrorText.Content = "Loading...";
			var selectResult = DBOps.Select(queryToSelect);
			if (selectResult.Count > 0)
			{
				as1.test.Content = "Hi, " + username;
				as1.Show();
				this.Close();
			}
			else
			{
				ErrorText.Content = "Incorrect username or password."; 
			}
		}
	}
}
