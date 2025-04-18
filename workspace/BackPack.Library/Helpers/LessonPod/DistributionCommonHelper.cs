using BackPack.Library.Requests.LessonPod;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.Json;

namespace BackPack.Library.Helpers.LessonPod
{
    public static class DistributionCommonHelper
    {
        #region JArrayToStringList
        public static List<string> JArrayToStringList(JArray jArray)
        {
            List<string> listString = [];
            if (jArray != null && jArray.ToString() != "" && jArray.Count > 0)
            {
                foreach (var item in jArray)
                {
                    if (item.ToString() != "")
                    {
                        listString.Add(item.ToString());
                    }                    
                }
            }
            return listString;
        }
        #endregion

        #region JArrayToListStringList
        public static List<List<string>> JArrayToListStringList(JArray jArray)
        {
            List<List<string>> listListString = [];

            if (jArray != null && jArray.ToString() != "" && jArray.Count > 0)
            {
                foreach (var item in jArray)
                {
                    List<string> listString = [];
                    if (item.ToString() != "")
                    {
                        foreach (var text in (JArray)item)
                        {
                            listString.Add(text.ToString());
                        }
                        listListString.Add(listString);
                    }
                }
            }

            return listListString;
        }
        #endregion

        #region JArrayToIntList
        public static List<int> JArrayToIntList(JArray jArray)
        {
            List<int> listInt = [];

            if (jArray != null && jArray.ToString() != "" && jArray.Count > 0)
            {
                foreach (var item in jArray)
                {
                    listInt.Add((int)item);
                }
            }

            return listInt;
        }
        #endregion

        #region JArrayToFloatList
        public static List<float> JArrayToFloatList(JArray jArray)
        {
            List<float> listFloat = [];

            if (jArray != null && jArray.ToString() != "" && jArray.Count > 0)
            {
                foreach (var item in jArray)
                {
                    listFloat.Add((float)item);
                }
            }

            return listFloat;
        }
        #endregion

        #region JArrayToBooleanList
        public static List<bool> JArrayToBooleanList(JArray jArray)
        {
            List<bool> listBool = [];
            if (jArray != null && jArray.ToString() != "" && jArray.Count > 0)
            {
                foreach (var item in jArray)
                {
                    listBool.Add((bool)item);
                }
            }
            return listBool;
        }
        #endregion

        #region JArrayToFloat
        public static float JArrayToFloat(JArray jArray, int firstAttribute, string secondAttribute)
        {
            float response = (jArray![firstAttribute][secondAttribute] != null && jArray![firstAttribute][secondAttribute]!.ToString() != "") ? (float)jArray![firstAttribute][secondAttribute]! : 0;

            return response;
        }
        #endregion

        #region JObjectToString
        public static string JObjectToString(JObject jObject, string attribute)
        {
            string response = (jObject[attribute] != null && jObject[attribute]!.ToString() != "") ? jObject[attribute]!.ToString() : "";

            return response;
        }
        #endregion

        #region JObjectToString
        public static string JObjectToString(JObject jObject, string firstAttribute, string secondAttribute)
        {
            string response = (jObject[firstAttribute]![secondAttribute] != null && jObject[firstAttribute]![secondAttribute]!.ToString() != "") ? jObject[firstAttribute]![secondAttribute]!.ToString() : "";

            return response;
        }
        #endregion

        #region JObjectToString
        public static string JObjectToString(JObject jObject, string firstAttribute, string secondAttribute, string thirdAttribute)
        {
            string response = (jObject[firstAttribute]![secondAttribute]![thirdAttribute] != null && jObject[firstAttribute]![secondAttribute]![thirdAttribute]!.ToString() != "") ? jObject[firstAttribute]![secondAttribute]![thirdAttribute]!.ToString() : "";

            return response;
        }
        #endregion

        #region JObjectToString
        public static string JObjectToString(JObject jObject, string firstAttribute, string secondAttribute, string thirdAttribute, int index)
        {
            string response = (jObject[firstAttribute]![secondAttribute]![thirdAttribute]![index] != null && jObject[firstAttribute]![secondAttribute]![thirdAttribute]![index]!.ToString() != "") ? jObject[firstAttribute]![secondAttribute]![thirdAttribute]![index]!.ToString() : "";

            return response;
        }
        #endregion

