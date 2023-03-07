using System;
using System.Linq;

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
			if (pageWidget != null)
				this.Remove(pageWidget);
			var page = request.NavigationStack.Last(); // is stack top first or last?
			pageWidget = page.ToPlatform(mauiContext!);
			this.Add(pageWidget);
		}

	}

}