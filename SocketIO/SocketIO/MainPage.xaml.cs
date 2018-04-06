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

	    public string UserToken { get; set; } = "5601ab2d299cfb5a7698566158fcb2bc350701b9ac77b6f72e52c260fab0da57";


        public MainPage()
		{
			InitializeComponent();
            socketService = new SocketService(App.SignalUrl, UserToken);
		}

	    private SocketService socketService;

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
            socketService.Connect();
	    }
	}
}
