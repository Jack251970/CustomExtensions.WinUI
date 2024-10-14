using System.Collections.Concurrent;
using System.Reflection;

namespace CustomExtensions.WinUI.Extensions;

internal static class ResourceExtensions
{
	#region Win Resources

	#region Management

	internal static ConcurrentDictionary<string, Microsoft.Windows.ApplicationModel.Resources.ResourceManager> WinResourceManager = new();

	#endregion

	#region Load

	public static void LoadPriResourcesIntoWinResourceManager(Assembly ForeignAssembly)
	{
		var foreignAssemblyDir = Path.GetDirectoryName(ForeignAssembly.Location.AssertDefined()).AssertDefined();
		var foreignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();
		LoadPriResourcesIntoWinResourceManager(foreignAssemblyDir, foreignAssemblyName);
	}

	public static void LoadPriResourcesIntoWinResourceManager(string foreignAssemblyDir, string foreignAssemblyName)
	{
		if (WinResourceManager.ContainsKey(foreignAssemblyName))
		{
			return;
		}

		var resourcePriPath = Path.Combine(foreignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			WinResourceManager.TryAdd(foreignAssemblyName, new Microsoft.Windows.ApplicationModel.Resources.ResourceManager(resourcePriPath));
			return;
		}

		resourcePriPath = Path.Combine(foreignAssemblyDir, $"{foreignAssemblyName}.pri");
		if (File.Exists(resourcePriPath))
		{
			WinResourceManager.TryAdd(foreignAssemblyName, new Microsoft.Windows.ApplicationModel.Resources.ResourceManager(resourcePriPath));
			return;
		}

		WinResourceManager.TryAdd(foreignAssemblyName, new Microsoft.Windows.ApplicationModel.Resources.ResourceManager());
	}

	#endregion

	#region Unload

	public static void UnloadPriResourcesFromWinResourceManager(Assembly ForeignAssembly)
	{
		var foreignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();
		UnloadPriResourcesFromWinResourceManager(foreignAssemblyName);
	}

	public static void UnloadPriResourcesFromWinResourceManager(string foreignAssemblyName)
	{
		WinResourceManager.TryRemove(foreignAssemblyName, out _);
	}

	#endregion

	#endregion

	#region Core Resources

	#region Management

	internal static ConcurrentDictionary<string, string> CoreResources = new();

	#endregion

	#region Load

	public static void LoadPriResourcesIntoCoreResourceMap(Assembly ForeignAssembly)
	{
		var foreignAssemblyDir = Path.GetDirectoryName(ForeignAssembly.Location.AssertDefined()).AssertDefined();
		var foreignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();
		LoadPriResourcesIntoCoreResourceMap(foreignAssemblyDir, foreignAssemblyName);
	}

	public static async Task LoadPriResourcesIntoCoreResourceMapAsync(Assembly ForeignAssembly)
	{
		var foreignAssemblyDir = Path.GetDirectoryName(ForeignAssembly.Location.AssertDefined()).AssertDefined();
		var foreignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();
		await LoadPriResourcesIntoCoreResourceMapAsync(foreignAssemblyDir, foreignAssemblyName);
	}

	public static void LoadPriResourcesIntoCoreResourceMap(string foreignAssemblyDir, string foreignAssemblyName)
	{
		if (CoreResources.ContainsKey(foreignAssemblyName))
		{
			return;
		}

		var resourcePriPath = Path.Combine(foreignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			LoadPriResourcesIntoCoreResourceMap(resourcePriPath);
			CoreResources.TryAdd(foreignAssemblyName, resourcePriPath);
			return;
		}

		resourcePriPath = Path.Combine(foreignAssemblyDir, $"{foreignAssemblyName}.pri");
		if (File.Exists(resourcePriPath))
		{
			LoadPriResourcesIntoCoreResourceMap(resourcePriPath);
			CoreResources.TryAdd(foreignAssemblyName, resourcePriPath);
			return;
		}
	}

