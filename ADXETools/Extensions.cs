using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

/// <summary>
/// 
/// </summary>
static public class Extensions
{
    /// <summary>
    /// Gets the XmlRootAttribute.ElementName or the type's Name if no XmlRootAttribute.ElementName
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    static public string GetRootName(this Type type)
    {
        var rootAttributes = type?.GetCustomAttributes(typeof(XmlRootAttribute), false);
        if (rootAttributes?.Length <= 0)
            return type.Name;

        string name = ((XmlRootAttribute)rootAttributes?[0]).ElementName ?? type.Name;
        return name;
    }

    /// <summary>
    /// serializes 'this' instance as XML
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="thisObj"></param>
    /// <param name="xmlRootName"></param>
    /// <returns>returns a string containing the XML representation of the object</returns>
    static public string ToXml<T>(this T thisObj, string xmlRootName = null) where T : class
    {
        if (thisObj == null)
            return null;
    
        using (var stream = new StringWriter())
        {
            using (var xw = XmlWriter.Create(stream, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // tell the serializer we don't want the default xmlns= or xsi= attributes
                new XmlSerializer(thisObj.GetType(), new XmlRootAttribute(xmlRootName ?? thisObj.GetType().GetRootName()) { Namespace = "" }).Serialize(xw, thisObj, ns);
            }
            string xml = stream.ToString();
            return xml;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xml"></param>
    /// <param name="xmlRootName"></param>
    /// <returns></returns>
    static public T ParseXml<T>(this string xml, string xmlRootName = null) where T : class
    {
        if (xml == null)
            return null;

        T obj = default(T);
        using (var stream = new StringReader(xml))
        {
            using (var xr = XmlReader.Create(stream))
            {
                obj = (T)new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName ?? typeof(T).GetRootName()) { Namespace = "" }).Deserialize(xr);
            }
        }
        return obj;
    }

    /// <summary>
    /// serializes 'this' instance as JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="thisObj"></param>
    /// <returns>returns a string containing the JSON representation of the object</returns>
    static public string ToJson<T>(this T thisObj) where T : class
    {
        if (thisObj == null)
            return null;

        string json = JsonConvert.SerializeObject(thisObj);
        return json;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="thisObj"></param>
    /// <returns></returns>
    static public List<KeyValuePair<string, string>> SerializeAsKeyValuePairs<T>(this T thisObj) where T : class
    {
        if (thisObj == null)
            return null;

        var json = thisObj.ToJson();
        var jObject = JObject.Parse(json);

        List<KeyValuePair<string, string>> nameValues = new List<KeyValuePair<string, string>>();
        GetValues(jObject, nameValues);
        return nameValues;
    }

    static void GetValues(IEnumerable<JToken> jToken, List<KeyValuePair<string, string>> nameValues)
    {
        foreach (var item in jToken)
        {
            if (item.Type == JTokenType.Array)
            {
                GetValues(item.Children(), nameValues);
            }
            else if (item.Type == JTokenType.Object)
            {
                GetValues(item.Children(), nameValues);
            }
            else if (item.Type == JTokenType.Property)
            {
                GetValues(item, nameValues);
            }
            else
            {
                nameValues.Add(new KeyValuePair<string, string>(item.Path, item.Value<string>()));
            }
        }
    }

    /// <summary>
    /// returns the name of the calling method
    /// </summary>
    /// <param name="thisObj"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    static public string GetMethodName<T>(this T thisObj, [CallerMemberName]string memberName = "") where T : class
    {
        return memberName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectToSerialize"></param>
    /// <param name="xmlRootName"></param>
    /// <returns></returns>
    public static string Serialize<T>(this T objectToSerialize, string xmlRootName = null)
    {
        if (objectToSerialize == null)
            return null;

        using (var stringWriter = new StringWriter())
        {
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false, CheckCharacters = true, ConformanceLevel = ConformanceLevel.Auto }))
            {
                new XmlSerializer(objectToSerialize.GetType(), new XmlRootAttribute(xmlRootName ?? objectToSerialize.GetType().GetRootName()) { Namespace = "" })
                    .Serialize(xmlWriter, objectToSerialize, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
            var xmlString = stringWriter.ToString();
            if (XmlStringContainsEncodedValues(xmlString))
            {
                return DecodedXmlString(xmlString);
            }
            return xmlString;
        }
    }

    private static string DecodedXmlString(string v)
    {
        return HttpUtility.HtmlDecode(v);
    }

    private static bool XmlStringContainsEncodedValues(string v)
    {
        if (v.Contains("&gt;") || v.Contains("&lt;"))
        {
            return true;
        }
        return false;
    }

}