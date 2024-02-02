using IDRnD_check.Models;
using IDRnD_check.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDRnD_check.Controllers
{
    public class VoiceSDKController : Controller
    {
        // GET: VoiceSDK
        public ActionResult Enroll()
        {
            return View();
        }

        public ActionResult Verify()
        {
            return View();
        }

        public ActionResult DashBoard()
        {
            return View();
        }

        public JsonResult Upload()
        {
            VoiceAPIRepo apirepo = new VoiceAPIRepo();
            IDRAndDResponseModel response = null;
            string guid = Guid.NewGuid().ToString();
            foreach (string upload in Request.Files)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "UploadVoice/";
                var file = Request.Files[upload];
                if (file == null) continue;
                string fileName = $"{guid}_{upload}.wav";
                string pathName = Path.Combine(path, fileName);
                file.SaveAs(pathName);

                string userID = Request["userID"];

                string _template = apirepo.VoiceAPICall(pathName, "voice_template_factory/create_voice_template_from_file");

                apirepo.InsertDataValue(userID, _template);
                apirepo.InsertHistory(userID, _template, "", 1);
                string _SNR = apirepo.VoiceAPICall(pathName, "snr_computer/compute_with_file");
                response = new IDRAndDResponseModel();
                response.SNR = _SNR;
            }

           
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateVoice()
        {
            VoiceAPIRepo apirepo = new VoiceAPIRepo();
            IDRAndDResponseModel response = null;
            string guid = Guid.NewGuid().ToString();
            foreach (string upload in Request.Files)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "UploadVoice/";
                var file = Request.Files[upload];
                if (file == null) continue;
                string fileName = $"{guid}_{upload}.wav";
                string pathName = Path.Combine(path, fileName);
                file.SaveAs(pathName);
                string userID = Request["userID"];
                string _template = apirepo.VoiceAPICall(pathName, "voice_template_factory/create_voice_template_from_file");
                string _SNR = apirepo.VoiceAPICall(pathName, "snr_computer/compute_with_file");
                MatchTemplate template = new MatchTemplate();
                string templateID = apirepo.GetTemplate(userID);
                template.template1 = templateID;
                template.template2 = _template;
                string match = apirepo.MatchTemplate(template, "voice_template_matcher/match_voice_templates");

                response = JsonConvert.DeserializeObject<IDRAndDResponseModel>(match);
                response.SNR = _SNR;
                apirepo.InsertHistory(userID, templateID, _template, 2);
            }


            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}