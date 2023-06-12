using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.AppCompat.App;
using Microsoft.Maui.ApplicationModel;

using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.Activity)]
	public class ActivityTests
	{
		[Fact(DisplayName = "Application UI colors are updated on dark mode change")]
		public async Task UIColorsAreUpdatedOnDarkModeChange()
		{
			var activity = MauiProgram.DefaultContext as AppCompatActivity;

			Android.Graphics.Color? bgColorInLightMode = null, bgColorInDarkMode = null;
			
			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				activity.SetTheme(Resource.Style.Theme_AppCompat_DayNight);
				// light mode
				activity.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
			});

			await MainThread.InvokeOnMainThreadAsync(() => 
			{ 
				var rootView = activity.Window.DecorView.RootView as ViewGroup;
				bgColorInLightMode = (rootView.Background as ColorDrawable).Color;
				// dark mode
				activity.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);
			});

			await MainThread.InvokeOnMainThreadAsync(() =>
			{
				var rootView = activity.Window.DecorView.RootView as ViewGroup;
				bgColorInDarkMode = (rootView.Background as ColorDrawable).Color;
			});

			Assert.True(bgColorInLightMode?.GetBrightness() > 0.5);
			Assert.True(bgColorInDarkMode?.GetBrightness() < 0.5);
		}
	}
}
