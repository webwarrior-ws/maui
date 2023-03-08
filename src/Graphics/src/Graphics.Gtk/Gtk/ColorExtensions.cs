using System;

namespace Microsoft.Maui.Graphics.Platform.Gtk;

public static class ColorExtensions
{

	public static Gdk.RGBA ToGdkRgba(this Color color)
		=> color == default ? default : new Gdk.RGBA { Red = color.Red, Green = color.Green, Blue = color.Blue, Alpha = color.Alpha };

	public static Color ToColor(this Gdk.RGBA color)
		=> new Color((float)color.Red, (float)color.Green, (float)color.Blue, (float)color.Alpha);

	public static Gdk.Color ToGdkColor(this Color color)
	{
		string hex = color.ToRgbaHex();
		Gdk.Color gtkColor = new Gdk.Color();
		// error CS0612: 'Color.Parse(string, ref Color)' is obsolete
#pragma warning disable 612
		Gdk.Color.Parse(hex, ref gtkColor);
#pragma warning restore 612

		return gtkColor;
	}

	public static Cairo.Color ToCairoColor(this Color color)
		=> color == default ? default : new Cairo.Color(color.Red, color.Green, color.Blue, color.Alpha);

	public static Cairo.Color ToCairoColor(this Gdk.RGBA color)
		=> new Cairo.Color(color.Red, color.Green, color.Blue, color.Alpha);

	public static Color ToColor(this Cairo.Color color)
		=> new Color((float)color.R, (float)color.G, (float)color.B, (float)color.A);

}