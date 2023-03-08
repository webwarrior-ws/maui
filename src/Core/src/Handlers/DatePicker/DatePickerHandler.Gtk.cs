using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Handlers
{

	public partial class DatePickerHandler : ViewHandler<IDatePicker, MauiDatePicker>
	{

		protected override MauiDatePicker CreatePlatformView()
		{
			return new MauiDatePicker();
		}

		protected override void ConnectHandler(MauiDatePicker platformView)
		{
			base.ConnectHandler(platformView);
			platformView.DateChanged += DateChanged;
		}

		protected override void DisconnectHandler(MauiDatePicker platformView)
		{
			base.DisconnectHandler(platformView);
			platformView.DateChanged -= DateChanged;
		}

		private void DateChanged(object? sender, System.EventArgs args)
		{
			VirtualView.Date = PlatformView.Date;
		}

		public static void MapFormat(IDatePickerHandler handler, IDatePicker datePicker)
		{
			handler.PlatformView?.UpdateFormat(datePicker);
		}

		public static void MapDate(IDatePickerHandler handler, IDatePicker datePicker)
		{
			handler.PlatformView?.UpdateDate(datePicker);
		}

		public static void MapMinimumDate(IDatePickerHandler handler, IDatePicker datePicker)
		{
			handler.PlatformView.MinDate = handler.VirtualView.MinimumDate;
		}

		public static void MapMaximumDate(IDatePickerHandler handler, IDatePicker datePicker)
		{
			handler.PlatformView.MaxDate = handler.VirtualView.MaximumDate;
		}

		[MissingMapper]
		public static void MapCharacterSpacing(IDatePickerHandler handler, IDatePicker datePicker) { }

		public static void MapFont(IDatePickerHandler handler, IDatePicker datePicker)
		{
			var fontManager = handler.GetRequiredService<IFontManager>();

			handler.PlatformView?.UpdateFont(datePicker, fontManager);
		}

		[MissingMapper]
		public static void MapTextColor(IDatePickerHandler handler, IDatePicker datePicker) { }

	}

}