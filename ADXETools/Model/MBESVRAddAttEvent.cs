using SwaggerExample.FalconRequests;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SwaggerExample.Model
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot]
    public class MBESVRAddAttEventRequest : ServiceInput
    {
        /// <summary>
        /// 
        /// </summary>
        public MBESVRAddAttEventRequest() { }

        /// <summary>
        /// 
        /// </summary>
        [BindNever, XmlAttribute, JsonIgnore]
        public string Method { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        [Required, XmlElement]
        public string RoleCode { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [Required, XmlElement]
        public string BusinessCategory { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        [Required, XmlElement]
        public string WorkAssignmentId { get; set; } = string.Empty;        
    }
}
