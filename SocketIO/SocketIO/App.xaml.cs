using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SocketIO
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			MainPage = new SocketIO.MainPage();
		}

	    internal static string SignalUrl { get; set; } = "wss://wss.zdorov.li:9092";

	    protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
