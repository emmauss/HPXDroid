using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace HappyPandaXDroid.Core
{
    class JSON
    {

        public class ServerObject
        {
            public string session;
            public string name;
            public object data;
        }

        public class DataObject
        {
            public string fname;
            public object data;
            public App.Server.ErrorObject error;
        }

        public  class Serializer
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public static Serializer SimpleSerializer = new Serializer();
            public string Serialize<T>( T obj )
            {
                return JsonConvert.SerializeObject(obj, obj.GetType(), null);
            }

            

            public T Deserialize<T>(string serialized_string)
            {
                object json = null;
                try
                {
                    json = JsonConvert.DeserializeObject<T>(serialized_string);
                }catch(Exception ex)
                {
                    
                }
                return (T)json;
            }

            public List<T> DeserializeToList<T>(string serialized_string)
            {

                var array = JArray.Parse(serialized_string);

                List<T> objectsList = new List<T>();
                
                foreach (var item in array)

                {
                    try

                    {
                        // CorrectElements
                        objectsList.Add(item.ToObject<T>());
                    }

                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In JSON.Serializer.DeserializeToString. string ={0}",serialized_string);

                    }
                }
                return objectsList;
            }
        }

        public class API
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public static void PushKey(ref List<Tuple<string, string>> request, string name, string value)
            {
                request.Add(new Tuple<string, string>(name,value) );
            }

            public static string ParseToString(List<Tuple<string, string>> request)
            {
                string jsonstring = string.Empty;
                jsonstring += "{ " + System.Environment.NewLine;
                foreach (Tuple<string, string> s in request)
                {
                    jsonstring += '"' + s.Item1 + '"' + " : ";
                    if (!s.Item2.StartsWith("{") && !s.Item2.StartsWith("["))
                    {
                        if (s.Item2.Contains("<int>"))
                            jsonstring += s.Item2.Replace("<int>", "") + "," + System.Environment.NewLine;
                        else if (s.Item2.Contains("<bool>"))
                            jsonstring += s.Item2.Replace("<bool>", "") + "," + System.Environment.NewLine;
                        else
                            jsonstring += '"' + s.Item2 + '"' + "," + System.Environment.NewLine;
                    }
                    else
                        jsonstring += s.Item2 + "," + System.Environment.NewLine;

                }
               
                jsonstring=jsonstring.Remove(jsonstring.LastIndexOf(","),1);
                jsonstring += "}";

                return jsonstring;
            }

            public static string GetData(string jsonstring, int level = 1)
            {
                //recursive get data segment
                string data = jsonstring;
                int startindex = 0,length = 0;
                string temp = data;
                try
                {
                    for (int i = 0; i < level; i++)
                    {

                        startindex = temp.IndexOf("data");
                        temp = temp.Substring(startindex);
                        startindex = temp.IndexOf(':') + 1;
                        length = temp.LastIndexOf("}") - (startindex);
                        temp = temp.Substring(startindex, length);
                    }
                }catch(Exception ex)
                {

                }
                return temp;
                
            }
            
        }
    }
}