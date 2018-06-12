using System;
using System.Xml;
using System.Xml.Serialization;

namespace ADXETools.FalconRequests
{ 
    /// <summary>
    /// Templatized Falcon Request object constrained to objects of type ServiceInput
    /// </summary>
    /// <typeparam name="T">Template type constrained to objects of type ServiceInput</typeparam>
    [XmlRoot("Request")]
    [Serializable]
    public class FalconRequest<T> where T : ServiceInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Header Header { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public T ServiceInput { get; set; }

        /// <summary>
        /// Dynamically create FalconRequest object from passed type and header
        /// </summary>
        /// <param name="serviceInput">The service input object of desired type T</param>
        /// <param name="headerString">String to store as request header var</param>
        /// <returns></returns>
        public static FalconRequest<T> CreateRequest(T serviceInput, string headerString)
        {
            return new FalconRequest<T> { Header = new Header(headerString), ServiceInput = serviceInput };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceInput"></param>
        /// <param name="headerObject"></param>
        /// <returns></returns>
        public static FalconRequest<T> CreateRequest(T serviceInput, Header headerObject)
        {
            return new FalconRequest<T> { Header = new Header(headerObject.FalconServiceRequestAction), ServiceInput = serviceInput };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Header
    {
        /// <summary>
        /// 
        /// </summary>
        public Header() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="falconServiceRequestAction"></param>
        public Header(string falconServiceRequestAction)
        {
            FalconServiceRequestAction = falconServiceRequestAction;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("Action")]
        public string FalconServiceRequestAction { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ServiceInput
    {
    }
}