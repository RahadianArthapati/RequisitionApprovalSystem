using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using Android.Views;
using Android.Util;

namespace RQNApprovalSystem
{
	public class DetailFlowAdapter: BaseAdapter<RAS.cemDetailFlow>
		{
			#region implemented abstract members of BaseAdapter
			private static string TAG="DetailFlowAdapter";
			Activity context;
			public List<RAS.cemDetailFlow> listDetailFlow;
			public DetailFlowAdapter (Activity context, List<RAS.cemDetailFlow> lDetail): base()
			{
				this.context = context;
				this.listDetailFlow = lDetail;
			}
			public override RAS.cemDetailFlow this [int position] {
				get { return this.listDetailFlow [position]; }
			}
			public override long GetItemId (int position)
			{
				return position;
			}
			public override View GetView (int position, View convertView, ViewGroup parent)
			{
				var item = this.listDetailFlow [position];
				var view = convertView;
				if (convertView == null || !(convertView is LinearLayout))
				view = context.LayoutInflater.Inflate (Resource.Layout.list_detail_flow, parent, false);
				var stepDesc  = view.FindViewById (Resource.Id.tvStepDesc) as TextView;
				var reqBy = view.FindViewById (Resource.Id.tvReqBy) as TextView;
				var date = view.FindViewById (Resource.Id.tvDate) as TextView;

				stepDesc.SetText (item.STEPDESC, TextView.BufferType.Normal);
				reqBy.SetText (string.Format("By : {0}",item.USERID), TextView.BufferType.Normal);
				date.SetText (string.Format("Date : {0}",item.EVENTDATE), TextView.BufferType.Normal);
				return view;
			}

			public override int Count {
				get {
					try{
						return this.listDetailFlow.Count; 
					}catch(Exception ex){
						return 0;
					}
				}
			}
			public RAS.cemDetailFlow GetDetailItem(int position){
				return this.listDetailFlow [position];
			}
			#endregion


		}
	}


