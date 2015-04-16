
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
using Android.Util;
using System.IO;

namespace RQNApprovalSystem
{
	public class MainBranchAdapter : BaseAdapter<RAS.cemPendingRequisition>
	{
		#region implemented abstract members of BaseAdapter
		private static string TAG="MainBranchAdapter";
		Activity context;
		string branch;
		public List<RAS.cemPendingRequisition> listPending;
		public MainBranchAdapter (Activity context,string branch_code,List<RAS.cemPendingRequisition> lPending): base()
		{
			this.context = context;
			this.listPending = lPending;
			this.branch = branch_code;
		}
		public override RAS.cemPendingRequisition this [int position] {
			get { return this.listPending [position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = this.listPending [position];
			var view = convertView;
			if (convertView == null || !(convertView is LinearLayout))
				view = context.LayoutInflater.Inflate (Resource.Layout.list_main_item, parent, false);
			var docNumber = view.FindViewById (Resource.Id.tvDocNumber) as TextView;
			var rqnVal = view.FindViewById (Resource.Id.tvRQNValue) as TextView;
			var desc = view.FindViewById (Resource.Id.tvDesc) as TextView;
			var reqBy = view.FindViewById (Resource.Id.tvReqBy) as TextView;
			var actionView = view.FindViewById (Resource.Id.vAction) as LinearLayout;
			docNumber.SetText (item.DOCNUMBER, TextView.BufferType.Normal);
			desc.SetText (item.DESCRIPTIO, TextView.BufferType.Normal);
			rqnVal.SetText (string.Format("{0} {1}",item.RQNCURRCODE,item.RQNTOTVALUE), TextView.BufferType.Normal);
			reqBy.SetText (string.Format("Requested by : {0}",(string.IsNullOrEmpty(item.REQRNAME)) ? "-" : item.REQRNAME), TextView.BufferType.Normal);

			if (string.IsNullOrEmpty (item.ACTION)) {
				actionView.SetBackgroundResource (Resource.Drawable.corner_yellow_right);
			} else {
				if (item.ACTION.Contains ("1")) {
					actionView.SetBackgroundResource (Resource.Drawable.corner_green_right);
				} else if (item.ACTION.Contains ("0")) {
					actionView.SetBackgroundResource (Resource.Drawable.corner_red_right);
				} else {

				}
			}

			actionView.Click += (object sender, EventArgs e) => {
				Log.Debug(TAG,"Action view clicked! Doc number: "+item.DOCNUMBER);
				IAction action = context as IAction;
				action.setActionOnChanged(branch,item.DOCNUMBER,"","",item.ACTION, false);
			};
			return view;
		}

		public override int Count {
			get {
				try{
					return this.listPending.Count; 
				}catch(Exception ex){
					return 0;
				}
			}
		}
		public RAS.cemPendingRequisition GetPendingData(int position){
			return this.listPending [position];
		}
		#endregion


	}
}

