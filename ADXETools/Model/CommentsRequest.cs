using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ADXETools.Model
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot(Namespace = "")]
    public class CommentRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [BindNever, XmlAttribute, JsonIgnore]
        public string Method { get; set; }
        /// <summary>
        /// Claim GUIDs (if more than one, separate with '|' )
        /// </summary>
        [Required]
        public string GUID { get; set; } = "";
        /// <summary>
        /// Profile ids of agents (if more than one, separate with '|' )
        /// </summary>
        [Required]
        public string ProfId { get; set; } = "";

        /// <summary>
        /// The subject of the comment
        /// </summary>
        [Required]
        public string CommentSubject { get; set; } = "";

        /// <summary>
        /// The body of the comment
        /// </summary>
        [Required]
        public string CommentText { get; set; } = "";

        /// <summary>
        /// The author of the comment (ignored)
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The email address info
        /// </summary>
        public EmailInfo EmailInfo { get; set; }

        /// <summary>
        /// The client token
        /// </summary>
        [JsonIgnore, BindNever]
        public string ClientToken { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlRoot(Namespace = "")]
    public class EmailInfo
    {
        /// <summary>
        /// destination email address
        /// </summary>
        public string Destination { get; set; } = "";
    }
}
