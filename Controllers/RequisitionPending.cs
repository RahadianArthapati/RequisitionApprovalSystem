using System;
using System.Collections.Generic;

namespace RQNApprovalSystem
{
	public static class RequisitionPending
	{
		private static List<RAS.cemPendingRequisition> _lRqn;
		private static List<RAS.cemDetailRequisition> _lDetailItem;
		private static List<RAS.cemDetailFlow> _lDetailFlow;
		public static void setRequisitionPending (List<RAS.cemPendingRequisition> lRqn)
		{
			if (_lRqn != null)
				_lRqn.Clear ();
			_lRqn = lRqn; 
		}
		public static List<RAS.cemPendingRequisition> getRequisitionPending(){
			return _lRqn;
		}
		public static void setRequisitionDetailItem (List<RAS.cemDetailRequisition> lItem)
		{
			if (_lDetailItem != null)
				_lDetailItem.Clear ();
			_lDetailItem = lItem; 
		}
		public static List<RAS.cemDetailRequisition> getRequisitionDetailItem(){
			return _lDetailItem;
		} 
		public static void setRequisitionDetailFlow (List<RAS.cemDetailFlow> lFlow)
		{
			if (_lDetailFlow != null)
				_lDetailFlow.Clear ();
			_lDetailFlow = lFlow; 
		}
		public static List<RAS.cemDetailFlow> getRequisitionDetailFlow(){
			return _lDetailFlow;
		} 
	}
}

