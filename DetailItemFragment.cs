
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
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.V4.View;

namespace RQNApprovalSystem
{
	public class DetailItemFragment : Fragment
	{
		private static string TAG = "DetailItemFragment";
		private static List<RAS.cemDetailRequisition> listRQN;
		DetailItemAdapter adapter;
		public static DetailItemFragment NewInstance()
		{
			var f = new DetailItemFragment ();
			return f;
		}
		public static void setAdapter()
		{
			try{
				listRQN = RequisitionPending.getRequisitionDetailItem ();
				//listRQN = temp.FindAll(x => x.BRANCHCODE == branch);
			}catch(Exception ex){
				Log.Error (TAG, ex.ToString ());
			}
		}
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			setAdapter ();
		}
		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var root = inflater.Inflate(Resource.Layout.fragment_detail_list, container, false);
			var lv = root.FindViewById<ListView> (Resource.Id.list);
			adapter = new DetailItemAdapter (this.Activity,listRQN);
			lv.ItemClick += OnListItemClick;
			lv.Adapter = adapter;
			//adapter.NotifyDataSetChanged ();
			//ViewCompat.SetElevation(root, 10);
			return root;
		}
		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e) {

		}
	}
}

