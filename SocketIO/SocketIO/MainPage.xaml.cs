using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SocketIO
{
	public partial class MainPage : ContentPage
	{
        
		public MainPage()
		{
			InitializeComponent();
            socketService = new SocketService(App.SignalUrl);
		}

	    private SocketService socketService;

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
            socketService.Connect();
	    }
	}
}
