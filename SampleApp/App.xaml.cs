using CustomExtensions.WinUI.Models;
using Microsoft.UI.Xaml;
using SampleApp.Views;

namespace SampleApp;

public partial class App : Application
{
	private static Window MainWindow { get; set; } = null!;

	public App()
	{
		InitializeComponent();

		// Initialize the application extension host
		ApplicationExtensionHost.Initialize(this);
	}

	protected override void OnLaunched(LaunchActivatedEventArgs args)
	{
		MainWindow = new MainWindow();

		MainWindow.Activate();
	}
}
