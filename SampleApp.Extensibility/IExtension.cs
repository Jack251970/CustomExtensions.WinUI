using Microsoft.UI.Xaml;

namespace SampleApp.Extensibility;

public interface IExtension
{
	string Name { get; }

	FrameworkElement Content { get; }
}
