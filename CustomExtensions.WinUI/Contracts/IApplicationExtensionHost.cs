using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Markup;

namespace CustomExtensions.WinUI.Contracts;

public interface IApplicationExtensionHost
{
    IExtensionAssembly LoadExtension(string pathToAssembly);

	IExtensionAssembly LoadExtensionAndPriResources(string pathToAssembly);

	Task<IExtensionAssembly> LoadExtensionAsync(string pathToAssembly);

	Task<IExtensionAssembly> LoadExtensionAndPriResourcesAsync(string pathToAssembly);

	Type FromAssemblyGetTypeOfInterface(Assembly assembly, Type type);

    IDisposable RegisterXamlTypeMetadataProvider(IXamlMetadataProvider provider);

    Uri LocateResource(object component, [CallerFilePath] string callerFilePath = "");
}
