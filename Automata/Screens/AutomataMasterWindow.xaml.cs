using System.Windows;

namespace Automata.Screens
{
	/// <summary>
	/// Interaction logic for AutomataMasterWindow.xaml
	/// </summary>
	public partial class AutomataMasterWindow : Window
	{
		//TODO: change this back to true (selection screen)
		bool selectionScreen = false;
		AutomataSelect asControl = new AutomataSelect();
		AutomataEdit aeControl = new AutomataEdit();
		public AutomataMasterWindow()
		{
			InitializeComponent();
			masterContentControl.Content = new AutomataSelect();
			asControl.OnEditMode += (sensor, e) =>
			{
				SwitchScreens();
			};
			asControl.OnClose += (sensor, e) =>
			{
				Close();
			};
			aeControl.OnReturnToSelect += (sensor, e) =>
			{
				SwitchScreens();
			};
			//TODO: change this back to asControl (selection screen)
			masterContentControl.Content = aeControl;
		}

		public AutomataMasterWindow(string name)
		{
			InitializeComponent();
			masterContentControl.Content = new AutomataSelect(name);
		}

		public void SwitchScreens()
		{
			if (selectionScreen)
			{
				masterContentControl.Content = aeControl;
				selectionScreen = !selectionScreen;
			}
			else
			{
				masterContentControl.Content = asControl;
				selectionScreen = !selectionScreen;
			}
		}
	}
}
