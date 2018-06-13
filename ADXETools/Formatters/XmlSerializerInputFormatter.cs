using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SwaggerExample.Formatters
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlSerializerInputFormatter : Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerInputFormatter
    {
        static readonly string _myAssemblyName = Assembly.GetExecutingAssembly().FullName;

        /// <summary>
        /// 
        /// </summary>
        public XmlSerializerInputFormatter() : base(new MvcOptions { RespectBrowserAcceptHeader = true })
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readStream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        protected override XmlReader CreateXmlReader(Stream readStream, Encoding encoding)
        {
            var reader = base.CreateXmlReader(readStream, encoding);
            return reader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override XmlSerializer CreateSerializer(Type type)
        {
            if (type.Assembly.FullName != _myAssemblyName)
                return base.CreateSerializer(type);

            XmlSerializer serializer = new XmlSerializer(type, new XmlRootAttribute(type.GetRootName()) { Namespace = "" });
            return serializer;
        }
    }
}
