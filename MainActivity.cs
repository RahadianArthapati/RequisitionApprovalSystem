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
using Fragment = Android.Support.V4.App.Fragment;
using PopupMenu = Android.Support.V7.Widget.PopupMenu;

namespace RQNApprovalSystem
{
	[Activity (Label = "Main")]
	public class MainActivity : BaseActivity,IAction
	{
		private static string TAG = "MainActivity";
		private List<RAS.cemPendingRequisition> lRequisitionData;
		protected override int LayoutResource {
			get {
				setActionType (0);
				return Resource.Layout.activity_main;
			}
		}

//		private DrawerLayout mDrawerLayout;
//		private RecyclerView mDrawerList;
//		private String[] mDrawerTitles;

		private MyPagerAdapter adapter;
		private Drawable oldBackground = null;
		private int currentColor;
		private ViewPager pager;
		private PagerSlidingTabStrip tabs;
//		private List<RAS.cemPendingRequisition> lRequisitionData = new List<RQNApprovalSystem.RAS.cemPendingRequisition>();
		public  string[] Branches = {"BAKAN", "LANUT", "JAKARTA", "PENJOM", "SERUYUNG"};
		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
//			if (await main.LoadDataPending (Config.TEMP_ACCESS)) {

//			}
			adapter = new MyPagerAdapter(SupportFragmentManager);
			pager = FindViewById<ViewPager> (Resource.Id.pager);
			tabs = FindViewById<PagerSlidingTabStrip> (Resource.Id.tabs);
			pager.OffscreenPageLimit = 6;

			var pageMargin = (int)TypedValue.ApplyDimension (ComplexUnitType.Dip, 0, Resources.DisplayMetrics);
			pager.PageMargin = pageMargin;
//			pager.PageSelected += (object sender, ViewPager.PageSelectedEventArgs e) => {
//				BranchFragment.setAdapter (Branches[e.Position]);
//			};
//			tabs.OnTabReselectedListener = this;
		
			ChangeColor (Resources.GetColor (Resource.Color.blue));
			try{
				showProgress("Loading data. Please wait...");
				if(await LoadRequisitionData (Config.TEMP_ACCESS)){
					Log.Debug(TAG,"LoadRequisitionData true");
					pager.Adapter = adapter;
					tabs.SetViewPager (pager);
					adapter.NotifyDataSetChanged();

				}else{
					Log.Debug(TAG,"LoadRequisitionData false");
				}
				progress.Hide();

			}catch(Exception ex){
				Log.Error (TAG, ex.ToString());
				progress.Hide();
			}
		}
		public override bool OnPrepareOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.main, menu);

			//Spinner spinner = (Spinner) menu.FindItem (Resource.Id.action_setting).ActionView;
			//ISpinnerAdapter spinnerAdapter = ArrayAdapter.CreateFromResource(this,Resource.Array.action_setting,Android.Resource.Layout.SimpleSpinnerDropDownItem);
			//spinner.Adapter  = spinnerAdapter;
			return base.OnPrepareOptionsMenu(menu);
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.action_logout:
				//do something
				Log.Debug (TAG, "Log out");
				OnLogout ();
				return true;
			}
			return base.OnOptionsItemSelected(item);
		}
		#region IAction implementation
		public async void setActionOnChanged (string sBranch, string sDocNumber, string sWorkflow, string sComment, string sAction, bool bIsTriggered)
		{
			//Log.Debug (TAG, "Doc Number :" + sDocNumber);
			if (bIsTriggered) {
				await InsertRequisitionData(sBranch, sDocNumber, sWorkflow, sComment, sAction);
			} else {
				Bundle data = new Bundle ();
				data.PutString ("branchCode", sBranch);
				data.PutString ("docNumber", sDocNumber);
				data.PutString ("action", sAction);
				data.PutString ("workflow", sWorkflow);
				var actionFragment = new ActionFragment ();
				actionFragment.Arguments = data;
				actionFragment.Show (SupportFragmentManager, "action_fragment");
			}
		}
		#endregion
		private void OnLogout(){
			RunOnUiThread (() => {
				Intent intent = new Intent (this.ApplicationContext,typeof(MainActivity));
				intent.SetFlags (ActivityFlags.NewTask | ActivityFlags.ClearTask);
				OverridePendingTransition(Resource.Animation.abc_slide_out_top, Resource.Animation.abc_slide_out_bottom);
				StartActivity (intent);
			} );
		}
		public async Task<bool> LoadRequisitionData(string sNxtactionr){
			bool result = false;
			await Task.Run (() => {
				try{
					if(checkConnection(this)){

						try{
							lRequisitionData = new List<RAS.cemPendingRequisition>();
							RAS.cemPendingRequisition[] arrPendingRequisition  = svc.getPendingRequisition(sNxtactionr);
							Log.Debug(TAG,"arr RequisitionData count:"+arrPendingRequisition.GetLength (0));
							for (int i = 0; i < arrPendingRequisition.GetLength (0); i++) {
								lRequisitionData.Add(arrPendingRequisition[i]);
							}
							RequisitionPending.setRequisitionPending(lRequisitionData);
							result = true;
						}catch(Exception ex){
							Log.Error(TAG,"LoadRequisitionData error in :"+ex.ToString());
							//resetConnection();
							RunOnUiThread (() => {
								showAlert("Error","Please try again.");
							} );
						
						}
					
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
					Log.Error(TAG,"LoadRequisitionData error out:"+ex.ToString());
				}
			});
			return result;
		}
		private void ChangeColor(Color newColor) {
			tabs.SetBackgroundColor(newColor);
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
	}
	internal class MyPagerAdapter : FragmentPagerAdapter{
		private  string[] Branches = {"BAKAN", "LANUT", "JAKARTA", "PENJOM", "SERUYUNG"};
		public MyPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
		{

		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted (int position)
		{
			return new Java.Lang.String (Branches [position]);
		}
		#region implemented abstract members of PagerAdapter
		public override int Count {
			get {
				return Branches.Length;
			}
		}
		#endregion
		#region implemented abstract members of FragmentPagerAdapter
		public override Fragment GetItem (int position)
		{
			return BranchFragment.NewInstance (Branches [position]);
		}
		#endregion
	}
}


