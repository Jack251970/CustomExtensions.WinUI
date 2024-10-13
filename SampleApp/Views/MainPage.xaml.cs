using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SampleApp.Models;

namespace SampleApp.Views;

public sealed partial class MainPage : Page
{
	public ObservableCollection<ExtensionModel> MenuItems = new();

	public MainPage()
	{
		InitializeComponent();
	}

	private void MainWindow_Closed(object sender, WindowEventArgs args)
	{
		foreach (var item in MenuItems)
		{
			item.Dispose();
		}
	}

	private void Page_Loaded(object sender, RoutedEventArgs e)
	{
#if DEBUG
		string searchDir = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\..\SampleExtension\bin\Debug\net6.0-windows10.0.22621.0");
#else
		string searchDir = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\..\SampleExtension\bin\Release\net6.0-windows10.0.22621.0");
#endif
		foreach (FileInfo fileInfo in new DirectoryInfo(searchDir).EnumerateFiles("*.SampleAppExtension.dll", new EnumerationOptions() { RecurseSubdirectories = true }))
		{
			MenuItems.Add(new(fileInfo.FullName));
		}

		App.MainWindow.Closed += MainWindow_Closed;
	}

	private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
	{
		if (args.SelectedItem is ExtensionModel model)
		{
			model.Load();
		}
	}
}
