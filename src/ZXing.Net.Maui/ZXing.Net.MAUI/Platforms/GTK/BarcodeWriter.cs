using ZXing.Common;
using ZXing.Rendering;
using Microsoft.Maui.Platform;
using MauiColor = Microsoft.Maui.Graphics.Color;
using Gdk;
using GLib;

namespace ZXing.Net.Maui;

public class BarcodeWriter : BarcodeWriter<NativePlatformImage>, IBarcodeWriter
{
	WriteableBitmapRenderer bitmapRenderer;

	public BarcodeWriter()
		=> Renderer = (bitmapRenderer = new WriteableBitmapRenderer());

	public MauiColor ForegroundColor
	{
		get => bitmapRenderer.Foreground;
		set => bitmapRenderer.Foreground = value;
	}

	public MauiColor BackgroundColor
	{
		get => bitmapRenderer.Background;
		set => bitmapRenderer.Background = value;
	}

	internal class WriteableBitmapRenderer : IBarcodeRenderer<Pixbuf>
	{
		internal MauiColor Foreground { get; set; }
		internal MauiColor Background { get; set; }
		
		public Pixbuf Render(BitMatrix matrix, ZXing.BarcodeFormat format, string content)
			=> Render(matrix, format, content, new EncodingOptions());

		public Pixbuf Render(BitMatrix matrix, ZXing.BarcodeFormat format, string content, EncodingOptions options)
		{
			var width = matrix.Width;
			var height = matrix.Height;
			var bytes = new byte[width * height * 4];
			var outputIndex = 0;
			Foreground.ToRgba(out byte fgR, out byte fgG, out byte fgB, out byte fgA);
			Background.ToRgba(out byte bgR, out byte bgG, out byte bgB, out byte bgA);

			for (var y = 0; y < height; y++)
			{
				for (var x = 0; x < width; x++)
				{
					var isFg = matrix[x, y];
					
					bytes[outputIndex++] = isFg ? fgR : bgR;
					bytes[outputIndex++] = isFg ? fgG : bgG;
					bytes[outputIndex++] = isFg ? fgB : bgB;
					bytes[outputIndex++] = isFg ? fgA : bgA;
				}
			}
			
			return new Pixbuf(new Bytes(bytes), Colorspace.Rgb, true, 8, width, height, width*4);
		}
	}
}