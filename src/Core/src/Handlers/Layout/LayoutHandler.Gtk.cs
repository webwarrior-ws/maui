﻿using System;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Handlers
{

	public partial class LayoutHandler : ViewHandler<ILayout, LayoutView>
	{

		protected override LayoutView CreatePlatformView()
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a {nameof(LayoutView)}");
			}

			return new LayoutView
			{
				CrossPlatformVirtualView = () => VirtualView,

			};
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			PlatformView.CrossPlatformVirtualView = () => VirtualView;

			PlatformView.ClearChildren();

			foreach (var child in VirtualView)
			{
				if (child.ToPlatform(MauiContext) is { } nativeChild)
					PlatformView.Add(child, nativeChild);

			}

			PlatformView.QueueAllocate();
			PlatformView.QueueResize();
		}

		protected override void DisconnectHandler(LayoutView nativeView)
		{
			base.DisconnectHandler(nativeView);
			PlatformView.ClearChildren();
		}

		public void Add(IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			if (child.ToPlatform(MauiContext) is { } nativeChild)
				PlatformView.Add(child, nativeChild);

			PlatformView.QueueAllocate();
		}

		public void Remove(IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			if (child.ToPlatform(MauiContext) is { } nativeChild)
				PlatformView.Remove(nativeChild);

			PlatformView.QueueAllocate();
		}

		public void Clear()
		{
			PlatformView?.ClearChildren();
		}

		public void Insert(int index, IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			if (child.ToPlatform(MauiContext) is { } nativeChild)
				PlatformView.Insert(child, nativeChild, index);

			PlatformView.QueueAllocate();

		}

		public void Update(int index, IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			if (child.ToPlatform(MauiContext) is { } nativeChild)
				PlatformView.Update(child, nativeChild, index);

			PlatformView.QueueAllocate();

		}

#if DEBUG
		public override void PlatformArrange(Rect rect)
		{
			PlatformView?.Arrange(rect);
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (PlatformView is not { } nativeView)
				return Size.Zero;

			return nativeView.GetDesiredSize(widthConstraint, heightConstraint);
		}

#endif

		private void UpdateVisibility(Visibility visibility)
		{
			if (visibility == Visibility.Visible)
				PlatformView.Show();
			else
				PlatformView.Hide();
		}

		[MissingMapper]
		public void UpdateZIndex(IView view) => throw new NotImplementedException();

		static void MapVisibility(ILayoutHandler handler, ILayout layout)
		{
			((LayoutHandler)handler).UpdateVisibility(layout.Visibility);
		}
	}

}
