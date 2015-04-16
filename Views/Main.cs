using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mono.Data.Sqlite;

namespace RQNApprovalSystem
{
	public class Main
	{
		private RAS.Service1 svc = new RAS.Service1();
		private cbll bll = new cbll();
		private cdal dal = new cdal ();
		public Main ()
		{

		}
		private void DeleteDataPending(){
			bll.vExecuteSql("DELETE FROM tPendingRqn");
		}
		public async Task<bool> LoadDataPending(string sNxtactionr){
			bool result = false;
			await Task.Run (() => {
				try{
					DeleteDataPending();
					bll.vInsertPendingData(svc,sNxtactionr);
					result = false;
				}
				catch(Exception ex){ 
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}
			});
			return result;
		}
		public async Task<List<RAS.cemPendingRequisition>> GetDataPendingByBranch(string sBranchCode){

			await Task.Run (() => {
				try{
					return bll.lGetPendingdDataByBranch(sBranchCode);
				}
				catch(Exception ex){ 
					System.Diagnostics.Debug.WriteLine(ex.ToString());
					return null;
				}
			});
			return null;
		}
		public async Task<List<RAS.cemDetailRequisition>> GetDetailRequisition(string sDocNumber){

			await Task.Run (() => {
				try{
					return svc.getDetailRequisition(sDocNumber);
				}
				catch(Exception ex){ 
					System.Diagnostics.Debug.WriteLine(ex.ToString());
					return null;
				}
			});
			return null;
		}
		public async Task<List<RAS.cemDetailRequisition>> GetFlowRequisition(string sDocNumber){

			await Task.Run (() => {
				try{
					return svc.getDetailFlow(sDocNumber);
				}
				catch(Exception ex){ 
					System.Diagnostics.Debug.WriteLine(ex.ToString());
					return null;
				}
			});
			return null;
		}

	}
}



