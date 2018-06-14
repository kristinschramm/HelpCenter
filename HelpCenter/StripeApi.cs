//using ServiceStack.Stripe;
using Stripe;
using System.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelpCenter.Ignored;

namespace HelpCenter
{
    public class StripeApi
    {
        public static void LaunchStripe()
        {
            StripeConfiguration.SetApiKey(APIKeys.Stripe);
        }

        public static void NewTenant()
        {
            var customerOptions = new StripeCustomerCreateOptions()
            {

            };

            var customerService = new StripeCustomerService();
            StripeCustomer customer = customerService.Create(customerOptions);
        }

        public static void ChargeTenant()
        {
            var options = new StripeChargeCreateOptions
            {
                Amount = 999,
                Currency = "usd",
                SourceTokenOrExistingSourceId = "tok_visa",
                ReceiptEmail = "jenny.rosen@example.com",
            };
            var service = new StripeChargeService();
            // StripeCharge charge = service.Create(chargeOptions);
        }
    }
}