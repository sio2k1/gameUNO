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
 * or
 * serializer_helper.json_serialize_object_to_string(obj,filename)
 * 
 * object_type obj = serializer_helper.json_deserialize_object_from_string<object_type>(str);
 * or 
 * object_type obj = json_deserialize_object_from_file<object_type>(filename);
  */


/* //binnary serialization example to implement in future in this classes
 *  public static void Save_(string file_, object data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(file_);
        bf.Serialize(file, data);
        file.Close();
    }

    public static T Load<T>(string file_)
    {
        T data= default(T);
        if (File.Exists(file_))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(file_, FileMode.Open);
            data = (T)bf.Deserialize(file);
            file.Close();
        }
        return data;
    }

    [Serializable]
    class PlayerData
    {
        public float health;
        public float experience;
    }
 * 
 * 
 * */

namespace cmn_infrastructure 
{

    public class string_serializer // base class for serialization
    {
        public T deserialize_object_from_string<T>(string objectData) //wraper for simple syntrax usage
        {
            return (T)deserialize_object_from_string(objectData, typeof(T));
        }

        public virtual object deserialize_object_from_string(string json, Type incoming_type) // deserealization in nested classes
        {
            return null;
        }

        public serialized_string serialize_obj_to_serialized_string(object obj) //serializing obj to serialized_string
        {
            serialized_string sr = new serialized_string();
            sr.serialized_data = serialize_object_to_string(obj);
            return sr;
        }
        public virtual string serialize_object_to_string(object obj) // serealization in nested classes
        {
            return "";
        }
    }


    public static class string_writer_reader // using to read and write files
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

    public class serialized_string // serialized string with ability to write its to file
    {
        public string serialized_data = "";
        public void write_to_file(string filename)
        {
            File.WriteAllText(filename, serialized_data);
        }

    }

    public static class serializer_helper //helper class we use to serialize\deserialize objects from to strings or files and determine if we need JSON or XML
    {
        public static T json_deserialize_object_from_string<T>(string objectData) // deserealize string and return object of T
        {
            JsonSerializer sr = new JsonSerializer();
            return sr.deserialize_object_from_string<T>(objectData);
        }

        public static T json_deserialize_object_from_file<T>(string filename) // deserealize file and return object of T
        {
            string objectData = File.ReadAllText(filename);
            JsonSerializer sr = new JsonSerializer();
            return sr.deserialize_object_from_string<T>(objectData);
        }

        public static string json_serialize_object_to_string(object obj) // serealize and return serialized str
        {
            JsonSerializer sr = new JsonSerializer();
            return sr.serialize_object_to_string(obj);
        }
        public static void json_serialize_object_to_string(object obj, string filename) // serealize and write to file
        {
            JsonSerializer sr = new JsonSerializer();
            sr.serialize_obj_to_serialized_string(obj).write_to_file(filename);
        }


        public static T xml_deserialize_object_from_string<T>(string objectData) // deserealize string and return object of T
        {
            Xml_Serializer sr = new Xml_Serializer();
            return sr.deserialize_object_from_string<T>(objectData);
        }

        public static T xml_deserialize_object_from_file<T>(string filename)// deserealize file and return object of T
        {
            string objectData = File.ReadAllText(filename);
            Xml_Serializer sr = new Xml_Serializer();
            return sr.deserialize_object_from_string<T>(objectData);
        }

        public static string xml_serialize_object_to_string(object obj) // serealize and return serialized str
        {
            Xml_Serializer sr = new Xml_Serializer();
            return sr.serialize_object_to_string(obj);
        }
        public static void xml_serialize_object_to_string(object obj, string filename)  // serealize and write to file
        {
            Xml_Serializer sr = new Xml_Serializer();
            sr.serialize_obj_to_serialized_string(obj).write_to_file(filename);
        }


    }
    public class JsonSerializer : string_serializer // used for json serialization\deserialization
    {
        public override object deserialize_object_from_string(string json, Type incoming_type)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; //in different computers we will have same result for date, time, etc...
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(incoming_type);
            object res;
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)); //using UTF8
            res = serializer.ReadObject(ms);
            ms.Close();
            return res;
        }


        public override string serialize_object_to_string(object obj)
        {
            Type type = obj.GetType(); //getting incomming obj type
            string serializedobj = "";
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;  //in different computers we will have same result for date, time, etc...

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
                using (var xmlWriter = JsonReaderWriterFactory.CreateJsonWriter(memoryStream, Encoding.UTF8, true, true, "  ")) // we using this to get human readeble json, otherwise it will be like one string lane without \r\n and spaces
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(type, Settings);
                    serializer.WriteObject(xmlWriter, obj);
                    xmlWriter.Flush();
                    serializedobj = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());  //using UTF8
                }
            }
            return serializedobj;
        }




    }

    public class Xml_Serializer : string_serializer // used for XML serialization\deserialization
    {

        public override object deserialize_object_from_string(string xml, Type incoming_type) //XML deserialization
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;  //in different computers we will have same result for date, time, etc...
            XmlSerializer xmlSerializer = new XmlSerializer(incoming_type);
            object res;
            using (TextReader reader = new StringReader(xml))
            {
                res = xmlSerializer.Deserialize(reader);
            }

            return res;
        }


        public override string serialize_object_to_string(object obj) //XML serialization
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;  //in different computers we will have same result for date, time, etc...
            Type type = obj.GetType(); // get type of incomming obj
            string serializedobj = "";
            XmlSerializer serializer = new XmlSerializer(type);
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, obj);
            ms.Position = 0;
            serializedobj = System.Text.Encoding.UTF8.GetString(ms.ToArray()); //using utf8

            return serializedobj;
        }
    }
}
