using Microsoft.UI.Xaml;
using SampleApp.Extensibility;
using SampleExtension.Views;

namespace SampleExtension;

public class SampleExtension : IExtension
{
	private FrameworkElement _content;

	public string Name => "Sample Extension";

	public FrameworkElement Content => EnsureContent();

	private FrameworkElement EnsureContent()
	{
		if (_content != null)
		{
			return _content;
		}

		return _content = new SamplePage();
	}
}
