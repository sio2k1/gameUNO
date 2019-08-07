using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml.Serialization;


/*//the purpose of this is provide easy json and xml serialization.
 * string str = serializer_helper.json_serialize_object_to_string(obj)
 * object_type obj = serializer_helper.json_deserialize_object_from_string<object_type>(str);
 * 
  */


namespace cmn_infrastructure 
{

    public class string_serializer 
    {
        public T deserialize_object_from_string<T>(string objectData)
        {
            return (T)deserialize_object_from_string(objectData, typeof(T));
        }

        public virtual object deserialize_object_from_string(string json, Type incoming_type)
        {
            return null;
        }

        public serialized_string serialize_obj_to_serialized_string(object obj)
        {
            serialized_string sr = new serialized_string();
            sr.serialized_data = serialize_object_to_string(obj);
            return sr;
        }
        public virtual string serialize_object_to_string(object obj)
        {
            return "";
        }
    }


    public static class string_writer_reader
    {
        public static void write_to_file(string text, string filename)
        {
            File.WriteAllText(filename, text);
        }

        public static string read_from_file(string filename)
        {
            return File.ReadAllText(filename);
        }
    }

    public class serialized_string
    {
        public string serialized_data = "";
        public void write_to_file(string filename)
        {
            File.WriteAllText(filename, serialized_data);
        }

    }

    public static class serializer_helper
    {
        public static T json_deserialize_object_from_string<T>(string objectData)
        {
            JsonSerializer sr = new JsonSerializer();
            return sr.deserialize_object_from_string<T>(objectData);
        }

        public static T json_deserialize_object_from_file<T>(string filename)
        {
            string objectData = File.ReadAllText(filename);
            JsonSerializer sr = new JsonSerializer();
            return sr.deserialize_object_from_string<T>(objectData);
        }

        public static string json_serialize_object_to_string(object obj)
        {
            JsonSerializer sr = new JsonSerializer();
            return sr.serialize_object_to_string(obj);
        }
        public static void json_serialize_object_to_string(object obj, string filename)
        {
            JsonSerializer sr = new JsonSerializer();
            sr.serialize_obj_to_serialized_string(obj).write_to_file(filename);
        }


        public static T xml_deserialize_object_from_string<T>(string objectData)
        {
            Xml_Serializer sr = new Xml_Serializer();
            return sr.deserialize_object_from_string<T>(objectData);
        }

        public static T xml_deserialize_object_from_file<T>(string filename)
        {
            string objectData = File.ReadAllText(filename);
            Xml_Serializer sr = new Xml_Serializer();
            return sr.deserialize_object_from_string<T>(objectData);
        }

        public static string xml_serialize_object_to_string(object obj)
        {
            Xml_Serializer sr = new Xml_Serializer();
            return sr.serialize_object_to_string(obj);
        }
        public static void xml_serialize_object_to_string(object obj, string filename)
        {
            Xml_Serializer sr = new Xml_Serializer();
            sr.serialize_obj_to_serialized_string(obj).write_to_file(filename);
        }


    }
    public class JsonSerializer : string_serializer
    {
        public override object deserialize_object_from_string(string json, Type incoming_type)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(incoming_type);
            //StreamReader
            object res;
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            res = serializer.ReadObject(ms);
            ms.Close();
            return res;

        }


        public override string serialize_object_to_string(object obj)
        {
            Type type = obj.GetType();
            string serializedobj = "";
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            /* //this serializes to small format (hardly readable to human)
             
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            ms.Position = 0;
            serializedobj = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            */

            DataContractJsonSerializerSettings Settings = new DataContractJsonSerializerSettings();
            Settings.UseSimpleDictionaryFormat = true;


            using (var memoryStream = new MemoryStream())
            {
                using (var xmlWriter = JsonReaderWriterFactory.CreateJsonWriter(memoryStream, Encoding.UTF8, true, true, "  "))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(type, Settings);
                    serializer.WriteObject(xmlWriter, obj);
                    xmlWriter.Flush();
                    //serializedobj = Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                    serializedobj = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }



            return serializedobj;
        }




    }

    public class Xml_Serializer : string_serializer
    {

        public override object deserialize_object_from_string(string xml, Type incoming_type)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            XmlSerializer xmlSerializer = new XmlSerializer(incoming_type);
            //StreamReader
            object res;

            using (TextReader reader = new StringReader(xml))
            {
                res = xmlSerializer.Deserialize(reader);
            }

            return res;
        }


        public override string serialize_object_to_string(object obj)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Type type = obj.GetType();
            string serializedobj = "";

            /*StringWriter textWriter = new StringWriter();     
            serializer.Serialize(textWriter, obj);
            serializedobj = textWriter.ToString();*/



            XmlSerializer serializer = new XmlSerializer(type);
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, obj);
            ms.Position = 0;
            serializedobj = System.Text.Encoding.UTF8.GetString(ms.ToArray());

            return serializedobj;
        }
    }
}
