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

	private void Page_Loaded(object sender, RoutedEventArgs e)
	{
		string searchDir = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\..\SampleExtension\bin\Debug\net6.0-windows10.0.19041.0");
		foreach (FileInfo fileInfo in new DirectoryInfo(searchDir).EnumerateFiles("*.SampleAppExtension.dll", new EnumerationOptions() { RecurseSubdirectories = true }))
		{
			MenuItems.Add(new(fileInfo.FullName));
		}
	}

	private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
	{
		if (args.SelectedItem is ExtensionModel model)
		{
			model.Load();
		}
	}
}
