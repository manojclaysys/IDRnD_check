using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IDRnD_check.Helper;
using IDRnD_check.Models;
using Newtonsoft.Json;
using RestSharp;

namespace IDRnD_check.Repository
{
    public class VoiceAPIRepo
    {

        public string VoiceAPICall(string path, string apimethod)
        {
            string endpoint = $"https://voice-rest-api.idrnd.net/{apimethod}";
            IDRAndDResponseModel iDRAndDResponse = new IDRAndDResponseModel();
            var client = new RestClient(endpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-api-key", "FdArSwfW1O4kqA3AtiZTaKaf4zXP2K4wR2DRxae0");
            request.AddFile("wav_file", path);
            IRestResponse response = client.Execute(request);
            //iDRAndDResponse = JsonConvert.DeserializeObject<IDRAndDResponseModel>(response.Content);
            //log.LogInformation("CheckLiveness response : " + response.Content);
            return response.Content;
        }

        public string MatchTemplate(MatchTemplate template, string apimethod)
        {
            string endpoint = $"https://voice-rest-api.idrnd.net/{apimethod}";

            var client = new RestClient(endpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-api-key", "FdArSwfW1O4kqA3AtiZTaKaf4zXP2K4wR2DRxae0");
            request.AddJsonBody(JsonConvert.SerializeObject(template));

            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        public void InsertDataValue(string userID, string template)
        {
            DAL _dal = new DAL();

            _dal.InsertEnrollmentData(userID, template);
        }

        public string GetTemplate(string userID)
        {
            DAL _dal = new DAL();

            return _dal.GetTemplate(userID);
        }

        public void InsertHistory(string userID,string template1, string template2,int type)
        {
            DAL _dal = new DAL();

            _dal.VerifyHistory(userID,template1,template2, type);
        }
    }
}