using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using WinRT;

namespace CustomExtensions.WinUI.Extensions;

public static class WinUIExtensions
{
	/// <summary>
	/// Initialize the component and try to infer the correct path to the Xaml file based on the `CallerFilePath` attribute.
	/// </summary>
	/// <typeparam name="T">The type of the component to load.</typeparam>
	/// <param name="component">The component to load.</param>
	/// <param name="contentLoaded">Whether the content has been loaded.</param>
	/// <param name="callerFilePath">The path to the file that called this method.</param>
	public static void LoadComponent<T>(this T component, ref bool contentLoaded, [CallerFilePath] string callerFilePath = "") where T : IWinRTObject
    {
        if (contentLoaded)
        {
            return;
        }

        contentLoaded = true;

        var resourceLocator = ApplicationExtensionHost.Current.LocateResource(component, callerFilePath);
        Application.LoadComponent(component, resourceLocator, ComponentResourceLocation.Nested);
    }
}
