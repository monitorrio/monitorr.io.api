using System;
using System.Web.Script.Serialization;

namespace Web.Infrastructure.DataTables.Helpers
{
    public class JsonSerializationUtil
    {
        /// <summary>
        /// Returns strongly typed object, with full recursion, from JSON string.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="jsonString">String to deserialize</param>
        /// <returns></returns>
        public static T GetDeserializedObject<T>(string jsonString)
            where T : class, new()
        {
            return GetDeserializedObject<T>(jsonString, null);
        }

        /// <summary>
        /// Returns strongly typed object, with full recursion, from JSON string.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="jsonString">String to deserialize</param>
        /// <param name="jsCnvtr">Specific JavaScriptConverter implementation to handle all serialization/deserialization.</param>
        /// <returns></returns>
        public static T GetDeserializedObject<T>(string jsonString, JavaScriptConverter jsCnvtr)
            where T : class, new()
        {
            var jsSerializer = new JavaScriptSerializer();
            if (jsCnvtr != null)
                jsSerializer.RegisterConverters(new JavaScriptConverter[] { jsCnvtr });
            return jsSerializer.Deserialize<T>(jsonString);
        }

        /// <summary>
        /// Serialize an object graph to a JSON string
        /// </summary>
        /// <param name="data">The object to be serialized.</param>
        /// <returns></returns>
        public static string GetSerializedJsonString(Object data)
        {
            var jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(data);
        }

        /// <summary>
        /// Serialize an object graph to a JSON string
        /// </summary>
        /// <param name="data">The object to be serialized.</param>
        /// <param name="jsCnvtr">Specific JavaScriptConverter implementation to handle all serialization/deserialization.</param>
        /// <returns></returns>
        public static string GetSerializedJsonString(object data, JavaScriptConverter jsCnvtr)
        {
            var jsSerializer = new JavaScriptSerializer();
            jsSerializer.RegisterConverters(new[] { jsCnvtr });
            return jsSerializer.Serialize(data);
        }
    }
}
