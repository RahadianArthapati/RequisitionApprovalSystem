using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RQNApprovalSystem
{
	public class Login
	{
		private RAS.Service1 svc = new RAS.Service1();
		private cbll bll = new cbll();
		public Login ()
		{

		}
		public async Task<bool> ValidateUser(string user, string password, bool isAuto){
			bool result = true;
			await Task.Run (() => {
				try{
					//string nik = svc.getUserAD(Config.DOMAIN,user,password);


					if(result){
						//string qUser = string.Format("INSERT INTO tUser (USER_ID, IS_AUTOLOGIN) VALUES({0},{1})",branchId[i],isAuto?1:0);
						//bll.vExecuteSql(qUser);
					}
				}
				catch(Exception ex){ 
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}
			});
			return result;
		}
		public async Task<bool> InitData(){
			bool result = false;
			await Task.Run (() => {
				try{
//					List<string> branchId= svc.getBranch();
//					for (int i = 0; i < branchId.Count; i++) {
//						try {
//							string qBranch = string.Format("INSERT INTO tBranch (BRANCHCODE) VALUES({0})",branchId[i]);
//							bll.vExecuteSql(qBranch);
//						} catch (Exception ex) {
//							System.Diagnostics.Debug.WriteLine(ex.ToString());
//						}
//						result = true;
//					}
				}
				catch(Exception ex){ 
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}
			});
			return result;
		}
	}
}

