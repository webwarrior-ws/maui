using System;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;

using Gtk;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace ZXing.Net.Maui
{
	internal partial class CameraManager
	{
		Frame cameraPreview;

		public NativePlatformCameraPreviewView CreateNativeView()
		{
			if(cameraPreview is not null)
				return cameraPreview;
			
			cameraPreview = new Frame();
			cameraPreview.SetBackgroundColor(Colors.Black);
			var textWidget = new Label();
			textWidget.Text = "NOT SUPPORTED";
			textWidget.SetForegroundColor(Colors.White);
			cameraPreview.Add(textWidget);
			return cameraPreview;
		}

		public void Connect()
			=> LogUnsupported();

		public void Disconnect()
			=> LogUnsupported();

		public void UpdateCamera()
			=> LogUnsupported();

		public void UpdateTorch(bool on)
			=> LogUnsupported();

		public void Focus(Microsoft.Maui.Graphics.Point point)
			=> LogUnsupported();

		public void AutoFocus()
			=> LogUnsupported();

		public void Dispose()
			=> LogUnsupported();

		void LogUnsupported()
			=> Debug.WriteLine("Camera preview is not supported on this platform.");
	}
}
