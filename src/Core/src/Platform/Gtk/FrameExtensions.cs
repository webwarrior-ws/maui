using System;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform.Gtk;

namespace Microsoft.Maui.Platform;

public static class FrameExtensions
{
	// https://www.w3.org/TR/css-backgrounds-3/

	static (string mainNode, string subNode)? _borderCssNode = default;

	public static (string mainNode, string subNode) BorderCssNode(this Gtk.Container platformView)
	{
		if (_borderCssNode is { })
			return _borderCssNode.Value;

		var mainNode = platformView.CssMainNode();

		_borderCssNode = (mainNode, $"border,\n.{mainNode}");

		return _borderCssNode.Value;
	}

	public static void UpdateBorderColor(this Gtk.Container platformView, Color? color)
	{
		platformView.SetStyleValueNode($"1px solid {color.ToGdkRgba().ToString()}", platformView.CssMainNode(), "border");
	}

	public static void UpdateBorderWidth(this Gtk.Container platformView, int width)
	{
		var (mainNode, subNode) = platformView.BorderCssNode();
		platformView.SetStyleValueNode($"{width}px", mainNode, "border-width");
	}

	public static void UpdateDashPattern(this Gtk.Container platformView, float[]? strokeDashPattern)
	{
		var (mainNode, subNode) = platformView.BorderCssNode();

		if (strokeDashPattern is { Length: > 1 })
		{
			if (strokeDashPattern[0] > 1)
				platformView.SetStyleValueNode($"{BorderStyle.Dashed}", mainNode, "border-style", subNode);
			else
				platformView.SetStyleValueNode($"{BorderStyle.Dotted}", mainNode, "border-style", subNode);
		}
		else
		{
			platformView.SetStyleValueNode($"{BorderStyle.Solid}", mainNode, "border-style", subNode);
		}

	}

	// border-top-left-radius, border-top-right-radius, border-bottom-right-radius, border-bottom-left-radius
	public static void UpdateBorderRadius(this Gtk.Container platformView, float radius, float? tr = default, float? br = default, float? bl = default)
	{
		int Clamp(float r) => (int)(r < 0 ? 0 : r);

		var (mainNode, subNode) = platformView.BorderCssNode();
		if (tr is not { } || br is not { } || bl is not { } ||
			(radius == tr && radius == br && radius == bl))
		{
			platformView.SetStyleValueNode($"{Clamp(radius)}px", mainNode, "border-radius");
			return;
		}

		platformView.SetStyleValueNode($"{Clamp(radius)}px", mainNode, "border-top-left-radius");
		platformView.SetStyleValueNode($"{Clamp(tr.Value)}px", mainNode, "border-top-right-radius");
		platformView.SetStyleValueNode($"{Clamp(br.Value)}px", mainNode, "border-bottom-right-radius");
		platformView.SetStyleValueNode($"{Clamp(bl.Value)}px", mainNode, "border-bottom-left-radius");

		// no effect:
		platformView.SetStyleValueNode("padding-box", mainNode, "background-clip");
	}


	// radius can be calculated from path
	static (PointF from, PointF to)[] Corners(IRoundRectangle shape, Rect bounds)
	{
		using var path = shape.PathForBounds(bounds);

		var corners = new (PointF from, PointF to)[4];
		var iCorner = 0;
		PointF fromP = default;
		PointF toP = default;
		for (var i = 0; i < path.OperationCount; i++)
		{
			var type = path.GetSegmentType(i);
			var points = path.GetPointsForSegment(i);

			if (type == PathOperation.Cubic)
			{
				toP = points[2];
			}

			if (type == PathOperation.Move)
			{
				fromP = points[0];
			}

			if (type == PathOperation.Line)
			{
				corners[iCorner] = (fromP, toP);
				iCorner++;
				if (iCorner > corners.Length)
					break;
				fromP = points[0];
			}
		}

		corners[iCorner] = (fromP, toP);

		return corners;
	}

	static float[] Radii((PointF from, PointF to)[] corners)
	{
		float cornerOffset(int i) => Math.Abs(corners[i].to.X - corners[i].from.X);

		return [cornerOffset(0), cornerOffset(1), cornerOffset(2), cornerOffset(3)];
	}

	public static void UpdateShape(this Gtk.Container platformView, IShape? shape)
	{
		if (platformView is not { })
			return;

		if (shape is { })
		{
			if (shape is IRoundRectangle roundRectangle)
			{
				var radii = Radii(Corners(roundRectangle, platformView.Allocation.ToRect()));

				platformView.UpdateBorderRadius(radii[0], radii[1], radii[2], radii[3]);
			}
			else
			{
				platformView.UpdateBorderRadius(0);
			}
		}
	}
}