        #region JObjectToString
        public static string JObjectToString(JObject jObject, string firstAttribute, string secondAttribute, string thirdAttribute, string fourthAttribute, string fifthAttribute)
        {
            string response = (jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]![fifthAttribute] != null && jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]![fifthAttribute]!.ToString() != "") ? jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]![fifthAttribute]!.ToString() : "";

            return response;
        }
        #endregion

        #region JObjectToString
        public static string JObjectToString(LessonpodHelperMethodRequest request)
        {
            string response = (request.Jobject[request.FirstAttribute]![request.SecondAttribute]![request.ThirdAttribute]![request.FourthAttribute]![request.FifthAttribute]![request.SixthAttribute]![request.Index] != null && request.Jobject[request.FirstAttribute]![request.SecondAttribute]![request.ThirdAttribute]![request.FourthAttribute]![request.FifthAttribute]![request.SixthAttribute]![request.Index]!.ToString() != "") ? request.Jobject[request.FirstAttribute]![request.SecondAttribute]![request.ThirdAttribute]![request.FourthAttribute]![request.FifthAttribute]![request.SixthAttribute]![request.Index]!.ToString() : "";
            
            return response;
        }
        #endregion

        #region JObjectToInteger
        public static int JObjectToInteger(JObject jObject, string attribute)
        {
            int response = (jObject[attribute] != null && jObject[attribute]!.ToString() != "") ? (int)jObject[attribute]! : 0;

            return response;
        }
        #endregion

        #region JObjectToInteger
        public static int JObjectToInteger(JObject jObject, string firstAttribute, string secondAttribute)
        {
            int response = (jObject[firstAttribute]![secondAttribute] != null && jObject[firstAttribute]![secondAttribute]!.ToString() != "") ? (int)jObject[firstAttribute]![secondAttribute]! : 0;

            return response;
        }
        #endregion

        #region JObjectToFloat
        public static float JObjectToFloat(JObject jObject, string attribute)
        {
            float response = (jObject[attribute] != null && jObject[attribute]!.ToString() != "") ? (float)jObject[attribute]! : 0;

            return response;
        }
        #endregion

        #region JObjectToFloat
        public static float JObjectToFloat(JObject jObject, string firstAttribute, string secondAttribute)
        {
            float response = (jObject[firstAttribute]![secondAttribute] != null && jObject[firstAttribute]![secondAttribute]!.ToString() != "") ? (float)jObject[firstAttribute]![secondAttribute]! : 0;

            return response;
        }
        #endregion
        
        #region JObjectToBoolean
        public static bool JObjectToBoolean(JObject jObject, string attribute)
        {
            bool response = (jObject[attribute] != null && jObject[attribute]!.ToString() != "") && (bool)jObject[attribute]!;

            return response;
        }
        #endregion

        #region JObjectToBoolean
        public static bool JObjectToBoolean(JObject jObject, string firstAttribute, string secondAttribute)
        {
            bool response = (jObject[firstAttribute]![secondAttribute] != null && jObject[firstAttribute]![secondAttribute]!.ToString() != "") && (bool)jObject[firstAttribute]![secondAttribute]!;

            return response;
        }
        #endregion

        #region JObjectToBoolean
        public static bool JObjectToBoolean(JObject jObject, string firstAttribute, string secondAttribute, string thirdAttribute)
        {
            bool response = (jObject[firstAttribute]![secondAttribute]![thirdAttribute] != null && jObject[firstAttribute]![secondAttribute]![thirdAttribute]!.ToString() != "") && (bool)jObject[firstAttribute]![secondAttribute]![thirdAttribute]!;

            return response;
        }
        #endregion

        #region JObjectToJArray
        public static JArray JObjectToJArray(JObject jObject, string firstAttribute)
        {
            JArray response = [];
            if (jObject[firstAttribute] != null && jObject[firstAttribute]!.ToString() != "")
            {
                response = (JArray)jObject[firstAttribute]!;
            }

            return response;
        }
        #endregion

        #region JObjectToJArray
        public static JArray JObjectToJArray(JObject jObject, string firstAttribute, string secondAttribute)
        {
            JArray response = [];
            if (jObject[firstAttribute]![secondAttribute] != null && jObject[firstAttribute]![secondAttribute]!.ToString() != "")
            {
                response = (JArray)jObject[firstAttribute]![secondAttribute]!;
            }

            return response;
        }
        #endregion

        #region JObjectToJArray
        public static JArray JObjectToJArray(JObject jObject, string firstAttribute, string secondAttribute, string thirdAttribute, string fourthAttribute)
        {
            JArray response = [];
            if (jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute] != null && jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]!.ToString() != "")
            {
                response = (JArray)jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]!;
            }

            return response;
        }
        #endregion

        #region JObjectToJArray
        public static JArray JObjectToJArray(JObject jObject, string firstAttribute, string secondAttribute, string thirdAttribute, string fourthAttribute, string fifthAttribute, string sixthAttribute)
        {
            JArray response = [];
            if (jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]![fifthAttribute]![sixthAttribute] != null && jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]![fifthAttribute]![sixthAttribute]!.ToString() != "")
            {
                response = (JArray)jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]![fifthAttribute]![sixthAttribute]!;
            }

            return response;
        }
        #endregion

        #region JObjectToJObject
        public static JObject JObjectToJObject(JObject jObject, string firstAttribute)
        {
            JObject response = [];
            if (jObject[firstAttribute] != null && jObject[firstAttribute]!.ToString() != "")
            {
                response = (JObject)jObject[firstAttribute]!;
            }

            return response;
        }
        #endregion

        #region JObjectToJObject
        public static JObject JObjectToJObject(JObject jObject, string firstAttribute, string secondAttribute)
        {
            JObject response = [];
            if (jObject[firstAttribute]![secondAttribute] != null && jObject[firstAttribute]![secondAttribute]!.ToString() != "")
            {
                response = (JObject)jObject[firstAttribute]![secondAttribute]!;
            }

            return response;
        }
        #endregion

        #region JObjectToJObject
        public static JObject JObjectToJObject(JObject jObject, string firstAttribute, string secondAttribute, string thirdAttribute)
        {
            JObject response = [];
            if (jObject[firstAttribute]![secondAttribute]![thirdAttribute] != null && jObject[firstAttribute]![secondAttribute]![thirdAttribute]!.ToString() != "")
            {
                response = (JObject)jObject[firstAttribute]![secondAttribute]![thirdAttribute]!;
            }

            return response;
        }
        #endregion

        #region JObjectToJObject
        public static JObject JObjectToJObject(JObject jObject, string firstAttribute, string secondAttribute, string thirdAttribute, string fourthAttribute)
        {
            JObject response = [];
            if (jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute] != null && jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]!.ToString() != "")
            {
                response = (JObject)jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]!;
            }

            return response;
        }
        #endregion

        #region JObjectToJObject
        public static JObject JObjectToJObject(JObject jObject, string firstAttribute, string secondAttribute, string thirdAttribute, string fourthAttribute, string fifthAttribute)
        {
            JObject response = [];
            if (jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]![fifthAttribute] != null && jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]![fifthAttribute]!.ToString() != "")
            {
                response = (JObject)jObject[firstAttribute]![secondAttribute]![thirdAttribute]![fourthAttribute]![fifthAttribute]!;
            }

            return response;
        }
        #endregion

        #region JObjectToObject
        public static object JObjectToObject(JObject jObject, string firstAttribute)
        {
            object? response = new();
            if (jObject[firstAttribute] != null && jObject[firstAttribute]!.ToString() != "")
            {
                JObject data = (JObject)jObject[firstAttribute]!;
                response = (data != null) ? JsonSerializer.Deserialize<object>(data.ToString()) : new();
            }

            return response!;
        }
        #endregion

        #region JObjectToObject
        public static object JObjectToObject(JObject jObject, string firstAttribute, string secondAttribute)
        {
            object? response = new();
            if (jObject[firstAttribute]![secondAttribute] != null && jObject[firstAttribute]![secondAttribute]!.ToString() != "")
            {
                JObject data = (JObject)jObject[firstAttribute]![secondAttribute]!;
                response = (data != null) ? JsonSerializer.Deserialize<object>(data.ToString()) : new();
            }

            return response!;
        }
        #endregion

        #region JTokenToInteger
        public static int JTokenToInteger(JToken token)
        {
            try
            {
                int response = (token != null && token.ToString() != "") ? int.Parse(token.ToString()) : 0;

                return response;
            }
            catch (Exception)
            {
                return 0;
            }            
        }
        #endregion

        #region JTokenToBoolean
        public static bool JTokenToBoolean(JToken token)
        {
            bool response = (token != null && token.ToString() != "") && bool.Parse(token.ToString());

            return response;
        }
        #endregion

        #region JTokenToFloat
        public static float JTokenToFloat(JToken token)
        {
            float response = (token != null && token.ToString() != "") ? float.Parse(token.ToString()) : 0;

            return response;
        }
        #endregion

        #region JTokenToFloat
        public static float JTokenToFloat(JToken token, string attribute)
        {
            return (token[attribute] != null && !string.IsNullOrEmpty(token[attribute]!.ToString())) ? (float)token[attribute]! : 0;
        }
        #endregion

        #region JTokenToString
        public static string JTokenToString(JToken token)
        {
            string response = (token != null && token.ToString() != "") ? token.ToString() : "";

            return response;
        }
        #endregion

        #region StringToTime
        public static TimeOnly StringToTime(string time)
        {
            TimeOnly response = new();
            if (!string.IsNullOrEmpty(time))
            {
                CultureInfo culture = new("en-US");
                response = TimeOnly.Parse(time, culture);
            }

            return response;
        }
        #endregion

        #region StringToDate
        public static DateTime StringToDate(string text)
        {
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            DateTime response = DateTime.Parse(text, cultureInfo);
            return response;
        }
        #endregion
    }
}
