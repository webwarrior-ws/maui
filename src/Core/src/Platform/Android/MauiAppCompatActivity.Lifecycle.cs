using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Microsoft.Maui.Devices;
using Microsoft.Maui.LifecycleEvents;

namespace Microsoft.Maui
{
	public partial class MauiAppCompatActivity
	{
		private UiMode currentNightMode = UiMode.NightUndefined;

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			ActivityResultCallbackRegistry.InvokeCallback(requestCode, resultCode, data);
			MauiApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnActivityResult>(del => del(this, requestCode, resultCode, data));
		}

		// TODO: Investigate whether the new AndroidX way is actually useful:
		//       https://developer.android.com/reference/android/app/Activity#onBackPressed()
		[Obsolete]
#pragma warning disable 809
		public override void OnBackPressed()
#pragma warning restore 809
		{
			var preventBackPropagation = false;
			MauiApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnBackPressed>(del =>
			{
				preventBackPropagation = del(this) || preventBackPropagation;
			});

			if (!preventBackPropagation)
#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1422 // Validate platform compatibility
				base.OnBackPressed();
#pragma warning restore CA1422 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
		}

		public override void OnConfigurationChanged(Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);

			MauiApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnConfigurationChanged>(del => del(this, newConfig));

			var newNightMode = newConfig.UiMode & UiMode.NightMask;
			if (newNightMode != currentNightMode)
			{
				Recreate();
				currentNightMode = newNightMode;
			}
		}

		protected override void OnNewIntent(Intent? intent)
		{
			base.OnNewIntent(intent);

			MauiApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnNewIntent>(del => del(this, intent));
		}

		protected override void OnPostCreate(Bundle? savedInstanceState)
		{
			base.OnPostCreate(savedInstanceState);

			MauiApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnPostCreate>(del => del(this, savedInstanceState));
		}

		protected override void OnPostResume()
		{
			base.OnPostResume();

			MauiApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnPostResume>(del => del(this));
		}

		protected override void OnRestart()
		{
			base.OnRestart();

			MauiApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnRestart>(del => del(this));
		}

		[System.Runtime.Versioning.SupportedOSPlatform("android23.0")]
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			MauiApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnRequestPermissionsResult>(del => del(this, requestCode, permissions, grantResults));

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected override void OnRestoreInstanceState(Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState(savedInstanceState);

			MauiApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnRestoreInstanceState>(del => del(this, savedInstanceState));
		}
	}
}