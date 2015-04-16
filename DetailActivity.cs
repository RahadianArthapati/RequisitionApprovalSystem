
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
using Android.Support.V4.View;
using com.refractored;
using Android.Util;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.V4.App;
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Threading.Tasks;

namespace RQNApprovalSystem
{
	[Activity (Label = "Detail")]			
	public class DetailActivity : BaseActivity, IAction
	{
		private static string TAG = "DetailActivity";
		private List<RAS.cemDetailRequisition> lDetailItem = new List<RAS.cemDetailRequisition>();
		private List<RAS.cemDetailFlow> lDetailFlow = new List<RAS.cemDetailFlow>();
		private Drawable oldBackground = null;
		private int currentColor;

		protected override int LayoutResource {
			get {
				setActionType (1);
				return Resource.Layout.activity_detail;
			}
		}


		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Create your application here
			var desc = FindViewById (Resource.Id.tvDesc) as TextView;
			var requestedDate = FindViewById (Resource.Id.tvRequestedDate) as TextView;
			var requiredDate = FindViewById (Resource.Id.tvRequiredDate) as TextView;
			var remarks = FindViewById (Resource.Id.tvRemarks) as TextView;
			var actionView = FindViewById (Resource.Id.vAction) as View;
			//var docNum = this.Intent.Extras.GetString("docNumber");
			var docNum = "RQN0001802";
			var action = this.Intent.Extras.GetString ("action");
			Toolbar.Title = docNum;
			ChangeColor (Resources.GetColor (Resource.Color.blue));

			var sDateRequired = this.Intent.Extras.GetString ("requireddate");
			var sDateRequested = this.Intent.Extras.GetString ("requesteddate");
			var sAction = this.Intent.Extras.GetString ("action");

			desc.SetText (this.Intent.Extras.GetString("desc"), TextView.BufferType.Normal);
			requestedDate.SetText (string.Format("Date requested : {0}",Utils.GetFormattedDate(sDateRequested)), TextView.BufferType.Normal);
			requiredDate.SetText (string.Format("Date required : {0}",Utils.GetFormattedDate(sDateRequired)), TextView.BufferType.Normal);
			remarks.SetText (string.Format("Remarks : {0}",this.Intent.Extras.GetString("comment")), TextView.BufferType.Normal);
			if (string.IsNullOrEmpty (action)) {
				actionView.SetBackgroundResource (Resource.Drawable.corner_yellow_right);
			} else {
				actionView.SetBackgroundResource (Resource.Drawable.corner_green_right);
			}
			actionView.Click += (object sender, EventArgs e) => {
				Log.Debug(TAG,"Action view clicked! Doc number: "+docNum);
				IAction iaction = this as IAction;
				iaction.setActionOnChanged( this.Intent.Extras.GetString ("branchcode"),docNum, "","", action, false);
			};
			try{
				showProgress("Loading detail. Please wait...");
				if(await LoadRequisitionDetail (docNum)){
					Log.Debug(TAG,"LoadRequisitionDetail true");
					var transaction = SupportFragmentManager.BeginTransaction ();
					var fragment = new DetailFragment ();
					transaction.Replace (Resource.Id.content_fragment, fragment);
					transaction.Commit ();
				}else{
					Log.Debug(TAG,"LoadRequisitionDetail false");
					showAlert("Error","Loading failed. Please try again.");
				}
				progress.Hide();
			}catch(Exception ex){
				Log.Error (TAG, ex.ToString());
				progress.Hide();
				showAlert("Error","Loading failed. Please try again.");
			}
		}
		public override bool OnPrepareOptionsMenu(IMenu menu)
		{
			return base.OnPrepareOptionsMenu(menu);
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				//do something
				Finish();
				return true;
			}
			return base.OnOptionsItemSelected(item);
		}
		public async Task<bool> LoadRequisitionDetail(string sDocNumber){
			bool result = false;
			await Task.Run (() => {
				try{
					if(checkConnection(this)){
						//showProgress("Loading Master Data. Please wait..");
						try{
							RAS.cemDetailRequisition[] arrDetailItem  = svc.getDetailRequisition(sDocNumber);
							Log.Debug(TAG,"arrDetailItem count:"+arrDetailItem.GetLength (0));
							for (int i = 0; i < arrDetailItem.GetLength (0); i++) {
								lDetailItem.Add(arrDetailItem[i]);
							}
							RAS.cemDetailFlow[] arrDetailFlow  = svc.getDetailFlow(sDocNumber);
							Log.Debug(TAG,"arrDetailFlow count:"+arrDetailFlow.GetLength (0));
							for (int i = 0; i < arrDetailFlow.GetLength (0); i++) {
								lDetailFlow.Add(arrDetailFlow[i]);
							}
							RequisitionPending.setRequisitionDetailItem(lDetailItem);
							RequisitionPending.setRequisitionDetailFlow(lDetailFlow);
							result = true;
						}catch(Exception ex){
							Log.Error(TAG,"LoadRequisitionDetil error in :"+ex.ToString());
							//resetConnection();
							//showAlert("Error","Please try again.");
						}
						//progress.Hide();
					}else{
						//showAlert("Network Error","Please check your network connection.");
					}
				}
				catch(Exception ex){ 
					Log.Error(TAG,"LoadRequisitionDetil error out:"+ex.ToString());
				}
			});
			return result;
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
		#region IAction implementation
		public async void setActionOnChanged (string sBranch, string sDocNumber, string sWorkflow, string sComment, string sAction, bool bIsTriggered)
		{
			//Log.Debug (TAG, "Doc Number :" + sDocNumber);
			if (bIsTriggered) {
				await InsertRequisitionData(sBranch, sDocNumber, sWorkflow, sComment, sAction);
			} else {
				Bundle data = new Bundle ();
				data.PutString ("branchCode",sBranch);
				data.PutString ("docNumber", sDocNumber);
				data.PutString ("action", sAction);
				data.PutString ("workflow", sWorkflow);
				var actionFragment = new ActionFragment ();
				actionFragment.Arguments = data;
				actionFragment.Show (SupportFragmentManager, "action_fragment");
			}
		}
		#endregion
	}

}

