using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebApp.Repositories.DoctorRepositories;

namespace WebApp.Helper
{
    public class StripePayHelper
    {
        #region Declarations

        private static string StripeApiKey = ConfigurationManager.AppSettings["StripePaySecretKey"].ToString();

        #endregion

        #region Methods

        /// <summary>
        /// Deduct money from client's credit card
        /// </summary>
        /// <param name="tokenId">Enter Token ID</param>
        /// <param name="amount">Enter Amount in cents</param>
        /// <returns></returns>
        public static bool PerformStripeCharge(string tokenId, int amount)
        {
            try
            {
                new HelperRepository().PerformStripeCharge(tokenId, amount);

                return true;
            }
            catch (Exception ex)
            { return false; }
        }

        #endregion

    }

}
