using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata
{
	class SecretsManagement
    {
		public static dynamic EnterSecretsVault(string s)
		{
			string json = string.Empty;
			using (StreamReader r = new StreamReader("../../secrets.json"))
			{
				json = r.ReadToEnd();
			}
			return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json)[s];
		}
	}
}
