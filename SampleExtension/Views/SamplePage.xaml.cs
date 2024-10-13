using CustomExtensions.WinUI.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SampleExtension.Models;

namespace SampleExtension.Views;

public sealed partial class SamplePage : Page
{
	public GreetEntity InitialSelectedEntity
	{
		get { return (GreetEntity)GetValue(SelectedEntityProperty); }
		set { SetValue(SelectedEntityProperty, value); }
	}

	// Using a DependencyProperty as the backing store for InitialSelectedEntity.  This enables animation, styling, binding, etc...
	public static readonly DependencyProperty SelectedEntityProperty =
		DependencyProperty.Register(nameof(InitialSelectedEntity), typeof(GreetEntity), typeof(SamplePage), new PropertyMetadata(GreetEntity.World));

	public SamplePage()
	{
		// Method 1: Load the Xaml files when the extension is loading.
		InitializeComponent();

		// Method 2: Load the Xaml files every time when they are needed.
		this.LoadComponent(ref _contentLoaded);
	}
}
