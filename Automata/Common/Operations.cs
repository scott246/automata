﻿using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using MySql.Data.MySqlClient;
using System;

namespace Automata
{
	public class Operations
	{
		//UNIVERSAL STATIC VARIABLES
		public static int AutomataMaxNameLength = 30;
		public static int AutomataMaxDescLength = 100;

		private static MySqlConnection connection;
		private static bool initialized = false;

		public static dynamic GetSecret(string s)
		{
			string json = string.Empty;
			using (StreamReader r = new StreamReader("../../Common/secrets.json"))
			{
				json = r.ReadToEnd();
			}
			return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json)[s];
		}

		public static string GetDateTime()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

		//Initialize values
		private static void Initialize()
		{
			string connectionString;
			connectionString = "SERVER=" + GetSecret("dbserver") + ";"
				+ "DATABASE=" + GetSecret("dbname") + ";"
				+ "UID=" + GetSecret("dbun") + ";"
				+ "PASSWORD=" + GetSecret("dbpw") + ";"
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
				while (dataReader.Read())
				{
					List<dynamic> innerList = new List<dynamic>();
					for (int col = 0; col < dataReader.FieldCount; col++)
					{
						if (dataReader.GetName(col).Length > 0)
						{
							innerList.Add(dataReader.GetValue(col));
						}
					}
					list.Add(innerList);
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
