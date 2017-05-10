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
		}

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
			MessagingCenter.Subscribe<HistoryRecorder, string>(this, LocationSubmitted,
				(recorder, postalCode) => LocationHistory.Add(new HistoryItem { DateTime = DateTime.Now, PostalCode = postalCode }));
		}

		public static ObservableCollection<HistoryItem> LocationHistory = new ObservableCollection<HistoryItem>
		{
			new HistoryItem {DateTime = DateTime.Now.AddHours(-4), PostalCode = "46901"},
			new HistoryItem {DateTime = DateTime.Now.AddHours(-3), PostalCode = "47803"},
			new HistoryItem {DateTime = DateTime.Now.AddHours(-2), PostalCode = "90210"},
			new HistoryItem {DateTime = DateTime.Now.AddHours(-1), PostalCode = "80021"},
			new HistoryItem {DateTime = DateTime.Now.AddMinutes(-25), PostalCode = "34534"}
		};
	}

	public class HistoryItem
	{
		public DateTime DateTime { get; set; }
		public string PostalCode { get; set; }
	}
}