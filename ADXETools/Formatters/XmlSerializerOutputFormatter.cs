using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace ADXETools.Formatters
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlSerializerOutputFormatter : Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter
    {
        static readonly string _myAssemblyName = Assembly.GetExecutingAssembly().FullName;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override XmlSerializer CreateSerializer(Type type)
        {
            if (type.Assembly.FullName != _myAssemblyName)
                return base.CreateSerializer(type);

            var serializer = new XmlSerializer(type, new XmlRootAttribute(type.GetRootName()) { Namespace = "" });
            return serializer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="xmlWriter"></param>
        /// <param name="value"></param>
        protected override void Serialize(XmlSerializer xmlSerializer, XmlWriter xmlWriter, object value)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            xmlSerializer.Serialize(xmlWriter, value, ns);
        }
    }
}