	public static async Task LoadPriResourcesIntoCoreResourceMapAsync(string foreignAssemblyDir, string foreignAssemblyName)
	{
		var resourcePriPath = Path.Combine(foreignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			await LoadPriResourcesIntoCoreResourceMapAsync(resourcePriPath);
			CoreResources.TryAdd(foreignAssemblyName, resourcePriPath);
			return;
		}

		resourcePriPath = Path.Combine(foreignAssemblyDir, $"{foreignAssemblyName}.pri");
		if (File.Exists(resourcePriPath))
		{
			await LoadPriResourcesIntoCoreResourceMapAsync(resourcePriPath);
			CoreResources.TryAdd(foreignAssemblyName, resourcePriPath);
			return;
		}
	}

	private static void LoadPriResourcesIntoCoreResourceMap(string resourcePriPath)
	{
		FileInfo resourcePriFileInfo = new(resourcePriPath);
		var getFileTask = Windows.Storage.StorageFile.GetFileFromPathAsync(resourcePriFileInfo.FullName).AsTask();
		getFileTask.Wait();
		var file = getFileTask.Result;
		Windows.ApplicationModel.Resources.Core.ResourceManager.Current.LoadPriFiles(new[] { file });
	}

	private static async Task LoadPriResourcesIntoCoreResourceMapAsync(string resourcePriPath)
	{
		FileInfo resourcePriFileInfo = new(resourcePriPath);
		var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(resourcePriFileInfo.FullName);
		Windows.ApplicationModel.Resources.Core.ResourceManager.Current.LoadPriFiles(new[] { file });
	}

	#endregion

	#region Unload

	public static void UnloadPriResourcesFromCoreResourceMap(Assembly ForeignAssembly)
	{
		var foreignAssemblyDir = Path.GetDirectoryName(ForeignAssembly.Location.AssertDefined()).AssertDefined();
		var foreignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();
		UnloadPriResourcesFromCoreResourceMap(foreignAssemblyDir, foreignAssemblyName);
	}

	public static async Task UnloadPriResourcesFromCoreResourceMapAsync(Assembly ForeignAssembly)
	{
		var foreignAssemblyDir = Path.GetDirectoryName(ForeignAssembly.Location.AssertDefined()).AssertDefined();
		var foreignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();
		await UnloadPriResourcesFromCoreResourceMapAsync(foreignAssemblyDir, foreignAssemblyName);
	}

	public static void UnloadPriResourcesFromCoreResourceMap(string foreignAssemblyDir, string foreignAssemblyName)
	{
		CoreResources.TryRemove(foreignAssemblyName, out var resourcePriPath);
		if (resourcePriPath != null)
		{
			UnloadPriResourcesFromCoreResourceMap(resourcePriPath);
		}
	}

	public static async Task UnloadPriResourcesFromCoreResourceMapAsync(string foreignAssemblyDir, string foreignAssemblyName)
	{
		CoreResources.TryRemove(foreignAssemblyName, out var resourcePriPath);
		if (resourcePriPath != null)
		{
			await UnloadPriResourcesFromCoreResourceMapAsync(resourcePriPath);
		}
	}

	private static void UnloadPriResourcesFromCoreResourceMap(string resourcePriPath)
	{
		FileInfo resourcePriFileInfo = new(resourcePriPath);
		var getFileTask = Windows.Storage.StorageFile.GetFileFromPathAsync(resourcePriFileInfo.FullName).AsTask();
		getFileTask.Wait();
		var file = getFileTask.Result;
		Windows.ApplicationModel.Resources.Core.ResourceManager.Current.UnloadPriFiles(new[] { file });
	}

	private static async Task UnloadPriResourcesFromCoreResourceMapAsync(string resourcePriPath)
	{
		FileInfo resourcePriFileInfo = new(resourcePriPath);
		var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(resourcePriFileInfo.FullName);
		Windows.ApplicationModel.Resources.Core.ResourceManager.Current.UnloadPriFiles(new[] { file });
	}

	#endregion

	#endregion
}
