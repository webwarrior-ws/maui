using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.AppCompat.App;
using Microsoft.Maui.Hosting;

using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.Activity)]
	public class ActivityTests
	{
		[Fact(DisplayName = "Application UI colors are updated on dark mode change")]
		public void UIColorsAreUpdatedOnDarkModeChange()
		{
			var mauiApp = MauiApp
				.CreateBuilder()
				.Build();

			var mauiContext = new MauiContext(mauiApp.Services, MauiProgram.DefaultContext);
			var activity = mauiContext.GetActivity();

			activity.RunOnUiThread(() =>
			{
				activity.SetTheme(Resource.Style.Theme_AppCompat_DayNight);

				// set to light mode
				activity.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);

				var rootView = activity.Window.DecorView as ViewGroup;

				Assert.True((rootView.Background as ColorDrawable).Color.GetBrightness() > 0.5);

				// set to dark mode
				activity.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);

				rootView = activity.Window.DecorView as ViewGroup;

				Assert.True((rootView.Background as ColorDrawable).Color.GetBrightness() < 0.5);
			});

		}
	}
}
