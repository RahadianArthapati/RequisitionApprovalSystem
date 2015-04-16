
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.OS;
using Android.App;
using Android.Net;
using Android.Util;
using Android.Net.Wifi;
using Android.Content;
using Android.Widget;
using System.Net;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Threading.Tasks;


namespace RQNApprovalSystem
{
	public abstract class BaseActivity : ActionBarActivity
	{
		private static string TAG = "Base Activity";
		protected RAS.Service1 svc = new RAS.Service1();
		protected ProgressDialog progress;
		private int actionType = 0;
		public Toolbar Toolbar {
			get;
			set;
		}
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (LayoutResource);
			Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			if (Toolbar != null) {
				SetSupportActionBar(Toolbar);
				//Toolbar.InflateMenu (Resource.Menu.main);
				switch(actionType){
				case 0:
					SupportActionBar.SetDisplayHomeAsUpEnabled (false);
					SupportActionBar.SetHomeButtonEnabled (false);
					SupportActionBar.SetLogo (Resource.Drawable.logo);
					SupportActionBar.SetTitle (0);
					break;
				case 1:
					SupportActionBar.SetDisplayHomeAsUpEnabled (true);
					SupportActionBar.SetHomeButtonEnabled (true);
					SupportActionBar.SetTitle (0);
					break;
				}
			}
		}
		protected void setActionType(int type){
			actionType = type;
		}
		protected abstract int LayoutResource{
			get;
		}

		protected int ActionBarIcon {
			set{ Toolbar.SetNavigationIcon (value); }
		}
		protected void showProgress(string message){
			progress = new ProgressDialog(this);
			progress.Indeterminate = true;
			progress.SetProgressStyle(ProgressDialogStyle.Spinner);
			progress.SetMessage(message);
			progress.SetCancelable(false);
			RunOnUiThread (() => {
				progress.Show();
			} );
		}
		protected void showToast(string message){
			RunOnUiThread(() => Toast.MakeText(this, message, ToastLength.Short).Show());
		}
		protected void NetworkException(Activity activity,System.Net.WebException ex){
			if (ex.Status == WebExceptionStatus.ProtocolError)
			{
				var response = ex.Response as HttpWebResponse;
				if (response != null)
				{
					Console.WriteLine("HTTP Status Code: " + (int)response.StatusCode);
					Log.Info (activity.LocalClassName.ToString (), response.StatusCode.ToString ());
				}
				else
				{
					// no http status code available
				}
			}
			else
			{
				// no http status code available
			}
		}
		protected bool checkConnection(Activity activity){
			bool result = false;
			try{
				var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
				var activeConnection = connectivityManager.ActiveNetworkInfo;
				if ((activeConnection != null)  && activeConnection.IsConnected)
				{
					result = true;
					// we are connected to a network.
					//					var mobileState = connectivityManager.GetNetworkInfo(ConnectivityType.Mobile).GetState();
					//					if (mobileState == NetworkInfo.State.Connected)
					//					{
					//						// We are connected via WiFi
					//						result = true;
					//					}
				}
			}catch(Exception ex){
				Log.Info (activity.LocalClassName.ToString (), ex.ToString ());
			}
			return result;
		}
		protected void showAlert(string title, string message){
			//set alert for executing the task
			AlertDialog.Builder alert = new AlertDialog.Builder (this);
			alert.SetTitle (title);
			alert.SetMessage (message);
			alert.SetNeutralButton ("OK", (senderAlert, args) => {
				//change value write your own set of instructions
				//you can also create an event for the same in xamarin
				//instead of writing things here
				RunOnUiThread (() => {
					alert.Dispose();
				} );
			} );
			//run the alert in UI thread to display in the screen
			RunOnUiThread (() => {
				alert.Show();
			} );
		}
		private void toggleWiFi(Boolean status) {
			WifiManager wifiManager = (WifiManager)this.GetSystemService (Context.WifiService);
			if (status == true && !wifiManager.IsWifiEnabled) {
				wifiManager.SetWifiEnabled (true);
			} else if (status == false && wifiManager.IsWifiEnabled) {
				wifiManager.SetWifiEnabled (false);
			}
		}
		public void resetConnection(){
			toggleWiFi (false);
			toggleWiFi (true);
		}
		protected async Task InsertRequisitionData(string sBranch, string sDocNumber, string sWorkflow, string sComment, string sAction){
			await Task.Run (() => {
				string message = "";
				try{
					if(checkConnection(this)){
						message = svc.vInsertRequisition (sBranch, sDocNumber, sAction, DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
						showToast(message);
					}else{
						RunOnUiThread (() => {
							showAlert("Error","Please check your network connection.");
						} );
					}
				}
				catch(Exception ex){ 
					RunOnUiThread (() => {
						showAlert("Error","Please try again.");
					} );
					Log.Error(TAG,ex.ToString());
				}
			});
		}
//		public override void OnBackPressed ()
//		{
//			switch(actionType){
//			case 0:
//			
//				break;
//			case 1:
//			
//				break;
//			}
//
//		}
	}
}

