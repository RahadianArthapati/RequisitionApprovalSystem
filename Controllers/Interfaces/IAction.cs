using System;

namespace RQNApprovalSystem
{
	public interface IAction
	{
		void setActionOnChanged(string sBranch, string sDocNumber, string sWorkflow, string sComment, string sAction, bool isTriggered);

	}
}

