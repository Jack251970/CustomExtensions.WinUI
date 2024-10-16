﻿using System;
using System.IO;
using System.Reflection;
using CustomExtensions.WinUI.Contracts;
using CustomExtensions.WinUI.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SampleApp.Extensibility;

namespace SampleApp.Models;

public partial class ExtensionModel : DependencyObject, IDisposable
{
	public readonly string ExtensionPath;

	public IExtensionAssembly? ExtensionAssembly { get; private set; }

	public IExtension? Instance { get; private set; }

	public string DisplayName { get; }

	public bool IsLoaded
	{
		get { return (bool)GetValue(IsLoadedProperty); }
		set { SetValue(IsLoadedProperty, value); }
	}

	public Symbol Icon
	{
		get { return (Symbol)GetValue(IconProperty); }
		set { SetValue(IconProperty, value); }
	}

	public static readonly DependencyProperty IsLoadedProperty =
		DependencyProperty.Register(nameof(IsLoaded), typeof(bool), typeof(ExtensionModel), new PropertyMetadata(false));

	public static readonly DependencyProperty IconProperty =
		DependencyProperty.Register(nameof(Icon), typeof(Symbol), typeof(ExtensionModel), new PropertyMetadata(Symbol.Page2));

	public ExtensionModel(string extensionPath)
	{
		ExtensionPath = extensionPath;
		DisplayName = Path.GetFileNameWithoutExtension(extensionPath);
	}

	public void Load()
	{
		if (!IsLoaded)
		{
			// Method 1: Load the Xaml files when the extension is loading.
			Instance = LoadMyExtensionAndCreateInstance(ExtensionPath, true);

			// Method 2: Load the Xaml files every time when they are needed.
			//Instance = LoadMyExtensionAndCreateInstance(ExtensionPath, false);

			Icon = Symbol.Page;

			IsLoaded = true;
		}
	}

	IExtension? LoadMyExtensionAndCreateInstance(string assemblyLoadPath, bool loadXamlResources, bool loadPriResourcesIntoWinResourceMap = false, bool loadPriResourcesIntoCoreResourceMap = false)
	{
		// save off the handle so we can clean up our registration with the hosting process later if desired.
		ExtensionAssembly = ApplicationExtensionHost.Current.LoadExtension(assemblyLoadPath, loadPriResourcesIntoWinResourceMap, loadPriResourcesIntoCoreResourceMap);

		// load xaml files when the extension is loading
		if (loadXamlResources)
		{
			// resourceFolder is the symbolic path to the resource folder in the host project directory.
			string? resourceFolder = ExtensionAssembly.TryLoadXamlResources();
		}

		// get the actual assembly object
		Assembly assembly = ExtensionAssembly.ForeignAssembly;

		// get the type of the extension
		Type? type = ApplicationExtensionHost.Current.FromAssemblyGetTypeOfInterface(assembly, typeof(IExtension));

		// create an instance of the extension
		IExtension? extension = Activator.CreateInstance(type) as IExtension;

		return extension;
	}

	private bool isDisposed;

	protected virtual void Dispose(bool disposing)
	{
		if (!isDisposed)
		{
			if (disposing)
			{
				ExtensionAssembly?.Dispose();
			}

			isDisposed = true;
		}
	}

	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
