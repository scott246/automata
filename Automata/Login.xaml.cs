using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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
			InitializeComponent();
			UsernameBox.Focus();
			this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
		}

		private void HandleEsc(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
				Close();
		}

		private string HashAndSalt(string pw)
		{
			var prf = KeyDerivationPrf.HMACSHA256;
			byte[] salt = Encoding.ASCII.GetBytes("p*rfu9;a@iq2>8e#$");
			const int iterCount = 10000;
			const int numBytesRequested = 256 / 8;
			var subkey = KeyDerivation.Pbkdf2(pw, salt, prf, iterCount, numBytesRequested);
			return "a;8wa34(&*F93o!h7f32" + Convert.ToBase64String(subkey);
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
			else if (!(new Regex(@"^[a-zA-Z0-9]*$").IsMatch(username)))
			{
				ErrorText.Text = "Usernames must be alphanumeric.";
				UsernameBox.SelectAll();
				UsernameBox.Focus();
			}
			else
			{
				var addUserResult = DBOps.Insert(
					string.Format(
					"INSERT INTO users(username, password) VALUES (\"{0}\", \"{1}\");",
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
			var usersWithCreds = DBOps.Select(
				string.Format(
				"SELECT username FROM users WHERE username='{0}' AND password='{1}';",
				username,
				password));

			if (usersWithCreds.Count > 0)
			{
				as1 = new AutomataSelect(username);
				as1.Show();
				Close();
			}
			else
			{
				ErrorText.Text = "Incorrect username or password."; 
			}
		}
	}
}
