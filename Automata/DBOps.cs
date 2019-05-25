using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Automata
{
    public class DBOps
    {
		private static MySqlConnection connection;
		private static bool initialized = false;
		//Constructor
		public DBOps()
		{
			if (!initialized)
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
			initialized = true;
		}

		//open connection to database
		private static bool OpenConnection()
		{
			try
			{
				if (!initialized)
					Initialize();
				if (connection != null && connection.State == ConnectionState.Closed)
					connection.Open();
				return true;
			}
			catch (MySqlException)
			{
				return false;
			}
		}

		//Close connection
		private static bool CloseConnection()
		{
			try
			{
				if (connection != null && connection.State == ConnectionState.Open)
					connection.Close();
				return true;
			}
			catch (MySqlException)
			{
				return false;
			}
		}

		//Insert statement
		public static int Insert(string query)
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
					CloseConnection();
					return e.Number;
				}
				CloseConnection();
				return 0;
			}
			return -1;
		}
		
		//Update statement
		public static int Update(string query)
		{
			//sample string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

			if (OpenConnection() == true)
			{
				try
				{
					MySqlCommand cmd = new MySqlCommand
					{
						CommandText = query,
						Connection = connection
					};
					cmd.ExecuteNonQuery();
					CloseConnection();
				}
				catch (MySqlException e)
				{
					CloseConnection();
					return e.Number;
				}
				return 0;
			}
			return -1;
		}

		//Delete statement
		public static int Delete(string query)
		{
			//sample string query = "DELETE FROM tableinfo WHERE name='John Smith'";

			if (OpenConnection() == true)
			{
				try
				{
					MySqlCommand cmd = new MySqlCommand(query, connection);
					cmd.ExecuteNonQuery();
					CloseConnection();
				}
				catch (MySqlException e)
				{
					CloseConnection();
					return e.Number;
				}
				return 0;
			}
			return -1;
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
