
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace RQNApprovalSystem
{
	public class ActionFragment : DialogFragment
	{
		private static string TAG = "Action Fragment";
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//Dialog.SetTitle (Resource.String);
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle );
			//SetPosition ();
			var view = inflater.Inflate (Resource.Layout.fragment_action, container, false);
			var approve = view.FindViewById (Resource.Id.btnApprove) as Button;
			var reject = view.FindViewById (Resource.Id.btnReject) as Button;
			var comment = view.FindViewById (Resource.Id.edComment) as EditText;
			var undo = view.FindViewById (Resource.Id.cardview_undo) as View;
			string sAction = Arguments.GetString("action");
			string sDocNumber = Arguments.GetString ("docNumber");
			string sBranch = Arguments.GetString ("branchCode");
			string sWorkflow = Arguments.GetString ("workflow");

			if (string.IsNullOrEmpty (sAction)) {
				//undo.Visibility = ViewStates.Gone;
				undo.Enabled = false;
				//undo.Background = Resources.GetDrawable(Resource.Drawable.corner_grey);
				approve.Background = Resources.GetDrawable(Resource.Drawable.corner_green_top);
				reject.Background = Resources.GetDrawable(Resource.Drawable.corner_red_bottom);
			}else if(sAction.Equals("Approved")){
				undo.Background = Resources.GetDrawable(Resource.Drawable.corner_yellow);
				approve.Background = Resources.GetDrawable(Resource.Drawable.corner_grey);
				approve.Enabled = false;
				reject.Background = Resources.GetDrawable(Resource.Drawable.corner_red_bottom);
			}else{
				undo.Background = Resources.GetDrawable(Resource.Drawable.corner_yellow);
				approve.Background = Resources.GetDrawable(Resource.Drawable.corner_green_top);
				reject.Background = Resources.GetDrawable(Resource.Drawable.corner_grey_bottom);
				reject.Enabled = false;
			}
			IAction action = this.Activity as IAction;
			approve.Click += (object sender, EventArgs e) => {
				action.setActionOnChanged(sBranch,sDocNumber, sWorkflow, comment.Text, Config.APPROVED, true);
				Dismiss();
			};
			reject.Click += (object sender, EventArgs e) => {
				action.setActionOnChanged(sBranch, sDocNumber, sWorkflow, comment.Text, Config.REJECTED, true);
				Dismiss();
			};

			return view;
		}
		private void SetPosition(){
			Window window = Dialog.Window;
			window.SetGravity (GravityFlags.Bottom | GravityFlags.Right);
			WindowManagerLayoutParams param = window.Attributes;
			param.DimAmount = 1;
			param.Flags = WindowManagerFlags.DimBehind;
			param.Alpha = 0.5f;
			param.X = 30;
			param.Y = 60;
			window.Attributes = param;
		}
	}
}

