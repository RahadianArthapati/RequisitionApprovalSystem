using System;

namespace RQNApprovalSystem
{
	public class cArrayList
	{
		protected string _paramid;
		protected object _paramvalue;

		public string paramid
		{ set { _paramid = value; } get { return (_paramid); } }

		public object ParamValue
		{ set { _paramvalue = value; } get { return (_paramvalue); } }

		public cArrayList(string sParamID, object oParamValue)
		{
			paramid = sParamID;
			ParamValue = oParamValue;
		}
	}
}


