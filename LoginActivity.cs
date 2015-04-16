using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Graphics.Drawables;
using com.refractored;
using Android.Support.V4.View;
using Android.Util;
using Android.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;

namespace RQNApprovalSystem
{
	[Activity (MainLauncher = true, Label= "@string/app_name", Icon = "@drawable/icon")]

	public class LoginActivity : BaseActivity
	{
		private static string TAG = "Login Activity";

		protected override int LayoutResource {
			get {
				setActionType (0);
				return Resource.Layout.activity_login;
			}
		}

		private Drawable oldBackground = null;
		private int currentColor;
		Login login = new Login();
		EditText etPassword, etUsername;
		CheckBox cbAutoLogin;
		Button btLogin;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Create your application here

			etUsername = FindViewById<EditText> (Resource.Id.edUsername);
			etPassword = FindViewById<EditText> (Resource.Id.edPassword);
			cbAutoLogin = FindViewById<CheckBox> (Resource.Id.cbAutoLogin);
			ChangeColor (Resources.GetColor (Resource.Color.blue));
			btLogin = FindViewById<Button> (Resource.Id.btnLogin);
			btLogin.Click += OnLogin;
		}
		private void ChangeColor(Color newColor) {
			// change ActionBar color just if an ActionBar is available
			Drawable colorDrawable = new ColorDrawable(newColor);
			Drawable bottomDrawable = new ColorDrawable(Resources.GetColor(Android.Resource.Color.Transparent));
			LayerDrawable ld = new LayerDrawable(new Drawable[]{colorDrawable, bottomDrawable});
			if (oldBackground == null) {
				SupportActionBar.SetBackgroundDrawable(ld);
			} else {
				TransitionDrawable td = new TransitionDrawable(new Drawable[]{oldBackground, ld});
				SupportActionBar.SetBackgroundDrawable(td);
				td.StartTransition(200);
			}
			oldBackground = ld;
			currentColor = newColor;
		}
		protected override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			outState.PutInt ("currentColor", currentColor);
		}

		protected override void OnRestoreInstanceState (Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState (savedInstanceState);
			currentColor = savedInstanceState.GetInt ("currentColor");
			ChangeColor (new Color (currentColor));
		}
		async void OnLogin (object sender, EventArgs e)
		{
			try
			{	
				if(!checkConnection(this)){
					showAlert("Error","Please check your internet connection");
					return;
				}
				if(string.IsNullOrEmpty(etUsername.Text)){
					showAlert("Warning","Username must not be empty");
					return;
				}
				if(string.IsNullOrEmpty(etPassword.Text)){
					showAlert("Warning","Password must not be empty");
					return;
				}
				showProgress("Loading data. Please wait...");
				if(await login.ValidateUser(etUsername.Text, etPassword.Text, cbAutoLogin.Checked)){
					progress.Hide();
					RunOnUiThread (() => {
						Intent intent = new Intent (this.ApplicationContext,typeof(MainActivity));
						intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);
						OverridePendingTransition(Resource.Animation.abc_slide_out_top, Resource.Animation.abc_slide_out_bottom);
						StartActivity (intent);
					} );
				}else{
					progress.Hide();
					showAlert("Error","Username or password is incorrect. Please try again.");
				}
			}
			catch (Exception ex)
			{
				Log.Error(TAG,ex.ToString());
			}
		}

	}
}

