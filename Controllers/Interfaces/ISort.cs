using System;

namespace RQNApprovalSystem
{
	public interface ISort
	{
		void setSortByPrice(string sBranchCode);
		void setSortByNewest(string sBranchCode);
	}
}

