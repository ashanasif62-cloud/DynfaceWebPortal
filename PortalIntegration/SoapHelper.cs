using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PortalIntegration
{
    public class SoapHelper
    {
        // Phase 3 Optimization: Build the binding ONCE per app-domain.
        // BasicHttpBinding is thread-safe and stateless after construction — safe to share.
        private static readonly Binding _sharedBinding = BuildBinding();

        public static Binding SharedBinding => _sharedBinding;

        public static string GetSoapServiceUriString(string serviceName, string aosUriString)
        {
            var soapServiceUriStringTemplate = "{0}/soap/services/{1}";
            var soapServiceUriString = string.Format(soapServiceUriStringTemplate, aosUriString.TrimEnd('/'), serviceName);
            return soapServiceUriString;
        }

        // Kept for backward-compatibility; returns the shared cached instance
        public static Binding GetBinding()
        {
            return _sharedBinding;
        }

        private static Binding BuildBinding()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            // Set binding timeout and other configuration settings
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;

            binding.ReceiveTimeout = TimeSpan.MaxValue;
            binding.SendTimeout = TimeSpan.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;

            var httpsTransportBindingElement = binding.CreateBindingElements().OfType<HttpsTransportBindingElement>().FirstOrDefault();
            if (httpsTransportBindingElement != null)
            {
                httpsTransportBindingElement.MaxPendingAccepts = 10000;
            }

            var httpTransportBindingElement = binding.CreateBindingElements().OfType<HttpTransportBindingElement>().FirstOrDefault();
            if (httpTransportBindingElement != null)
            {
                httpTransportBindingElement.MaxPendingAccepts = 10000;
            }
            return binding;
        }
    }
}
