using CustomExtensions.WinUI.Extensions;
using CustomExtensions.WinUI.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SampleExtension.UI;

public sealed partial class Greeter : UserControl
{
	public Greeter()
	{
		// Method 1: Load the Xaml files when the extension is loading.
		InitializeComponent();

		// Method 2: Load the Xaml files every time when they are needed.
		this.LoadComponent(ref _contentLoaded);
	}

	public GreetEntity TargetEntity
	{
		get => (GreetEntity)GetValue(TargetEntityProperty);
		set => SetValue(TargetEntityProperty, value);
	}

	// Using a DependencyProperty as the backing store for TargetEntity.  This enables animation, styling, binding, etc...
	public static readonly DependencyProperty TargetEntityProperty =
		DependencyProperty.Register(nameof(TargetEntity), typeof(GreetEntity), typeof(Greeter), new PropertyMetadata(GreetEntity.World));

	private void Self_Loaded(object sender, RoutedEventArgs e)
	{
		// Method 1: Use `Windows.ApplicationModel.Resources.ResourceLoader` to load pri resources
		Microsoft.Windows.ApplicationModel.Resources.ResourceMap winResources = ApplicationExtensionHost.GetWinResourceMapForAssembly();
		Greeting.Text = winResources.GetValue("Greeting/Text").ValueAsString;

		// Method 2: Use `Windows.ApplicationModel.Resources.Core.ResourceMap` to load pri resources
		Windows.ApplicationModel.Resources.Core.ResourceMap coreResources = ApplicationExtensionHost.GetCoreResourceMapForAssembly();
		Greeting.Text = coreResources.GetValue("Greeting/Text").ValueAsString;
	}
}
