using System;
using System.Linq;
using Gtk;

namespace Microsoft.Maui.Platform
{

	public class NavigationView : Gtk.Box
	{
		Gtk.Widget? pageWidget = null;
		IMauiContext? mauiContext = null;
		
		public NavigationView() : base(Gtk.Orientation.Horizontal, 0) { }

		public void Connect(IStackNavigationView virtualView)
		{
			mauiContext = virtualView.Handler?.MauiContext;
		}

		public void Disconnect(IStackNavigationView virtualView)
		{
		}

		public void RequestNavigation(NavigationRequest request)
		{
			var page = request.NavigationStack.Last(); // stack top is last
			var newPageWidget = page.ToPlatform(mauiContext!);
			if (pageWidget == null)
			{
				this.PackStart(newPageWidget, true, true, 0);
			}
			else
			{
				this.Remove(pageWidget);
				this.Add(newPageWidget);
				this.SetChildPacking(newPageWidget, true, true, 0, PackType.Start);
			}
			pageWidget = newPageWidget;
		}

	}

}