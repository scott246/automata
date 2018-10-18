using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Automata
{
    class DBOps
    {
		private static MySqlConnection connection;

		//Constructor
		public DBOps()
		{
			Initialize();
		}

		//Initialize values
		private static void Initialize()
		{
			string connectionString;
			connectionString = "SERVER=" + SecretsManagement.EnterSecretsVault("dbserver") + ";" 
				+ "DATABASE=" + SecretsManagement.EnterSecretsVault("dbname") + ";" 
				+ "UID=" + SecretsManagement.EnterSecretsVault("dbun") + ";" 
				+ "PASSWORD=" + SecretsManagement.EnterSecretsVault("dbpw") + ";"
				+ "SslMode=none;";

			connection = new MySqlConnection(connectionString);
		}

		//open connection to database
		private static bool OpenConnection()
		{
			Initialize();
			try
			{
				connection.Open();
				return true;
			}
			catch (MySqlException ex)
			{
				//When handling errors, you can your application's response based 
				//on the error number.
				//The two most common error numbers when connecting are as follows:
				//0: Cannot connect to server.
				//1045: Invalid user name and/or password.
				switch (ex.Number)
				{
					case 0:
						MessageBox.Show("Cannot connect to server. Contact administrator");
						break;

					case 1045:
						MessageBox.Show("Invalid username/password, please try again");
						break;
				}
				return false;
			}
		}

		//Close connection
		private static bool CloseConnection()
		{
			try
			{
				connection.Close();
				return true;
			}
			catch (MySqlException ex)
			{
				MessageBox.Show(ex.Message);
				return false;
			}
		}

		//Insert statement
		public static void Insert(string query)
		{
			//sample string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";

			//open connection
			if (OpenConnection() == true)
			{
				//create command and assign the query and connection from the constructor
				MySqlCommand cmd = new MySqlCommand(query, connection);

				//Execute command
				cmd.ExecuteNonQuery();

				//close connection
				CloseConnection();
			}
		}

		//Update statement
		public static void Update(string query)
		{
			//sample string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

			//Open connection
			if (OpenConnection() == true)
			{
				//create mysql command
				MySqlCommand cmd = new MySqlCommand();
				//Assign the query using CommandText
				cmd.CommandText = query;
				//Assign the connection using Connection
				cmd.Connection = connection;

				//Execute query
				cmd.ExecuteNonQuery();

				//close connection
				CloseConnection();
			}
		}

		//Delete statement
		public static void Delete(string query)
		{
			//sample string query = "DELETE FROM tableinfo WHERE name='John Smith'";

			if (OpenConnection() == true)
			{
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.ExecuteNonQuery();
				CloseConnection();
			}
		}

		//Select statement
		public static List<dynamic> Select(string query)
		{
			//sample string query = "SELECT * FROM tableinfo";



			//Create a list to store the result
			List<dynamic> list = new List<dynamic>();

			//Open connection
			if (OpenConnection() == true)
			{
				//Create Command
				MySqlCommand cmd = new MySqlCommand(query, connection);
				//Create a data reader and Execute the command
				MySqlDataReader dataReader = cmd.ExecuteReader();

				//Read the data and store them in the list
				while (dataReader.Read())
				{
					for (int i = 0; i < dataReader.FieldCount; i++)
					{
						if (query.Contains(dataReader.GetName(i)))
						{
							list[i].Add(dataReader.GetValue(i));
						}
					}
				}

				//close Data Reader
				dataReader.Close();

				//close Connection
				CloseConnection();

				//return list to be displayed
				return list;
			}
			else
			{
				return null;
			}
		}

		//Count statement
		public static int Count(string query)
		{
			//sample string query = "SELECT Count(*) FROM tableinfo";
			int count = -1;

			//Open Connection
			if (OpenConnection() == true)
			{
				//Create Mysql Command
				MySqlCommand cmd = new MySqlCommand(query, connection);

				//ExecuteScalar will return one value
				count = int.Parse(cmd.ExecuteScalar() + "");

				//close Connection
				CloseConnection();

				return count;
			}
			else
			{
				return count;
			}
		}
	}
}
