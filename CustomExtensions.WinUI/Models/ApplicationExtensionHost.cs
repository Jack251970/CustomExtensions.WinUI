using System.Reflection;

namespace CustomExtensions.WinUI.Models;

/// <summary>
/// Provides a host for application extensions.
/// </summary>
public static partial class ApplicationExtensionHost
{
    internal static bool IsHotReloadEnabled => Environment.GetEnvironmentVariable("ENABLE_XAML_DIAGNOSTICS_SOURCE_INFO") == "1";

    private static IApplicationExtensionHost? _Current;

	/// <summary>
	/// Gets the current instance of the application extension host.
	/// </summary>
	public static IApplicationExtensionHost Current => _Current ?? throw new InvalidOperationException("ApplicationExtensionHost is not initialized");

	/// <summary>
	/// Initializes the application extension host with the specified application.
	/// <see cref="Microsoft.UI.Xaml.Application"/>
	/// </summary>
	/// <typeparam name="TApplication">The type of the application.</typeparam>
	/// <param name="application">The application to initialize the application extension host with.</param>
	/// <exception cref="InvalidOperationException">Cannot initialize application twice.</exception>
	public static void Initialize<TApplication>(TApplication application) where TApplication : Microsoft.UI.Xaml.Application
	{
        if (_Current != null)
        {
            throw new InvalidOperationException("Cannot initialize application twice");
        }

        _Current = new ApplicationExtensionHostSingleton<TApplication>(application);
	}

	/// <summary>
	/// Gets the default resource manager for the specified assembly, or the caller's executing assembly if not provided.
	/// <see cref="Microsoft.Windows.ApplicationModel.Resources.ResourceManager"/>
	/// </summary>
	/// <param name="assembly">Assembly for which to load the default resource manager</param>
	/// <returns>A ResourceManager if one is found, otherwise null</returns>
	public static Microsoft.Windows.ApplicationModel.Resources.ResourceManager? GetWinResourceManagerForAssembly(Assembly? assembly = null)
	{
		assembly ??= Assembly.GetCallingAssembly();
		var assemblyName = assembly.GetName().Name;
		if (assemblyName == null)
		{
			return null;
		}

		var resourceMap = TryFindWinResourceManager(assemblyName);
		if (resourceMap != null)
		{
			return resourceMap;
		}
		else
		{
			ResourceExtensions.LoadPriResourcesIntoWinResourceManager(assembly);
			return TryFindWinResourceManager(assemblyName);
		}
	}

	/// <summary>
	/// Gets the default resource map for the specified assembly, or the caller's executing assembly if not provided.
	/// <see cref="Microsoft.Windows.ApplicationModel.Resources.ResourceMap"/>
	/// </summary>
	/// <param name="assembly">Assembly for which to load the default resource map</param>
	/// <returns>A ResourceMap if one is found, otherwise null</returns>
	public static Microsoft.Windows.ApplicationModel.Resources.ResourceMap? GetWinResourceMapForAssembly(Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        var assemblyName = assembly.GetName().Name;
        if (assemblyName == null)
        {
            return null;
        }

		var resourceMap = TryFindWinResourceMap(assemblyName);
		if (resourceMap != null)
		{
			return resourceMap;
		}
		else
		{
			ResourceExtensions.LoadPriResourcesIntoWinResourceManager(assembly);
			return TryFindWinResourceMap(assemblyName);
		}
	}

	/// <summary>
	/// Gets the default resource map for the specified assembly, or the caller's executing assembly if not provided.
	/// <see cref="Windows.ApplicationModel.Resources.Core.ResourceMap"/>
	/// </summary>
	/// <param name="assembly">Assembly for which to load the default resource map</param>
	/// <returns>A ResourceMap if one is found, otherwise null</returns>
	public static Windows.ApplicationModel.Resources.Core.ResourceMap? GetCoreResourceMapForAssembly(Assembly? assembly = null)
	{
		assembly ??= Assembly.GetCallingAssembly();
		var assemblyName = assembly.GetName().Name;
		if (assemblyName == null)
		{
			return null;
		}

		var resourceMap = TryFindCoreResourceMap(assemblyName);
		if (resourceMap != null)
		{
			return resourceMap;
		}
		else
		{
			ResourceExtensions.LoadPriResourcesIntoCoreResourceMap(assembly);
			return TryFindCoreResourceMap(assemblyName);
		}
	}

	/// <summary>
	/// Gets the default resource map for the specified assembly, or the caller's executing assembly if not provided.
	/// <see cref="Windows.ApplicationModel.Resources.Core.ResourceMap"/>
	/// </summary>
	/// <param name="assembly">Assembly for which to load the default resource map</param>
	/// <returns>A ResourceMap if one is found, otherwise null</returns>
	public static async Task<Windows.ApplicationModel.Resources.Core.ResourceMap?> GetCoreResourceMapForAssemblyAsync(Assembly? assembly = null)
	{
		assembly ??= Assembly.GetCallingAssembly();
		var assemblyName = assembly.GetName().Name;
		if (assemblyName == null)
		{
			return null;
		}

		var resourceMap = TryFindCoreResourceMap(assemblyName);
		if (resourceMap != null)
		{
			return resourceMap;
		}
		else
		{
			await ResourceExtensions.LoadPriResourcesIntoCoreResourceMapAsync(assembly);
			return TryFindCoreResourceMap(assemblyName);
		}
	}

	private static Microsoft.Windows.ApplicationModel.Resources.ResourceManager? TryFindWinResourceManager(string assemblyName)
	{
		return ResourceExtensions.WinResourceManager.TryGetValue(assemblyName, out var manager) ? manager : null;
	}

	private static Microsoft.Windows.ApplicationModel.Resources.ResourceMap? TryFindWinResourceMap(string assemblyName)
	{
		return TryFindWinResourceManager(assemblyName)?.MainResourceMap.TryGetSubtree($"{assemblyName}/Resources");
	}

	private static Windows.ApplicationModel.Resources.Core.ResourceMap? TryFindCoreResourceMap(string assemblyName)
	{
		return Windows.ApplicationModel.Resources.Core.ResourceManager.Current.AllResourceMaps.TryGetValue(assemblyName, out var map) ?
			map!.GetSubtree($"{assemblyName}/Resources") :
			Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetSubtree($"{assemblyName}/Resources");
	}
}
