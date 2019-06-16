using System;

namespace Automata.Actions
{
	class FileCreate
	{
		public static void CreateFile(string path)
		{

			//TODO: make sure that the folder structure exists
			try
			{
				if (System.IO.Path.HasExtension(path))
				{
					if (!System.IO.File.Exists(path))
						System.IO.File.Create(path);
				}
				else
				{
					System.IO.Directory.CreateDirectory(path);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Creation failed. " + e.Message);
			}
		}
	}
}
