
using REST_Calculator.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace REST_Calculator.Log
{
    public class Logger
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Acer\source\repos\REST-Calculator\REST-Calculator\App_Data\DB.mdf;Integrated Security=True";
        //SqlConnection cnn;

        private List<CallDetails> sortedList;
        //private Queue<CallDetails> queue;
        private static readonly object Instancelock = new object();
        private int currentCallId;
        private static Logger instance = null;

        public Logger()
        {
            sortedList = new List<CallDetails>();
            //string getCurrentIdQuery = "select isnull(max(id),0)+1 from dbo.Call";
            string getCurrentIdQuery = "SELECT IDENT_CURRENT('dbo.Call')+1";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand(getCurrentIdQuery))
                {
                    cmd.Connection = sqlConnection;
                    currentCallId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }


        public static Logger GetInstance()
        {
            lock (Instancelock)
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }


        internal int RequestToJson()
        {
            int callId = InsertCall();
            InsertCallDetails(callId, MessageType.RequestToJson);
            return callId;
        }


        internal void RequestToSoap(int id)
        {
            InsertCallDetails(id, MessageType.RequestToSoap);
        }


        internal void ResponseFromSoap(int id)
        {
            InsertCallDetails(id, MessageType.ResponseFromSoap);
        }


        private void InsertCallDetails(int callId, MessageType type)
        {
            CallDetails callDetails = new CallDetails(callId, type);
            sortedList.Add(callDetails);
            CheckPending();
        }

        private int InsertCall()
        {
            int callId = 0;
            string newCallQuery = "insert into dbo.Call (InsertDate) values(GETDATE());  SELECT SCOPE_IDENTITY();";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand(newCallQuery))
                {
                    cmd.Connection = sqlConnection;
                    callId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return callId;
        }
        private void CheckPending()
        {
            if (sortedList.Count() == 0)
                return;

            CallDetails callDetails = this.PopFirstCurrentCall();
            if (callDetails!=null)
            {
                
                string newCallDetailQuery = String.Format(@"insert into dbo.CallDetails (CallId, Value) values ({0}, '{1}')", callDetails.ParentID, callDetails.Value);
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand(newCallDetailQuery))
                    {
                        cmd.Connection = sqlConnection;
                        cmd.ExecuteNonQuery();
                    }
                }
                if(callDetails.messageType == MessageType.ResponseFromSoap)
                {
                    currentCallId++;
                }
                CheckPending();
            }

        }

        private CallDetails PopFirstCurrentCall()
        {
            CallDetails result = sortedList.Where(x => x.ParentID == currentCallId).OrderBy(x => (int)x.messageType).First();

            if (result != null)
            {
                sortedList.Remove(result);
            }
            return result;
        }
    }


}