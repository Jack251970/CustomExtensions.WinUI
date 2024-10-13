using System.Diagnostics;
using System.Reflection;
using Microsoft.UI.Xaml.Markup;

namespace CustomExtensions.WinUI;

internal partial class ExtensionAssembly : IExtensionAssembly
{
	public Assembly ForeignAssembly { get; }

	private readonly string ForeignAssemblyDir;
	private readonly string ForeignAssemblyName;
	private bool? IsHotReloadAvailable;
	private readonly DisposableCollection Disposables = new();
	private bool IsDisposed;

	internal ExtensionAssembly(string assemblyPath)
	{
		// Note: For some reason, AssemblyLoadContext will cause issues in WinUI application,
		// such as unable to find secondarily-referred WinUI components.
		// Even if using AssemblyLoadContext.Default which should have no difference thanAssembly.LoadFrom(), but it does.
		// So, our packaged uses Assembly.LoadFrom() instead.
		//
		// ExtensionContext = new(assemblyPath);
		// ForeignAssembly = ExtensionContext.LoadFromAssemblyPath(assemblyPath);
		// So use Assembly.LoadFrom() instead.

		ForeignAssembly = Assembly.LoadFrom(assemblyPath);
		ForeignAssemblyDir = Path.GetDirectoryName(ForeignAssembly.Location.AssertDefined()).AssertDefined();
		ForeignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();
	}

    public void LoadResources(bool loadPriResources)
    {
		if (IsDisposed)
		{
			throw new ObjectDisposedException(nameof(ExtensionAssembly));
		}

		RegisterXamlTypeMetadataProviders();

		if (loadPriResources)
		{
			LoadPriResources();
		}
    }

