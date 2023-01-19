using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;

namespace Maui.SimpleSampleApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		void InitializeComponent()
		{
			string xamlText = File.ReadAllText("SimpleSampleApp/MainPage.xaml");
			ContentPage page = new ContentPage().LoadFromXaml(xamlText);

			Content = page.Content;
		}
	}
}