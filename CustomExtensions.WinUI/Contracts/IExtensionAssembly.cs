using System.Reflection;
using System.Runtime.CompilerServices;

namespace CustomExtensions.WinUI.Contracts;

public interface IExtensionAssembly : IDisposable
{
	/// <summary>
	/// Gets the foreign assembly that is loaded by the extension.
	/// </summary>
	Assembly ForeignAssembly { get; }

	/// <summary>
	/// Loads the resources from the foreign assembly.
	/// </summary>
	/// <param name="loadPriResourcesIntoWinResourceMap">
	/// Whether to load the PRI resources into the win resource map.
	/// See <see cref="Microsoft.Windows.ApplicationModel.Resources.ResourceMap"/>.
	/// </param>
	/// <param name="loadPriResourcesIntoCoreResourceMap">
	/// Whether to load the PRI resources into the core resource map.
	/// See <see cref="Windows.ApplicationModel.Resources.Core.ResourceMap"/>.
	/// </param>
	void LoadResources(bool loadPriResourcesIntoWinResourceMap = false, bool loadPriResourcesIntoCoreResourceMap = false);

	/// <summary>
	/// Loads the resources from the foreign assembly.
	/// </summary>
	/// <param name="loadPriResourcesIntoWinResourceMap">
	/// Whether to load the PRI resources into the win resource map.
	/// See <see cref="Microsoft.Windows.ApplicationModel.Resources.ResourceMap"/>.
	/// </param>
	/// <param name="loadPriResourcesIntoCoreResourceMap">
	/// Whether to load the PRI resources into the core resource map.
	/// See <see cref="Windows.ApplicationModel.Resources.Core.ResourceMap"/>.
	/// </param>
	Task LoadResourcesAsync(bool loadPriResourcesIntoWinResourceMap = false, bool loadPriResourcesIntoCoreResourceMap = false);

	/// <summary>
	/// Try to enable hot reload for the extension assembly.
	/// </summary>
	/// <returns>True if hot reload is enabled; otherwise, false.</returns>
	bool TryEnableHotReload();

	/// <summary>
	/// Try to infer the correct path to the Xaml file.
	/// </summary>
	/// <returns>The path to the Xaml file if found; otherwise, null.</returns>
	string? TryLoadXamlResources();

	/// <summary>
	/// Locates the resource for the specified component.
	/// </summary>
	/// <param name="component">The component for which to locate the resource.</param>
	/// <param name="callerFilePath">The path of the file that calls this method.</param>
	/// <returns>The URI of the located resource.</returns>
	Uri LocateResource(object component, [CallerFilePath] string callerFilePath = "");
}
