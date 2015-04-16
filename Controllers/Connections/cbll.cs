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
using System.Data;
using Mono.Data.Sqlite;
using Android.Util;


namespace RQNApprovalSystem
{
	public class cbll
	{
		private static string TAG = "CBLL";
		cdal dal = new cdal();
		#region basic function
		protected string vPrepareStatement(string sTableName, List<string> arr)
		{
			string sSQL = "INSERT INTO " + sTableName + " VALUES('";
			for (int i = 0; i < arr.Count; i++)
			{
				if (i == (arr.Count - 1))
				{
					sSQL += arr[i].ToString() + "')";
				}
				else
				{
					sSQL += arr[i].ToString() + "','";
				}
			}
			return (sSQL);
		}
		public void vExecuteSqlParameter(string sSql, List<cArrayList> arr)
		{
			try{
				dal.vPrepareParameter(sSql, arr);
			}catch(NullReferenceException ex){
				Log.Error (TAG, ex.ToString ());
			}catch(Exception ex){
				Log.Error (TAG, ex.ToString ());
			}
		}
		public void vExecuteSql(string sSQL)
		{
			dal.vExecuteSQL(sSQL);
		}
		#endregion

		#region implement Pending Requisition
		public void vInsertPendingData(RAS.Service1 svc, string sNxtactionr){
			try{
				RAS.cemPendingRequisition[] arrPendingRqn = svc.getPendingRequisition(sNxtactionr);
				System.Diagnostics.Debug.WriteLine("arrPendingRqn count: " + arrPendingRqn.GetLength (0));

				for (int i = 0; i < arrPendingRqn.GetLength (0); i++) {
					try {
						string qPendingRqn = "INSERT INTO tPendingRqn (BRANCHCODE, DOCNUMBER, DESCRIPTIO, REQRID, REQRNAME, REQDATE, COSTCTR, ACTION, RQNTOTVALUE, RQNCURRCODE, RQRDDATE, COMMENT )"+
							" VALUES(@BRANCHCODE, @DOCNUMBER, @DESCRIPTIO, @REQRID, @REQRNAME, @REQDATE, @COSTCTR, @ACTION, @RQNTOTVALUE, @RQNCURRCODE, @RQRDDATE, @COMMENT)";
						List<cArrayList> l = new List<cArrayList>();
						l.Add(new cArrayList("@BRANCHCODE",arrPendingRqn[i].BRANCHCODE));
						l.Add(new cArrayList("@DOCNUMBER",arrPendingRqn[i].DOCNUMBER));
						l.Add(new cArrayList("@DESCRIPTIO",arrPendingRqn[i].DESCRIPTIO));
						l.Add(new cArrayList("@REQRID",arrPendingRqn[i].REQRID));
						l.Add(new cArrayList("@REQRNAME",arrPendingRqn[i].REQRNAME));
						l.Add(new cArrayList("@REQDATE",arrPendingRqn[i].REQDATE));
						l.Add(new cArrayList("@COSTCTR",arrPendingRqn[i].COSTCTR));
						l.Add(new cArrayList("@ACTION",arrPendingRqn[i].ACTION));
						l.Add(new cArrayList("@RQNTOTVALUE",arrPendingRqn[i].RQNTOTVALUE));
						l.Add(new cArrayList("@RQNCURRCODE",arrPendingRqn[i].RQNCURRCODE));
						l.Add(new cArrayList("@RQRDDATE",arrPendingRqn[i].RQRDDATE));
						l.Add(new cArrayList("@COMMENT",arrPendingRqn[i].COMMENT));
						vExecuteSqlParameter(qPendingRqn,l);

					} catch (Exception ex) {
						System.Diagnostics.Debug.WriteLine("Error : "+ex.ToString());

					}
				}
			}catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.ToString());

			}
		}
		public List<RAS.cemPendingRequisition> lGetPendingdDataByBranch(string branch_code)
		{

			List<RAS.cemPendingRequisition> arr = new List<RAS.cemPendingRequisition>();
			SqliteDataReader rs = null;
			try{
			string query = string.Format ("SELECT DOCNUMBER, DESCRIPTIO, REQRID, REQRNAME, REQDATE, COSTCTR,ACTION, RQNTOTVALUE, RQNCURRCODE, RQRDDATE, COMMENT FROM [tPendingRqn] WHERE BRANCHCODE = '{0}'", branch_code);
			dal.vGetRecordSQL(query,ref rs);
			while (rs.Read())
			{
				RAS.cemPendingRequisition stPending = new RAS.cemPendingRequisition();
				stPending.DOCNUMBER = rs ["DOCNUMBER"].ToString ();
				stPending.DESCRIPTIO = rs["DESCRIPTIO"].ToString();
				stPending.REQRID = rs["REQRID"].ToString();
				stPending.REQRNAME = rs["REQRNAME"].ToString();
				stPending.REQDATE = rs["REQDATE"].ToString();
				stPending.COSTCTR = rs["COSTCTR"].ToString();
				stPending.ACTION = rs["ACTION"].ToString();
				stPending.RQNTOTVALUE = rs["RQNTOTVALUE"].ToString();
				stPending.RQNCURRCODE = rs["RQNCURRCODE"].ToString();
				stPending.RQRDDATE = rs["RQRDDATE"].ToString();
				stPending.COMMENT = rs["COMMENT"].ToString();
				arr.Add(stPending);
			} 
			rs.Close();
			}catch(Exception ex){
				System.Diagnostics.Debug.WriteLine(ex.ToString());

			}
			return (arr);
		}
		#endregion
		#region implement insert requisition
		public void vInsertRequisition(string sDocNumber, string sAction, string sDate){
			try{
				string qRequisition = "INSERT INTO tRqnAction (DOCNUMBER, ACTION, INPUTDATE, STAMP) VALUES(@DOCNUMBER, @ACTION, @INPUTDATE, @STAMP)";
				List<cArrayList> l = new List<cArrayList>();
				l.Add(new cArrayList("@DOCNUMBER",sDocNumber));
				l.Add(new cArrayList("@ACTION",sAction));
				l.Add(new cArrayList("@INPUTDATE",sDate));
				l.Add(new cArrayList("@STAMP",Utils.GetRandomNumber().ToString()));
				vExecuteSqlParameter(qRequisition,l);
			}catch(Exception ex){
				System.Diagnostics.Debug.WriteLine(ex.ToString());
			}
		}
		public void vInsertRequisitionData(RAS.Service1 svc,string sBranch, string sDocNumber, string sAction, string sDate){
			try{
				SqliteDataReader rs = null;
				List<ST_RequisitionData> listRqn = new List<ST_RequisitionData>();
				string query = string.Format ("SELECT DOCNUMBER, ACTION, INPUTDATE, STAMP FROM [tRqnAction]");
				dal.vGetRecordSQL(query,ref rs);
				while (rs.Read())
				{
					ST_RequisitionData data = new ST_RequisitionData();
					data.DOCNUMBER = rs ["DOCNUMBER"].ToString ();
					data.ACTION = rs["ACTION"].ToString();
					data.INPUTDATE = rs["INPUTDATE"].ToString();
					data.STAMP = rs["STAMP"].ToString();
					listRqn.Add(data);
				} 
				for (int i = 0; i < listRqn.Count; i++) {
					string status = svc.vInsertRequisition(sBranch,listRqn[i].DOCNUMBER, listRqn[i].ACTION, listRqn[i].INPUTDATE);
					//Log.Info(TAG,"Status : "+status);
					string del = string.Format ("DELETE FROM [tRqnAction] WHERE STAMP = '{0}'",listRqn[i].STAMP);
					vExecuteSql (del);
				}
			}catch(Exception ex){
				System.Diagnostics.Debug.WriteLine(ex.ToString());
			}
		}

		#endregion
	}

}