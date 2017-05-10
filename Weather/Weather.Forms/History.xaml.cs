using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Weather.Forms
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class History : ContentPage
	{
		public History()
		{
			InitializeComponent();
		}

		public const string HistoryItemSelected = "HistoryItemSelected";

		private void Button_OnClicked(object sender, EventArgs e)
		{
			MessagingCenter.Send(this, HistoryItemSelected, "46901");
		}
	}
}
