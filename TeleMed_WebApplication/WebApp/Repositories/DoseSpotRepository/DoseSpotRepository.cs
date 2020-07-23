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
    public class DoseSpotRepository
    {

        /// <summary>
        /// Gets Pharmacy Search
        /// </summary>
        /// <param name="oSrch"></param>
        /// <returns></returns>
        /// 


            //Felix
        //public List<PharmacyEntry> GetPharmacySearchResult(DoseSpotPharmacySearch oSrch)
        //{
        //    try
        //    {
        //        var strContent = JsonConvert.SerializeObject(oSrch);
        //        var response = ApiConsumerHelper.PostData("api/SearchPharmacy", strContent);
        //        var result = JsonConvert.DeserializeObject<List<PharmacyEntry>>(response);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public string GetRefillErr()
        {
            try
            {
              //  var strContent = JsonConvert.SerializeObject(oSrch);
                var response = ApiConsumerHelper.GetResponseString("api/GetRefillErr");
                // var result = JsonConvert.DeserializeObject<List<PharmacyEntry>>(response);
                var result = JsonConvert.DeserializeObject<string>(response);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Get Patient DoseSpot Url
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public string GetPatientDoseSpotUrl(long patientId)
        {
            try
            {
                var response = ApiConsumerHelper.GetResponseString("api/GetPatientDoseSpotUrl?patientId=" + patientId);
               // var result = JsonConvert.DeserializeObject<string>(response);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //Felix
        public List<PharmacySmartSearch> GetPharmacySmartSearchResult(string P_name)
        {
            try
            {
                //var strContent = JsonConvert.SerializeObject(P_name);
                var response = ApiConsumerHelper.GetResponseString("api/GetPharmacySmartSearch?P_name=" + P_name);
                var result = JsonConvert.DeserializeObject<List<PharmacySmartSearch>>(response);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<DoseSpotPharmacySearch> GetPharmacyAddressResult(string P_name)
        {
            try
            {
                //var strContent = JsonConvert.SerializeObject(P_name);
                var response = ApiConsumerHelper.GetResponseString("api/GetPharmacyAddress?P_name=" + P_name);
                var result = JsonConvert.DeserializeObject<List<DoseSpotPharmacySearch>>(response);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PharmacyEntry> GetPharmacySearchResult(DoseSpotPharmacySearch oSrch)
        {
            try
            {
                var strContent = JsonConvert.SerializeObject(oSrch);
                var response = ApiConsumerHelper.PostData("api/GetPharmacyAddress", strContent);
                var result = JsonConvert.DeserializeObject<List<PharmacyEntry>>(response);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}