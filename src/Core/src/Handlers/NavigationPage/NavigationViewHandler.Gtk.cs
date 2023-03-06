using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Handlers
{

	public partial class NavigationViewHandler : ViewHandler<IStackNavigationView, NavigationView>
	{

		protected override NavigationView CreatePlatformView()
		{
			return new NavigationView();
		}

		protected override void ConnectHandler(NavigationView nativeView)
		{
			base.ConnectHandler(nativeView);
			nativeView.Connect(VirtualView);
		}

		protected override void DisconnectHandler(NavigationView nativeView)
		{
			base.DisconnectHandler(nativeView);
			nativeView.Disconnect(VirtualView);
		}

		public static void RequestNavigation(INavigationViewHandler arg1, IStackNavigation arg2, object? arg3)
		{
			Console.WriteLine($"RequestNavigation({arg1}, {arg2}, {arg3})");
			if (arg1 is NavigationViewHandler platformHandler && arg3 is NavigationRequest ea)
				platformHandler.PlatformView?.RequestNavigation(ea);
		}
	}

}