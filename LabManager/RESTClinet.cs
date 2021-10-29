using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace LabManager
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RESTClinet
    {
        public string agvAddress { get; set; }
        public string rtlsAddress { get; set; }
        public httpVerb httpMethod { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string iterationNum { get; set; }
        public string intervalTime { get; set; }

        public RESTClinet()
        {
            agvAddress = "";
            rtlsAddress = "";
            httpMethod = httpVerb.GET;
        }

        public string[] MakeAGVRequest()
        {
            string strResponseValue = string.Empty;
            string result = string.Empty;
            string[] tempResult = new string[3];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(agvAddress);

            request.Method = httpMethod.ToString();

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                //Proecess the resppnse stream... (could be JSON, XML or HTML etc...)
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();

                            tempResult = ReadAGVJSON(strResponseValue, "position");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            //return result;
            return tempResult;
        }

        public string[] ReadAGVJSON(string jsonStr, string keyNameParent)
        {
            JObject json = JObject.Parse(jsonStr);
            string Orientation = "";
            string XValue = "";
            string YValue = "";

            string[] returnValue = new string[3];

            foreach (var data in json[keyNameParent])
            {
                JProperty jProperty = data.ToObject<JProperty>();

                if (jProperty.Name == "orientation") Orientation = jProperty.Value.ToString();
                else if (jProperty.Name == "x") XValue = jProperty.Value.ToString();
                else if (jProperty.Name == "y") YValue = jProperty.Value.ToString();
            }

            returnValue[0] = Orientation;
            returnValue[1] = XValue;
            returnValue[2] = YValue;

            return returnValue;
        }


        public string[] MakeRTLSRequest()
        {
            string strResponseValue = string.Empty;
            string result = string.Empty;

            // ObjID, Coordinate.X, Coordinate.Y, TimeStamp, Zone, Longitude, Latitude
            string[] tempResult = new string[7];
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rtlsAddress);

            request.Accept = "application/json";
            request.Headers.Add("X-Authenticate-User", userName);
            request.Headers.Add("X-Authenticate-Password", userPassword);
            request.Method = httpMethod.ToString();

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                var statuscode = response.StatusCode;

                //Proecess the resppnse stream... (could be JSON, XML or HTML etc...)
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            
                            tempResult = ReadRTLSJson(strResponseValue, "RootObject");
                        }
                    }
                }
           }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                tempResult[0] = string.Empty;
                tempResult[1] = string.Empty;
                tempResult[2] = string.Empty;
                string[] splitAddress = this.rtlsAddress.Split(new char[] { '/' });
                tempResult[3] = splitAddress[4];
                tempResult[4] = string.Empty;
                tempResult[5] = string.Empty;
                tempResult[6] = string.Empty;
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            //return result;
            return tempResult;
        }

        public string[] ReadRTLSJson(string jsonStr, string keyNameParent)
        {
            //JArray jarray = JArray.Parse(jsonStr);
            //var json = jarray[0]; // first element of json array
            JObject json = JObject.Parse(jsonStr);

            string Xvalue = "";
            string Yvalue = "";
            string TimeStamp = "";
            string ObjectID = "";
            string Zone = "";
            string longitude = "";
            string latitude = "";

            // Xvalue, Yvalue, TimeStamp, ObjectID, Zone
            string[] returnValue = new string[7];

            // Longitude, Latitude instead of X, Y ?
            ObjectID = (string)json.SelectToken("Object"); //0
            Xvalue = (string)json.SelectToken("X");//1
            Yvalue = (string)json.SelectToken("Y");//2
            TimeStamp = (string)json.SelectToken("Timestamp");//3
            Zone = (string)json.SelectToken("Zone");//4
            longitude = (string)json.SelectToken("Longitude");//5
            latitude = (string)json.SelectToken("Latitude");//6

            returnValue[0] = Xvalue;
            returnValue[1] = Yvalue;
            returnValue[2] = TimeStamp;
            returnValue[3] = ObjectID;
            returnValue[4] = Zone;
            returnValue[5] = longitude;
            returnValue[6] = latitude;

            return returnValue;
        }


        public string[] MakeRTLSposRequest()
        {
            string strResponseValue = string.Empty;
            string result = string.Empty;

            // ObjID, Coordinate.X, Coordinate.Y, TimeStamp, Zone, Longitude, Latitude
            string[] tempResult = new string[7];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rtlsAddress);

            request.Accept = "application/json";
            request.Headers.Add("X-Authenticate-User", userName);
            request.Headers.Add("X-Authenticate-Password", userPassword);
            request.Method = httpMethod.ToString();

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                var statuscode = response.StatusCode;

                //Proecess the resppnse stream... (could be JSON, XML or HTML etc...)
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();

                            tempResult = ReadRTLSposJson(strResponseValue, "RootObject");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                tempResult[0] = string.Empty;
                tempResult[1] = string.Empty;
                tempResult[2] = string.Empty;
                string[] splitAddress = this.rtlsAddress.Split(new char[] { '/', '=' });
                tempResult[3] = splitAddress[4];
                tempResult[4] = string.Empty;
                tempResult[5] = string.Empty;
                tempResult[6] = string.Empty;
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            //return result;
            return tempResult;
        }

        public string[] ReadRTLSposJson(string jsonStr, string keyNameParent)
        {
            JArray jarray = JArray.Parse(jsonStr);
            var json = jarray[0]; // first element of json array
            //JObject json = JObject.Parse(jsonStr);

            string Xvalue = "";
            string Yvalue = "";
            string TimeStamp = "";
            string ObjectID = "";
            string Zone = "";
            string longitude = "";
            string latitude = "";

            // Xvalue, Yvalue, TimeStamp, ObjectID, Zone
            string[] returnValue = new string[7];

            // Longitude, Latitude instead of X, Y ?
            ObjectID = (string)json.SelectToken("Object"); //0
            Xvalue = (string)json.SelectToken("X");//1
            Yvalue = (string)json.SelectToken("Y");//2
            TimeStamp = (string)json.SelectToken("Timestamp");//3
            Zone = (string)json.SelectToken("Zone");//4
            longitude = (string)json.SelectToken("Longitude");//5
            latitude = (string)json.SelectToken("Latitude");//6

            returnValue[0] = Xvalue;
            returnValue[1] = Yvalue;
            returnValue[2] = TimeStamp;
            returnValue[3] = ObjectID;
            returnValue[4] = Zone;
            returnValue[5] = longitude;
            returnValue[6] = latitude;

            return returnValue;
        }


        public List<string[]> MakeRTLSposRequest(int requiringNumofPoints)
        {
            string strResponseValue = string.Empty;
            //string result = string.Empty;

            List<string[]> result = new List<string[]>(requiringNumofPoints);

            // ObjID, Coordinate.X, Coordinate.Y, TimeStamp, Zone, Longitude, Latitude
            string[] tempResult = new string[7];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rtlsAddress);

            request.Accept = "application/json";
            request.Headers.Add("X-Authenticate-User", userName);
            request.Headers.Add("X-Authenticate-Password", userPassword);
            request.Method = httpMethod.ToString();

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                var statuscode = response.StatusCode;

                //Proecess the resppnse stream... (could be JSON, XML or HTML etc...)
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();

                            result = ReadRTLSposJson(strResponseValue, "RootObject", requiringNumofPoints);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                for (int i = 0; i < requiringNumofPoints; i++)
                {
                    strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                    result[i][0] = string.Empty;
                    result[i][1] = string.Empty;
                    result[i][2] = string.Empty;
                    string[] splitAddress = this.rtlsAddress.Split(new char[] { '/', '=' });
                    result[i][3] = splitAddress[4];
                    result[i][4] = string.Empty;
                    result[i][5] = string.Empty;
                    result[i][6] = string.Empty;
                }
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            //return result;
            return result;
        }


        public List<string[]> ReadRTLSposJson(string jsonStr, string keyNameParent, int requiringNumofPoints)
        {
            JArray jarray = JArray.Parse(jsonStr);
            
            List<string[]> result = new List<string[]>(requiringNumofPoints);
            //JObject json = JObject.Parse(jsonStr);

            string Xvalue = "";
            string Yvalue = "";
            string TimeStamp = "";
            string ObjectID = "";
            string Zone = "";
            string longitude = "";
            string latitude = "";

            // Xvalue, Yvalue, TimeStamp, ObjectID, Zone
            string[] returnValue = new string[7];

            for (int i = 0; i < requiringNumofPoints; i++)
            {
                var json = jarray[i]; // first element of json array

                ObjectID = (string)json.SelectToken("Object"); //0
                Xvalue = (string)json.SelectToken("X");//1
                Yvalue = (string)json.SelectToken("Y");//2
                TimeStamp = (string)json.SelectToken("Timestamp");//3
                Zone = (string)json.SelectToken("Zone");//4
                longitude = (string)json.SelectToken("Longitude");//5
                latitude = (string)json.SelectToken("Latitude");//6

                returnValue[0] = Xvalue;
                returnValue[1] = Yvalue;
                returnValue[2] = TimeStamp;
                returnValue[3] = ObjectID;
                returnValue[4] = Zone;
                returnValue[5] = longitude;
                returnValue[6] = latitude;

                result.Add(returnValue);
            }
            /*
            // Longitude, Latitude instead of X, Y ?
            ObjectID = (string)json.SelectToken("Object"); //0
            Xvalue = (string)json.SelectToken("X");//1
            Yvalue = (string)json.SelectToken("Y");//2
            TimeStamp = (string)json.SelectToken("Timestamp");//3
            Zone = (string)json.SelectToken("Zone");//4
            longitude = (string)json.SelectToken("Longitude");//5
            latitude = (string)json.SelectToken("Latitude");//6

            returnValue[0] = Xvalue;
            returnValue[1] = Yvalue;
            returnValue[2] = TimeStamp;
            returnValue[3] = ObjectID;
            returnValue[4] = Zone;
            returnValue[5] = longitude;
            returnValue[6] = latitude;
            */
            return result;
        }


        /// <summary>
        /// Get TAG IDs in RTLS system
        /// </summary>
        /// <returns></returns>
        public string[] GetIDs()
        {
            string strResponseValue = string.Empty;
            string result = string.Empty;
            string[] tempResult = new string[3];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rtlsAddress);

            request.Accept = "application/json";
            request.Headers.Add("X-Authenticate-User", userName);
            request.Headers.Add("X-Authenticate-Password", userPassword);
            request.Method = httpMethod.ToString();

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                var statuscode = response.StatusCode;

                //Proecess the resppnse stream... (could be JSON, XML or HTML etc...)
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();

                            tempResult = ReadRTLSTAGJson(strResponseValue, "RootObject");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            //return result;
            return tempResult;
        }

        public string[] ReadRTLSTAGJson(string jsonStr, string keyNameParent)
        {
            var jarray = JsonConvert.DeserializeObject<JArray>(jsonStr);

            string[] returnValue = new string[jarray.Count()];

            for (int i = 0; i < jarray.Count(); i++)
            {
                returnValue[i] = jarray[i].ToString();
            }

            return returnValue;
        }
    }
}
