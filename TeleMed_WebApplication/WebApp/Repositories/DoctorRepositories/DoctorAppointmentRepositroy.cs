using DataAccess;
using DataAccess.CustomModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApp.Helper;

namespace WebApp.Repositories.DoctorRepositories
{
    public class DoctorAppointmentRepositroy
    {
        public List<RescheduleAppModel> GetRescheduleApp(long doctorID)
        {

            try
            {

                var response = ApiConsumerHelper.GetResponseString("api/GetRescheduleAppforDoctor?doctorID=" + doctorID);
                var result = JsonConvert.DeserializeObject<List<RescheduleAppModel>>(response);
                return result;
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }

        }
       
        public List<RescheduleAppModel> GetUpcomingApp(long doctorID)
        {

            try
            {

                var response = ApiConsumerHelper.GetResponseString("api/GetUpcomingAppforDoctor?doctorID=" + doctorID);
                var result = JsonConvert.DeserializeObject<List<RescheduleAppModel>>(response);
                return result;
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }

        }
        public List<ReschedulePendingAppModel> GetPendingApp(long doctorID)
        {

            try
            {

                var response = ApiConsumerHelper.GetResponseString("api/GetPendingAppforDoctor?doctorID=" + doctorID);
                var result = JsonConvert.DeserializeObject<List<ReschedulePendingAppModel>>(response);
                return result;
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }

        }
        public GetAppDetail GetAppDetail(long appID)
        {

            try
            {

                var response = ApiConsumerHelper.GetResponseString("api/GetAppDetail?appID=" + appID);
                var result = JsonConvert.DeserializeObject<GetAppDetail>(response);
                return result;
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }

        }
        public ApiResultModel CancelReschedule(CancelRescheduleRequestModel model)
        {

            try
            {

                var strContent = JsonConvert.SerializeObject(model);
                var response = ApiConsumerHelper.PostData("api/CancelRescheduleRequest", strContent);
                var result = JsonConvert.DeserializeObject<ApiResultModel>(response);
                return result;
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }

        }

        public ApiResultModel Reschedule(RescheduleRequestModel model)
        {

            try
            {

                var strContent = JsonConvert.SerializeObject(model);
                var response = ApiConsumerHelper.PostData("api/RescheduleRequest", strContent);
                var result = JsonConvert.DeserializeObject<ApiResultModel>(response);
                return result;
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }

        }




        public List<GetPrescriptionFrequency> GetPrescriptionFrequency()
        {

            var response = ApiConsumerHelper.GetResponseString("api/getprescriptionFrequency");
            var result = JsonConvert.DeserializeObject<List<GetPrescriptionFrequency>>(response);
            return result;
        }


        public List<GetPrescriptionDrug> GetDrugSmartSearchResult(string P_name="")
        {
            try
            {
                //var strContent = JsonConvert.SerializeObject(P_name);
                var response = ApiConsumerHelper.GetResponseString("api/GetDrugSmartSearch?P_name=" + P_name);
                var result = JsonConvert.DeserializeObject<List<GetPrescriptionDrug>>(response);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetPrescriptionDrug> GetDrugDescription(string search)
        {

            var response = ApiConsumerHelper.GetResponseString("api/GetDrugDescription/?search=" + search);
            var result = JsonConvert.DeserializeObject<List<GetPrescriptionDrug>>(response);
            return result;
        }

        public ApiResultModel SavePrescription(List<SaveDrugPrescription> arr)
        {
            try
            {
                var strContent = JsonConvert.SerializeObject(arr);
                var response = ApiConsumerHelper.PostData("api/SavePrescriptionList", strContent);
                var result = JsonConvert.DeserializeObject<ApiResultModel>(response);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ApiResultModel XmlDataConversion(CancelRescheduleRequestModel AppId)
        {
            try
            {
                var strContent = JsonConvert.SerializeObject(AppId);
                var response = ApiConsumerHelper.PostData("api/SavePrescriptionXMLDateConvertion", strContent);
                var result = JsonConvert.DeserializeObject<ApiResultModel>(response);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}