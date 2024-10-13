using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Markup;

namespace CustomExtensions.WinUI.Contracts;

public interface IApplicationExtensionHost
{
	/// <summary>
	/// Gets the current instance of the application extension host.
	/// </summary>
	/// <param name="pathToAssembly">The path to the assembly to load.</param>
	/// <param name="loadPriResourcesIntoWinResourceMap">
	/// Whether to load the PRI resources into the win resource map.
	/// See <see cref="Microsoft.Windows.ApplicationModel.Resources.ResourceMap"/>.
	/// </param>
	/// <param name="loadPriResourcesIntoCoreResourceMap">
	/// Whether to load the PRI resources into the core resource map.
	/// See <see cref="Windows.ApplicationModel.Resources.Core.ResourceMap"/>.
	/// </param>
	/// <returns>The loaded extension assembly.</returns>
	IExtensionAssembly LoadExtension(string pathToAssembly, bool loadPriResourcesIntoWinResourceMap = false, bool loadPriResourcesIntoCoreResourceMap = false);

	/// <summary>
	/// Gets the current instance of the application extension host.
	/// </summary>
	/// <param name="pathToAssembly">The path to the assembly to load.</param>
	/// <param name="loadPriResourcesIntoWinResourceMap">
	/// Whether to load the PRI resources into the win resource map.
	/// See <see cref="Microsoft.Windows.ApplicationModel.Resources.ResourceMap"/>.
	/// </param>
	/// <param name="loadPriResourcesIntoCoreResourceMap">
	/// Whether to load the PRI resources into the core resource map.
	/// See <see cref="Windows.ApplicationModel.Resources.Core.ResourceMap"/>.
	/// </param>
	/// <returns>The loaded extension assembly.</returns>
	Task<IExtensionAssembly> LoadExtensionAsync(string pathToAssembly, bool loadPriResourcesIntoWinResourceMap = false, bool loadPriResourcesIntoCoreResourceMap = false);

	/// <summary>
	/// Get the type of the interface from the assembly.
	/// </summary>
	/// <param name="assembly">The assembly to search for the type.</param>
	/// <param name="type">The type of the interface.</param>
	/// <returns>The type of the interface from the assembly.</returns>
	Type FromAssemblyGetTypeOfInterface(Assembly assembly, Type type);

	/// <summary>
	/// Registers the specified XAML type metadata provider.
	/// </summary>
	/// <param name="provider">The XAML type metadata provider to register.</param>
	/// <returns>A disposable object that represents the registration.</returns>
	IDisposable RegisterXamlTypeMetadataProvider(IXamlMetadataProvider provider);

	/// <summary>
	/// Locates the resource for the specified component.
	/// </summary>
	/// <param name="component">The component for which to locate the resource.</param>
	/// <param name="callerFilePath">The path of the file that calls this method.</param>
	/// <returns>The URI of the located resource.</returns>
	Uri LocateResource(object component, [CallerFilePath] string callerFilePath = "");
}
