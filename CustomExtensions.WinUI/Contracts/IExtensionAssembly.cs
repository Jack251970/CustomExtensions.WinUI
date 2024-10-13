using System.Reflection;
using System.Runtime.CompilerServices;

namespace CustomExtensions.WinUI.Contracts;

public interface IExtensionAssembly : IDisposable
{
    Assembly ForeignAssembly { get; }

    void LoadResources(bool loadPriResources);

	Task LoadResourcesAsync(bool loadPriResources);

	bool TryEnableHotReload();

    string? TryLoadXamlResources();

    Uri LocateResource(object component, [CallerFilePath] string callerFilePath = "");
}
