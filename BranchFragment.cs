using Android.Support.V4.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Widget;
using System.Collections.Generic;
using Android.Util;
using System;
using Android.Content;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;

namespace RQNApprovalSystem
{
	public class BranchFragment : Fragment
	{
		private static string TAG = "BranchFragment";
		private static List<RAS.cemPendingRequisition> listRQN;
		MainBranchAdapter adapter;
		public static BranchFragment NewInstance(string branch_code)
		{
			var f = new BranchFragment ();
			var b = new Bundle ();
			b.PutString("branch_code",branch_code);
			f.Arguments = b;
			return f;
		}
		public Toolbar Toolbar {
			get;
			set;
		}
		public static void setAdapter(string branch)
		{
			try{
				var temp = RequisitionPending.getRequisitionPending ();
				Log.Debug ("BranchFragment", "Branch : " + branch);
				listRQN = temp.FindAll(x => x.BRANCHCODE == branch);

			}catch(Exception ex){
				Log.Error (TAG, ex.ToString ());
			}
		}
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			setAdapter (Arguments.GetString ("branch_code"));
		}
		public override void OnPrepareOptionsMenu(IMenu menu)
		{
			//return base.OnPrepareOptionsMenu(menu);
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.action_setting:
				//do something

				return true;
			}
			return base.OnOptionsItemSelected(item);
		}
		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var root = inflater.Inflate(Resource.Layout.fragment_list, container, false);
			Toolbar = root.FindViewById<Toolbar>(Resource.Id.toolbar);
			if (Toolbar != null) {
				Toolbar.SetBackgroundColor (Resources.GetColor (Resource.Color.pink));
				Toolbar.InflateMenu (Resource.Menu.bottom);
				//Spinner spinner = root.FindViewById <Spinner>(Resource.Id.action_setting);
				Toolbar.MenuItemClick += (sender, e) => {
				//Toast.MakeText(this.Activity, "Bottom toolbar pressed: " + e.Item.TitleFormatted, ToastLength.Short).Show();
					switch(e.Item.ItemId){
					case Resource.Id.action_sort:
							
						break;
					case Resource.Id.menuSortByPrice:
						listRQN.Sort((x,y) => x.RQNTOTVALUE.CompareTo(y.RQNTOTVALUE));
						adapter.NotifyDataSetChanged ();
							//FindAll(x => x.BRANCHCODE == branch);
						break;

					}
////					if(e.Item.TitleFormatted.Equals("Setting")){
////						var spinner = (Spinner) e.Item.ActionView;
////						ISpinnerAdapter spinnerAdapter = ArrayAdapter.CreateFromResource(this.Activity,Resource.Array.action_setting,Android.Resource.Layout.SimpleSpinnerDropDownItem);
////						spinner.Adapter  = spinnerAdapter;
////					}
				};
			}
			var lv = root.FindViewById<ListView> (Resource.Id.list);
			adapter = new MainBranchAdapter (this.Activity,Arguments.GetString ("branch_code"),listRQN);

			lv.ItemClick += OnListItemClick;
			lv.Adapter = adapter;
			//adapter.NotifyDataSetChanged ();
			ViewCompat.SetElevation(root, 50);
			return root;
		}


		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e) {
			Bundle b = new Bundle ();
			b.PutString ("docNumber", adapter.GetPendingData(e.Position).DOCNUMBER);
			b.PutString ("desc", adapter.GetPendingData(e.Position).DESCRIPTIO);
			b.PutString ("comment", adapter.GetPendingData(e.Position).COMMENT);
			b.PutString ("branchcode", adapter.GetPendingData (e.Position).BRANCHCODE);
			b.PutString ("requesteddate", adapter.GetPendingData (e.Position).REQDATE);
			b.PutString ("requireddate", adapter.GetPendingData (e.Position).RQRDDATE);
			b.PutString ("action", adapter.GetPendingData (e.Position).ACTION);
			Intent intent = new Intent (this.Activity,typeof(DetailActivity));
			intent.PutExtras (b);
			StartActivity (intent);

		}
	}
}

