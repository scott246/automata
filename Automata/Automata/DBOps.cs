using System;
using System.Collections.Generic;
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
		public static Dictionary<int, string> Insert(string query)
		{
			//sample string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";

			if (OpenConnection() == true)
			{
				MySqlCommand cmd = new MySqlCommand(query, connection);

				try
				{
					cmd.ExecuteNonQuery();
				}
				catch (MySqlException e)
				{
					Console.WriteLine(e.Number);
					Console.WriteLine(e.Message);
					CloseConnection();
					return new Dictionary<int, string> { { e.Number, e.Message } };
				}
				CloseConnection();
				return new Dictionary<int, string> { { 0, "Success" } };
			}
			return new Dictionary<int, string> { { -1, "Couldn't open connection" } };
		}

		//Update statement
		public static void Update(string query)
		{
			//sample string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

			if (OpenConnection() == true)
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.CommandText = query;
				cmd.Connection = connection;
				cmd.ExecuteNonQuery();
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

			List<dynamic> list = new List<dynamic>();
			if (OpenConnection() == true)
			{
				MySqlCommand cmd = new MySqlCommand(query, connection);
				MySqlDataReader dataReader = cmd.ExecuteReader();
				int row = 0;
				while (dataReader.Read())
				{
					List<dynamic> innerList = new List<dynamic>();
					for (int col = 0; col < dataReader.FieldCount; col++)
					{
						if (query.Contains(dataReader.GetName(col)))
						{
							innerList.Add(dataReader.GetValue(col));
						}
					}
					list.Add(innerList);
					row++;
				}
				dataReader.Close();
				CloseConnection();
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
			if (OpenConnection() == true)
			{
				MySqlCommand cmd = new MySqlCommand(query, connection);
				count = int.Parse(cmd.ExecuteScalar() + "");
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