	public async Task LoadResourcesAsync(bool loadPriResources)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(nameof(ExtensionAssembly));
		}

		RegisterXamlTypeMetadataProviders();

		if (loadPriResources)
		{
			await LoadPriResourcesAsync();
		}
	}

	public static void LoadPriResources(Assembly ForeignAssembly)
	{
		var foreignAssemblyDir = Path.GetDirectoryName(ForeignAssembly.Location.AssertDefined()).AssertDefined();
		var foreignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();

		var resourcePriPath = Path.Combine(foreignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			LoadPriResources(resourcePriPath);
		}
		else
		{
			resourcePriPath = Path.Combine(foreignAssemblyDir, $"{foreignAssemblyName}.pri");
			if (File.Exists(resourcePriPath))
			{
				LoadPriResources(resourcePriPath);
			}
		}
	}

	public static async Task LoadPriResourcesAsync(Assembly ForeignAssembly)
	{
		var foreignAssemblyDir = Path.GetDirectoryName(ForeignAssembly.Location.AssertDefined()).AssertDefined();
		var foreignAssemblyName = ForeignAssembly.GetName().Name.AssertDefined();

		var resourcePriPath = Path.Combine(foreignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			await LoadPriResourcesAsync(resourcePriPath);
		}
		else
		{
			resourcePriPath = Path.Combine(foreignAssemblyDir, $"{foreignAssemblyName}.pri");
			if (File.Exists(resourcePriPath))
			{
				await LoadPriResourcesAsync(resourcePriPath);
			}
		}
	}

	private void RegisterXamlTypeMetadataProviders()
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(nameof(ExtensionAssembly));
		}

		Disposables.AddRange(ForeignAssembly.ExportedTypes
			.Where(type => type.IsAssignableTo(typeof(IXamlMetadataProvider)))
			.Select(metadataType => (Activator.CreateInstance(metadataType) as IXamlMetadataProvider).AssertDefined())
			.Select(ApplicationExtensionHost.Current.RegisterXamlTypeMetadataProvider));
	}

	private void LoadPriResources()
	{
		var resourcePriPath = Path.Combine(ForeignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			LoadPriResources(resourcePriPath);
		}
		else
		{
			resourcePriPath = Path.Combine(ForeignAssemblyDir, $"{ForeignAssemblyName}.pri");
			if (File.Exists(resourcePriPath))
			{
				LoadPriResources(resourcePriPath);
			}
		}
	}

	private async Task LoadPriResourcesAsync()
	{
		var resourcePriPath = Path.Combine(ForeignAssemblyDir, "resources.pri");
		if (File.Exists(resourcePriPath))
		{
			await LoadPriResourcesAsync(resourcePriPath);
		}
		else
		{
			resourcePriPath = Path.Combine(ForeignAssemblyDir, $"{ForeignAssemblyName}.pri");
			if (File.Exists(resourcePriPath))
			{
				await LoadPriResourcesAsync(resourcePriPath);
			}
		}
	}

	private static void LoadPriResources(string resourcePriPath)
	{
		FileInfo resourcePriFileInfo = new(resourcePriPath);
		var getFileTask = Windows.Storage.StorageFile.GetFileFromPathAsync(resourcePriFileInfo.FullName).AsTask();
		getFileTask.Wait();
		var file = getFileTask.Result;
		Windows.ApplicationModel.Resources.Core.ResourceManager.Current.LoadPriFiles(new[] { file });
	}

	private static async Task LoadPriResourcesAsync(string resourcePriPath)
	{
		FileInfo resourcePriFileInfo = new(resourcePriPath);
		var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(resourcePriFileInfo.FullName);
		Windows.ApplicationModel.Resources.Core.ResourceManager.Current.LoadPriFiles(new[] { file });
	}

    public bool TryEnableHotReload()
	{
		if (IsHotReloadAvailable.HasValue)
		{
            return IsHotReloadAvailable.Value;
        }

		if (!ApplicationExtensionHost.IsHotReloadEnabled)
		{
            Trace.TraceWarning("HotReload(Debug) : Hot reload is not enabled in the current environment");
            IsHotReloadAvailable = false;
            return false;
		}

		if (ForeignAssemblyDir == HostingProcessDir)
		{
			Trace.TraceWarning($"HotReload(Debug) : Output directory for {ForeignAssembly.FullName} appears to be in the same location as the application directory. HotReload may not function in this environment.");
			IsHotReloadAvailable = false;
            return false;
        }

		var targetResDir = TryLoadXamlResources();
        IsHotReloadAvailable = targetResDir != null;

		return IsHotReloadAvailable.Value;
    }

    public string? TryLoadXamlResources()
    {
        // Note: this assumes all your resources exist under the current assembly name
        // this won't be true for nested dependencies or the like, so they will need to 
        // enable the same capabilities or they may crash when using hot reload
        var assemblyResDir = Path.Combine(ForeignAssemblyDir, ForeignAssemblyName);
        if (!Directory.Exists(assemblyResDir))
        {
            Trace.TraceError($"XamlLoad(Debug) : Cannot load xaml resources for {ForeignAssembly.FullName} because {assemblyResDir} does not exist on the system");
            return null;
        }

        var targetResDir = Path.Combine(HostingProcessDir, ForeignAssemblyName);
        DirectoryInfo debugTargetResDirInfo = new(targetResDir);
        if (debugTargetResDirInfo.Exists)
        {
            if (!debugTargetResDirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
            {
                Trace.TraceError($"XamlLoad(Debug) : Cannot load xaml resources for {ForeignAssembly.FullName} because {targetResDir} already exists as a non-symbolic linked directory");
                return null;
            }
            Directory.Delete(targetResDir, recursive: true);
        }
        if (!Directory.Exists(targetResDir))
        {
            Directory.CreateSymbolicLink(targetResDir, assemblyResDir);
        }

        return targetResDir;
    }

    protected virtual void Dispose(bool disposing)
	{
		if (!IsDisposed)
		{
			if (disposing)
			{
				Disposables?.Dispose();
				// ExtensionContext?.Unload();
			}

			IsDisposed = true;
		}
	}

	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
