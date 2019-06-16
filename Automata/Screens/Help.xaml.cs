using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Automata.Screens
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Help : Window
	{
		public Help()
		{
			InitializeComponent();
			string curDir = Directory.GetCurrentDirectory();
			browser.Navigate(new Uri(String.Format("file:///{0}/screens/helpcontent/helpmain.html", curDir)));
		}
	}
}
