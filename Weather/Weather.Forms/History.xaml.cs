using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Weather.Forms
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class History : ContentPage
	{
		public const string HistoryItemSelected = "HistoryItemSelected";

		public History()
		{
			InitializeComponent();

			HistoryItems.ItemsSource = HistoryRecorder.LocationHistory;
			HistoryItems.ItemTapped += HistoryItemsOnItemTapped;

			BindingContext = this;
		}

		public string PlatformBlurb => $"{Device.RuntimePlatform} :hearts: Xamarin.Forms";

		private void HistoryItemsOnItemTapped(object sender, ItemTappedEventArgs itemTappedEventArgs)
		{
			var historyItem = itemTappedEventArgs.Item as HistoryItem;

			if (historyItem == null)
			{
				return;
			}

			MessagingCenter.Send(this, HistoryItemSelected, historyItem.PostalCode);
		}
	}

	public class HistoryRecorder
	{
		public const string LocationSubmitted = "LocationSubmitted";

		public static HistoryRecorder Instance => _instance ?? (_instance = new HistoryRecorder());
		private static HistoryRecorder _instance;

		public HistoryRecorder()
		{
			MessagingCenter.Subscribe<HistoryRecorder, HistoryItem>(this, LocationSubmitted,
				(recorder, historyItem) => LocationHistory.Add(historyItem));
		}

		public static ObservableCollection<HistoryItem> LocationHistory = new ObservableCollection<HistoryItem>
		{
			new HistoryItem(DateTime.Now.AddHours(-4), "46901", "Howard County", "http://openweathermap.org/img/w/03d.png"),
			new HistoryItem(DateTime.Now.AddHours(-3), "47803", "Terre Haute", "http://openweathermap.org/img/w/03d.png"),
			new HistoryItem(DateTime.Now.AddHours(-2), "90210", "Beverly Hills", "http://openweathermap.org/img/w/03d.png"),
			new HistoryItem(DateTime.Now.AddHours(-1), "80021", "Broomfield", "http://openweathermap.org/img/w/03d.png"),
			new HistoryItem(DateTime.Now.AddMinutes(-42), "80203", "Denver", "http://openweathermap.org/img/w/03d.png"),
		};
	}

	public class HistoryItem
	{
		internal HistoryItem(DateTime dateTime, string postalCode, string locationName, string icon)
		{
			DateTime = dateTime;
			PostalCode = postalCode;
			LocationName = locationName;
			Icon = icon;
		}

		public HistoryItem(string postalCode, string locationName, string icon) : this(DateTime.Now, postalCode, locationName, icon)
		{
		}

		public DateTime DateTime { get; set; }
		public string PostalCode { get; set; }
		public string LocationName { get; set; }
		public string Icon { get; set; }
	}
}