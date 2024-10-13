using System.Collections.Concurrent;
using System.Reflection;

namespace CustomExtensions.WinUI.Extensions;

internal static class ResourceExtensions
{
	#region Win Resources

	#region Management

	internal static ConcurrentDictionary<string, Microsoft.Windows.ApplicationModel.Resources.ResourceMap> WinResourceMaps = new();

	#endregion

	#region Load

	public static void LoadPriResourcesIntoWinResourceMap(Assembly ForeignAssembly)
	{
		var foreignAssemblyDir = Path.GetDirectoryName(ForeignAssembly.Location.AssertDefined()).AssertDefined();
		var foreignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();
		LoadPriResourcesIntoWinResourceMap(foreignAssemblyDir, foreignAssemblyName);
	}

	public static void LoadPriResourcesIntoWinResourceMap(string foreignAssemblyDir, string foreignAssemblyName)
	{
		var resourcePriPath = Path.Combine(foreignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			WinResourceMaps.TryAdd(foreignAssemblyName, new Microsoft.Windows.ApplicationModel.Resources.ResourceManager(resourcePriPath).MainResourceMap);
		}
		else
		{
			resourcePriPath = Path.Combine(foreignAssemblyDir, $"{foreignAssemblyName}.pri");
			if (File.Exists(resourcePriPath))
			{
				WinResourceMaps.TryAdd(foreignAssemblyName, new Microsoft.Windows.ApplicationModel.Resources.ResourceManager(resourcePriPath).MainResourceMap);
			}
		}
	}

	#endregion

	#region Unload

	public static void UnloadPriResourcesFromWinResourceMap(Assembly ForeignAssembly)
	{
		var foreignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();
		UnloadPriResourcesFromWinResourceMap(foreignAssemblyName);
	}

	public static void UnloadPriResourcesFromWinResourceMap(string foreignAssemblyName)
	{
		WinResourceMaps.TryRemove(foreignAssemblyName, out _);
	}

	#endregion

	#endregion

	#region Core Resources

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
		var resourcePriPath = Path.Combine(foreignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			LoadPriResourcesIntoCoreResourceMap(resourcePriPath);
		}
		else
		{
			resourcePriPath = Path.Combine(foreignAssemblyDir, $"{foreignAssemblyName}.pri");
			if (File.Exists(resourcePriPath))
			{
				LoadPriResourcesIntoCoreResourceMap(resourcePriPath);
			}
		}
	}

	public static async Task LoadPriResourcesIntoCoreResourceMapAsync(string foreignAssemblyDir, string foreignAssemblyName)
	{
		var resourcePriPath = Path.Combine(foreignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			await LoadPriResourcesIntoCoreResourceMapAsync(resourcePriPath);
		}
		else
		{
			resourcePriPath = Path.Combine(foreignAssemblyDir, $"{foreignAssemblyName}.pri");
			if (File.Exists(resourcePriPath))
			{
				await LoadPriResourcesIntoCoreResourceMapAsync(resourcePriPath);
			}
		}
	}

	public static void LoadPriResourcesIntoCoreResourceMap(string resourcePriPath)
	{
		FileInfo resourcePriFileInfo = new(resourcePriPath);
		var getFileTask = Windows.Storage.StorageFile.GetFileFromPathAsync(resourcePriFileInfo.FullName).AsTask();
		getFileTask.Wait();
		var file = getFileTask.Result;
		Windows.ApplicationModel.Resources.Core.ResourceManager.Current.LoadPriFiles(new[] { file });
	}

	public static async Task LoadPriResourcesIntoCoreResourceMapAsync(string resourcePriPath)
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
		var resourcePriPath = Path.Combine(foreignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			UnloadPriResourcesFromCoreResourceMap(resourcePriPath);
		}
		else
		{
			resourcePriPath = Path.Combine(foreignAssemblyDir, $"{foreignAssemblyName}.pri");
			if (File.Exists(resourcePriPath))
			{
				UnloadPriResourcesFromCoreResourceMap(resourcePriPath);
			}
		}
	}

	public static async Task UnloadPriResourcesFromCoreResourceMapAsync(string foreignAssemblyDir, string foreignAssemblyName)
	{
		var resourcePriPath = Path.Combine(foreignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			await UnloadPriResourcesFromCoreResourceMapAsync(resourcePriPath);
		}
		else
		{
			resourcePriPath = Path.Combine(foreignAssemblyDir, $"{foreignAssemblyName}.pri");
			if (File.Exists(resourcePriPath))
			{
				await UnloadPriResourcesFromCoreResourceMapAsync(resourcePriPath);
			}
		}
	}

	public static void UnloadPriResourcesFromCoreResourceMap(string resourcePriPath)
	{
		FileInfo resourcePriFileInfo = new(resourcePriPath);
		var getFileTask = Windows.Storage.StorageFile.GetFileFromPathAsync(resourcePriFileInfo.FullName).AsTask();
		getFileTask.Wait();
		var file = getFileTask.Result;
		Windows.ApplicationModel.Resources.Core.ResourceManager.Current.UnloadPriFiles(new[] { file });
	}

	public static async Task UnloadPriResourcesFromCoreResourceMapAsync(string resourcePriPath)
	{
		FileInfo resourcePriFileInfo = new(resourcePriPath);
		var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(resourcePriFileInfo.FullName);
		Windows.ApplicationModel.Resources.Core.ResourceManager.Current.UnloadPriFiles(new[] { file });
	}

	#endregion

	#endregion
}
