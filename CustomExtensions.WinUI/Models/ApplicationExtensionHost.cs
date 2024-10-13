using System.Reflection;
using Microsoft.UI.Xaml;

namespace CustomExtensions.WinUI.Models;

public static partial class ApplicationExtensionHost
{
    internal static bool IsHotReloadEnabled => Environment.GetEnvironmentVariable("ENABLE_XAML_DIAGNOSTICS_SOURCE_INFO") == "1";

    private static IApplicationExtensionHost? _Current;
    public static IApplicationExtensionHost Current => _Current ?? throw new InvalidOperationException("ApplicationExtensionHost is not initialized");

	/// <summary>
	/// Initializes the application extension host with the specified application.
	/// </summary>
	/// <typeparam name="TApplication">The type of the application.</typeparam>
	/// <param name="application">The application to initialize the application extension host with.</param>
	/// <exception cref="InvalidOperationException">Cannot initialize application twice.</exception>
	public static void Initialize<TApplication>(TApplication application) where TApplication : Application
    {
        if (_Current != null)
        {
            throw new InvalidOperationException("Cannot initialize application twice");
        }

        _Current = new ApplicationExtensionHostSingleton<TApplication>(application);
    }

    /// <summary>
    /// Gets the default resource map for the specified assembly, or the caller's executing assembly if not provided.
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

		var find = ResourceExtensions.WinResourceMaps.TryGetValue(assemblyName, out var map);
		if (find)
		{
			return map!.GetSubtree($"{assemblyName}/Resources");
		}
		else
		{
			ResourceExtensions.LoadPriResourcesIntoWinResourceMap(assembly);
			return GetWinResourceMapForAssembly(assembly);
		}
	}

	/// <summary>
	/// Gets the default resource map for the specified assembly, or the caller's executing assembly if not provided.
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

		var find = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.AllResourceMaps.TryGetValue(assemblyName, out var map);
		if (find)
		{
			return map!.GetSubtree($"{assemblyName}/Resources");
		}
		else
		{
			ResourceExtensions.LoadPriResourcesIntoCoreResourceMap(assembly);
			return GetCoreResourceMapForAssembly(assembly);
		}
	}

	/// <summary>
	/// Gets the default resource map for the specified assembly, or the caller's executing assembly if not provided.
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

		var find = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.AllResourceMaps.TryGetValue(assemblyName, out var map);
		if (find)
		{
			return map!.GetSubtree($"{assemblyName}/Resources");
		}
		else
		{
			await ResourceExtensions.LoadPriResourcesIntoCoreResourceMapAsync(assembly);
			return await GetCoreResourceMapForAssemblyAsync(assembly);
		}
	}
}
