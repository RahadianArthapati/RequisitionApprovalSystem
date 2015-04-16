
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RQNApprovalSystem
{
//	[Activity (MainLauncher = true, Label= "@string/app_name", Icon = "@drawable/icon")]
	public class SplashscreenActivity : Activity
	{
		System.Timers.Timer t;
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_splashscreen);
			DataProvider.vInitDatabase (new cbll ());
			t = new System.Timers.Timer();
			t.Interval = 5000;
			t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
			t.Start();
		}

		protected void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			t.Stop();
			RunOnUiThread (() => {
				Intent intent = new Intent (this.ApplicationContext,typeof(LoginActivity));
				intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);
				OverridePendingTransition(Resource.Animation.abc_slide_out_top, Resource.Animation.abc_slide_out_bottom);
				StartActivity (intent);
			} );
		}
	}
}

