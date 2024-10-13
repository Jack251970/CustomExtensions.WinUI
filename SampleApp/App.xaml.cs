using CustomExtensions.WinUI.Models;
using Microsoft.UI.Xaml;
using SampleApp.Views;

namespace SampleApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Initialize the application extension host
		ApplicationExtensionHost.Initialize(this);
	}

	private static Window MainWindow { get; set; } = null!;

	protected override void OnLaunched(LaunchActivatedEventArgs args)
	{
		MainWindow = new MainWindow();

		MainWindow.Activate();
	}
}
