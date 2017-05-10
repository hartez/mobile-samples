using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Weather.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Button = Android.Widget.Button;
using Fragment = Android.App.Fragment;
using FragmentTransaction = Android.App.FragmentTransaction;
using View = Android.Views.View;

namespace WeatherApp.Droid
{
	[Activity(Label = "Sample Weather App", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : FragmentActivity
	{
		private Fragment _history;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

			FragmentTransaction ft = FragmentManager.BeginTransaction();
			ft.Replace(Resource.Id.fragment_frame_layout, new MainFragment(), "main");
			ft.Commit();
		}

		public override void OnBackPressed()
		{
			if (FragmentManager.BackStackEntryCount != 0)
			{
				FragmentManager.PopBackStack();
			}
			else
			{
				base.OnBackPressed();
			}
		}

		public void ShowAbout()
		{
			if (_history == null)
			{
				Forms.Init(this, null);
				_history = new History().CreateFragment(this);
			}

			FragmentTransaction ft = FragmentManager.BeginTransaction();

			ft.AddToBackStack(null);
			ft.Replace(Resource.Id.fragment_frame_layout, _history, "history");
			
			ft.Commit();
		}

		public class MainFragment : Fragment
		{
			private string lastPostalCode;

			public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
			{
				var view =  inflater.Inflate(Resource.Layout.MainFragment, container, false);
				Button button = view.FindViewById<Button>(Resource.Id.weatherBtn);

				button.Click += Button_Click;

				MessagingCenter.Subscribe<History, string>(this, History.HistoryItemSelected, (history, s) =>
				{
					Activity.FragmentManager.PopBackStack();
					lastPostalCode = s;
				});

				return view;
			}

			public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
			{
				inflater.Inflate(Resource.Menu.menu, menu);
				base.OnCreateOptionsMenu(menu, inflater);
			}

			public override void OnAttach(Context context)
			{
				base.OnAttach(context);
				SetHasOptionsMenu(true);
			}

			public override void OnResume()
			{
				base.OnResume();
				if (!string.IsNullOrEmpty(lastPostalCode))
				{
					SetPostalCode(lastPostalCode);
					lastPostalCode = String.Empty;
				}
			}

			public override bool OnOptionsItemSelected(IMenuItem item)
			{
				switch (item.ItemId)
				{
					case Resource.Id.history_menu_item:
						((MainActivity)Activity).ShowAbout();
						return true;
				}

				return base.OnOptionsItemSelected(item);
			}

			public async void SetPostalCode(string postalCode)
			{
				EditText zipCodeEntry = View.FindViewById<EditText>(Resource.Id.zipCodeEntry);
				zipCodeEntry.Text = postalCode;
				await GetWeather();
			}

			private async Task GetWeather()
			{
				EditText zipCodeEntry = View.FindViewById<EditText>(Resource.Id.zipCodeEntry);

				if (!String.IsNullOrEmpty(zipCodeEntry.Text))
				{
					Weather weather = await Core.GetWeather(zipCodeEntry.Text);
					if (weather != null)
					{
						View.FindViewById<TextView>(Resource.Id.locationText).Text = weather.Title;
						View.FindViewById<TextView>(Resource.Id.tempText).Text = weather.Temperature;
						View.FindViewById<TextView>(Resource.Id.windText).Text = weather.Wind;
						View.FindViewById<TextView>(Resource.Id.visibilityText).Text = weather.Visibility;
						View.FindViewById<TextView>(Resource.Id.humidityText).Text = weather.Humidity;
						View.FindViewById<TextView>(Resource.Id.sunriseText).Text = weather.Sunrise;
						View.FindViewById<TextView>(Resource.Id.sunsetText).Text = weather.Sunset;

						MessagingCenter.Send(HistoryRecorder.Instance, HistoryRecorder.LocationSubmitted, zipCodeEntry.Text);
					}
				}
			}

			private async void Button_Click(object sender, EventArgs e)
			{
				await GetWeather();
			}
		}
	}
}