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


namespace RQNApprovalSystem
{
	public class DetailFragment : Fragment
	{
		private static string TAG = "DetailFragment";
		private ViewPager pager;
		private PagerSlidingTabStrip tabs;
		private DetailAdapter adapter;
		public static DetailFragment NewInstance()
		{
			var f = new DetailFragment ();
			return f;
		}
		public static void setAdapter()
		{
			try{

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
			var root = inflater.Inflate(Resource.Layout.fragment_detail, container, false);
			adapter = new DetailAdapter(this.Activity.SupportFragmentManager);
			pager = root.FindViewById<ViewPager> (Resource.Id.pager_detail);
			tabs = root.FindViewById<PagerSlidingTabStrip> (Resource.Id.tabs_detail);
			pager.OffscreenPageLimit = 3;
			var pageMargin = (int)TypedValue.ApplyDimension (ComplexUnitType.Dip, 0, Resources.DisplayMetrics);
			pager.PageMargin = pageMargin;
			tabs.SetBackgroundColor(Resources.GetColor (Resource.Color.blue));
			pager.Adapter = adapter;
			tabs.SetViewPager (pager);
			//ViewCompat.SetElevation(root, 50);
			return root;
		}
	}
	internal class DetailAdapter : FragmentPagerAdapter{
		private string[] Details = {"DETAIL ITEM", "DETAIL FLOW"};
		public DetailAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
		{

		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted (int position)
		{
			return new Java.Lang.String (Details [position]);
		}
		#region implemented abstract members of PagerAdapter
		public override int Count {
			get {
				return Details.Length;
			}
		}
		#endregion
		#region implemented abstract members of FragmentPagerAdapter
		public override Fragment GetItem (int position)
		{
			switch (position) {
			case 0:
				return DetailItemFragment.NewInstance ();
			case 1:
				return DetailFlowFragment.NewInstance ();
			default:
				return null;
			}
		}
		#endregion
	}
}

