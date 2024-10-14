using Microsoft.UI.Xaml.Media;
using WinUIEx;

namespace SampleApp.Views;

public sealed partial class MainWindow : WindowEx
{
	public MainWindow()
	{
		InitializeComponent();

		Title = "CustomExtensions.WinUI Sample App";

		SystemBackdrop = new MicaBackdrop();
	}
}
