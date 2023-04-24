using System;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform.Gtk;

namespace Microsoft.Maui
{

	public static class WidgetColorExtensions
	{

		public static void SetBackgroundColor(this Gtk.Widget widget, Graphics.Color? color)
		{
			if (color == null)
				return;

			widget.SetBackgroundColor(Gtk.StateFlags.Normal, color);
		}

		public static void SetBackgroundColor(this Gtk.Widget widget, Gtk.StateType state, Graphics.Color? color)
		{
			if (color == null)
				return;

			widget.SetBackgroundColor(state.ToStateFlag(), color);
		}

		public static void SetBackgroundColor(this Gtk.Widget widget, Gtk.StateFlags state, Graphics.Color? color)
		{
			if (color == null)
				return;

			var cssFlags = state.CssState();
			var mainNode = widget.CssMainNode();
			if (cssFlags != null)
				mainNode = $"{mainNode}:{cssFlags}";
			widget.SetStyleColor(color, mainNode, "background");

		}

		public static Graphics.Color GetBackgroundColor(this Gtk.Widget widget)
		{
			return widget.GetBackgroundColor(Gtk.StateFlags.Normal);
		}

		public static Graphics.Color GetBackgroundColor(this Gtk.Widget widget, Gtk.StateType state)
		{
			return widget.GetBackgroundColor(state.ToStateFlag());
		}

		public static Graphics.Color GetBackgroundColor(this Gtk.Widget widget, Gtk.StateFlags state)
		{
#pragma warning disable 612
			return widget.StyleContext.GetBackgroundColor(state).ToColor();
#pragma warning restore 612
		}

		public static Graphics.Color GetForegroundColor(this Gtk.Widget widget)
		{
			return widget.GetForegroundColor(Gtk.StateType.Normal);
		}

		public static Graphics.Color GetForegroundColor(this Gtk.Widget widget, Gtk.StateType state)
		{
			return widget.GetForegroundColor(state.ToStateFlag());
		}

		public static Graphics.Color GetForegroundColor(this Gtk.Widget widget, Gtk.StateFlags state)
		{
			return widget.StyleContext.GetColor(state).ToColor();
		}

		public static void SetForegroundColor(this Gtk.Widget widget, Gtk.StateType state, Graphics.Color? color)
		{
			widget.SetForegroundColor(state.ToStateFlag(), color);
		}

		public static void SetStyleColor(this Gtk.Widget widget, Color? color, string mainNode, string attr, string? subNode = null)
		{
			if (color == null)
				return;

			widget.SetStyleColor(color.ToGdkRgba(), mainNode, attr, subNode);
		}

		public static void SetStyleColor(this Gtk.Widget widget, Gdk.RGBA color, string mainNode, string attr, string? subNode = null)
		{
			widget.SetStyleValueNode(color.ToString(), mainNode, attr, subNode);
		}

		public static void SetColor(this Gtk.Widget widget, Color? color, string attr, string? subNode = null)
		{
			if (color == null)
				return;

			widget.SetColor(color.ToGdkRgba(), attr, subNode);
		}

		public static void SetColor(this Gtk.Widget widget, Gdk.RGBA color, string attr, string? subNode = null)
		{
			var mainNode = widget.CssMainNode();
			widget.SetStyleColor(color, mainNode, attr, subNode);
		}

		public static void SetForegroundColor(this Gtk.Widget widget, Gtk.StateFlags state, Color? color)
		{
			if (color == null)
				return;

			var nativeColor = color.ToGdkRgba();

			if (nativeColor.Equals(widget.StyleContext.GetColor(state)))
				return;

			widget.SetColor(nativeColor, "color");
		}

		public static void SetForegroundColor(this Gtk.Widget widget, Color? color)
		{
			widget.SetForegroundColor(Gtk.StateType.Normal, color);
		}

		public static void UpdateTextColor(this Gtk.Widget widget, Graphics.Color? textColor)
		{
			if (textColor == null)
				return;

			widget.SetForegroundColor(textColor);
			// widget.SetForegroundColor(Gtk.StateFlags.Prelight, textColor);
		}

		public static Gtk.StateFlags ToStateFlag(this Gtk.StateType state)
		{
			switch (state)
			{
				case Gtk.StateType.Active:
					return Gtk.StateFlags.Active;
				case Gtk.StateType.Prelight:
					return Gtk.StateFlags.Prelight;
				case Gtk.StateType.Insensitive:
					return Gtk.StateFlags.Insensitive;
				case Gtk.StateType.Focused:
					return Gtk.StateFlags.Active;
				case Gtk.StateType.Inconsistent:
					return Gtk.StateFlags.Inconsistent;
				case Gtk.StateType.Selected:
					return Gtk.StateFlags.Selected;
			}

			return Gtk.StateFlags.Normal;
		}

		public static Color ColorFor(this Gtk.StyleContext ctx, string postfix, Gtk.StateType state)
		{
			var prefix = string.Empty;
			// see: https://developer.gnome.org/gtk3/stable/gtk-migrating-GtkStyleContext-css.html
			// examples: (see: gtk.css) selected_bg_color insensitive_bg_color base_color theme_text_color insensitive_base_color theme_unfocused_fg_color theme_unfocused_text_color theme_unfocused_bg_color

			switch (state)
			{
				case StateType.Normal:
					prefix = "theme_unfocused_";

					break;
				case StateType.Active:
					break;
				case StateType.Prelight:
					break;
				case StateType.Selected:
					prefix = "selected_";

					break;
				case StateType.Insensitive:
					prefix = "insensitive_";

					break;
				case StateType.Inconsistent:
					break;
				case StateType.Focused:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}

			if (ctx.LookupColor($"{prefix}{postfix}_color", out var col))
			{
				return col.ToColor();
			}

			ctx.LookupColor("base_color", out col);

			return col.ToColor();
		}

		public static Color Background(this Gtk.Style it, Gtk.StateType state)
		{

			return it.Context.ColorFor("bg", state);
		}

		public static Color Foreground(this Gtk.Style it, Gtk.StateType state)
		{
			return it.Context.ColorFor("fg", state);

		}

		public static Color Base(this Gtk.Style it, Gtk.StateType state)
		{
			return it.Context.ColorFor("", state);
		}

	}

}