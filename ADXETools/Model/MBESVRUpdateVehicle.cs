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
    public class MBESVRUpdateVehicle : ServiceInput
    {
        /// <summary>
        /// 
        /// </summary>
        public MBESVRUpdateVehicle() { }

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

        /// <summary>
        /// 
        /// </summary>
        [XmlElement]
        public string VINChange { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [XmlElement]
        public string MakeChange { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [XmlElement]
        public string ModelChange { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [XmlElement]
        public string YearChange { get; set; } = string.Empty;
    }
}