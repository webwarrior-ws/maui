using System;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;

namespace ZXing.Net.Maui
{
	internal partial class CameraManager : IDisposable
	{
		public CameraManager(IMauiContext context, CameraLocation cameraLocation)
		{
			Context = context;
			CameraLocation = cameraLocation;
		}

		protected readonly IMauiContext Context;
// Event 'FrameReady' is never invoked (there is no camera support on Gtk)	
#pragma warning disable CS0067
		public event EventHandler<CameraFrameBufferEventArgs> FrameReady;
#pragma warning restore CS0067

		public CameraLocation CameraLocation { get; private set; }

		public void UpdateCameraLocation(CameraLocation cameraLocation)
		{
			CameraLocation = cameraLocation;

			UpdateCamera();
		}

		public async Task<bool> CheckPermissions()
			=> (await Permissions.RequestAsync<Permissions.Camera>()) == PermissionStatus.Granted;
	}
}
