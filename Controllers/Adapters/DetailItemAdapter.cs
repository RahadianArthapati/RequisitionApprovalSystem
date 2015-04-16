using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using Android.Views;
using Android.Util;

namespace RQNApprovalSystem
{
	public class DetailItemAdapter: BaseAdapter<RAS.cemDetailRequisition>
	{
		#region implemented abstract members of BaseAdapter
		private static string TAG="DetailItemAdapter";
		Activity context;
		public List<RAS.cemDetailRequisition> listDetailItem;
		public DetailItemAdapter (Activity context, List<RAS.cemDetailRequisition> lDetail): base()
		{
			this.context = context;
			this.listDetailItem = lDetail;
		}
		public override RAS.cemDetailRequisition this [int position] {
			get { return this.listDetailItem [position]; }
		}
		public override long GetItemId (int position)
		{
			return position;
		}
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = this.listDetailItem [position];
			var view = convertView;
			if (convertView == null || !(convertView is LinearLayout))
				view = context.LayoutInflater.Inflate (Resource.Layout.list_detail_item, parent, false);
			var itemDesc  = view.FindViewById (Resource.Id.tvItemDesc) as TextView;
			var qty = view.FindViewById (Resource.Id.tvQty) as TextView;
			var unitCost = view.FindViewById (Resource.Id.tvUnitCost) as TextView;

			itemDesc.SetText (item.ITEMDESC, TextView.BufferType.Normal);
			qty.SetText (string.Format("{0} {1}",item.REQQTY, item.ORDERUNIT), TextView.BufferType.Normal);
			unitCost.SetText (string.Format("{0} {1}",item.CURRCODE,item.UNITCOST), TextView.BufferType.Normal);
			return view;
		}

		public override int Count {
			get {
				try{
					return this.listDetailItem.Count; 
				}catch(Exception ex){
					return 0;
				}
			}
		}
		public RAS.cemDetailRequisition GetDetailItem(int position){
			return this.listDetailItem [position];
		}
		#endregion


	}
}


