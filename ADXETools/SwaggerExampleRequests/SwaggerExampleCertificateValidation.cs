using System;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace SwaggerExample.FalconRequests
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerExampleCertificateValidationHandler : HttpClientHandler
    {
        IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public SwaggerExampleCertificateValidationHandler(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            ServerCertificateCustomValidationCallback = ValidateRemoteCertificate;
        }

        /// <summary>
        /// 
        /// </summary>
        public class Validator : X509CertificateValidator
        {
            SwaggerExampleCertificateValidationHandler _handler;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="handler"></param>
            public Validator(SwaggerExampleCertificateValidationHandler handler)
            {
                _handler = handler;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="certificate"></param>
            public override void Validate(X509Certificate2 certificate)
            {
                if (!_handler.ValidateRemoteCertificate(null, certificate, null, SslPolicyErrors.None))
                {
                    throw new SecurityTokenValidationException("Invalid certificate issuer.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public X509ServiceCertificateAuthentication GetCertificateValidatorWCF()
        {
            return new X509ServiceCertificateAuthentication
            {
                CertificateValidationMode = X509CertificateValidationMode.Custom,
                CustomCertificateValidator = new Validator(this)
            };
        }

        private bool ValidateAdxeCertificate(X509Certificate cert)
        {
            string issuer = cert.Issuer.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            return issuer.Contains("SwaggerExample") || issuer.Contains("AUDATEX") || issuer.Contains("DO_NOT_TRUST_FIDDLERROOT") || ValidateCertificateFromEnvironment(cert);
        }

        private bool ValidateCertificateFromEnvironment(X509Certificate cert)
        {
            string issuer = cert.Issuer.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            string subject = cert.Subject.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            string[] allowedIssuers = (Environment.GetEnvironmentVariable("ALLOWED_ISSUERS") ?? "").Split(',');
            string[] allowedDomains = (Environment.GetEnvironmentVariable("ALLOWED_DOMAINS") ?? "").Split(',');

            return allowedIssuers.Any(allowedIssuer => allowedIssuer.Trim() != "" && issuer.Contains(allowedIssuer))
                && allowedDomains.Any(allowedDomain => allowedDomain.Trim() != "" && subject.Contains(allowedDomain));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsDev()
        {
            if (_hostingEnvironment.IsDevelopment() ||
                _hostingEnvironment.IsEnvironment("Local")||
                _hostingEnvironment.IsEnvironment("Development"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsQA()
        {
            if (_hostingEnvironment.IsStaging() ||
                _hostingEnvironment.IsEnvironment("qa"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsProduction()
        {
            if (_hostingEnvironment.IsProduction() ||
                _hostingEnvironment.IsEnvironment("production"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="policyErrors"></param>
        /// <returns></returns>
        public bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        {
            if (IsDev() && ValidateAdxeCertificate(cert))
            {
                return true;
            }

            if (IsQA() && ValidateCertificateFromEnvironment(cert))
            {
                return true;
            }

            return policyErrors == SslPolicyErrors.None;
        }
    }
}