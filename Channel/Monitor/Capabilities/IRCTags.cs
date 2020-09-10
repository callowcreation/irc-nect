#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: TwitchUnityIRC
     
*/
#endregion

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace IRCnect.Channel.Monitor.Capabilities
{
    /// <summary>
    /// IRC Tags holder to parse string tag into property name and value
    /// </summary>
    public abstract class IRCTags
    {
        const char EQUAL_SIGN = '=';
        const char SEMICOLON_SIGN = ';';

        /// <summary>
        /// Use a static property infos to reduce reflections lookups
        /// </summary>
        /// <typeparam name="T">Any IRCTags</typeparam>
        /// <returns></returns>
        protected static Dictionary<string, PropertyInfo> GetPropertyInfos<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .ToDictionary(m => m.Name, m => m);
        }

        /// <summary>
        /// Use a static property infos to reduce reflections lookups
        /// </summary>
        protected abstract Dictionary<string, PropertyInfo> propertyInfos { get; }

        /// <summary>
        /// Use a static property infos to reduce reflections lookups
        /// </summary>
        protected string[] properties { get; set; }

        /// <summary>
        /// The message send unchanged
        /// </summary>
        protected string rawData { get; set; }

        /// <summary>
        /// IRC Tags holder to parse string tag into property name and value
        /// </summary>
        public IRCTags() { }

        /// <summary>
        /// Parse string tag into property name and value
        /// <para>ie. @tag-one=value;tag-two=value</para>
        /// </summary>
        public abstract void Parse();

        /// <summary>
        /// Parse string tag into property name and value
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyInfos"></param>
        /// <param name="tag">Key value pairs key=value</param>
        /// <param name="valueAction">How to get the value</param>
        public static void DefaultParser(object target, Dictionary<string, PropertyInfo> propertyInfos, string[] tag, Func<PropertyInfo, string, object> valueAction)
        {
            string tagName = tag[0];
            string tagValue = tag.Length == 2 ? tag[1] : string.Empty;

            string propName = MakePropertyName(tagName);

            if (propertyInfos.TryGetValue(propName, out PropertyInfo propertyInfo) && propertyInfo.CanWrite)
            {
                object value = valueAction(propertyInfo, tagValue);
                propertyInfo.SetValue(target, value, null);
            }
            /*else
            {
                Console.WriteLine($"Tag Failed: {tag} {tagName} {tagValue}");
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetPropertyNamesToTagNames()
        {
            return properties.Select(x => x.Split(EQUAL_SIGN)[0]).ToDictionary(k => MakePropertyName(k), v => v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            Dictionary<string, string> propsToTags = GetPropertyNamesToTagNames();

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in propertyInfos)
            {
                stringBuilder.Append(propsToTags[item.Key]);
                stringBuilder.Append(EQUAL_SIGN);
                stringBuilder.Append(item.Value.GetValue(this, null));
                stringBuilder.Append(SEMICOLON_SIGN);
            }

            return stringBuilder.ToString().TrimEnd(SEMICOLON_SIGN);
        }

        /// <summary>
        /// Converts tag name to property name for reflections lookup
        /// <para>example: display-name or display_name would be converted displayName</para>
        /// </summary>
        /// <param name="tagName">The tag name which may contain - and a letter display-name for example</param>
        /// <returns></returns>
        public static string MakePropertyName(string tagName)
        {
            return Regex.Replace(tagName, @"-[a-z]|_[a-z]", Evaluator, RegexOptions.IgnoreCase);
        }

        static string Evaluator(Match match)
        {
            return match.ToString().ToUpper().Replace("-", "").Replace("_", "");
        }
        
        /// <summary>
        /// Gets the topic prototype properties
        /// <para>ie. @tag-one=value;tag-two=value;</para>
        /// </summary>
        /// <param name="info">Semicolon delimied list of key value pairs tag-one=value</param>
        /// <returns>Expected prototype properties</returns>
        public static T GetTags<T>(string info) where T : IRCTags, new()
        {
            string text = info.TrimStart('@');
            int indexof = text.IndexOf(' ');
            string[] properties = text.Substring(0, indexof > -1 ? indexof : text.Length).Trim().Split(SEMICOLON_SIGN);

            T instance = new T()
            {
                rawData = info,
                properties = properties
            };
            instance.Parse();
            return instance;
        }

        /// <summary>
        /// Gets the topic prototype properties
        /// <para>ie. @tag-one=value;tag-two=value;</para>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info">Semicolon delimied list of key value pairs tag-one=value</param>
        /// <returns>Expected prototype properties</returns>
        public static IRCTags GetTags(Type type, string info)
        {
            if (type == null) return null;

            string text = info.TrimStart('@');
            int indexof = text.IndexOf(' ');
            string[] properties = text.Substring(0, indexof > -1 ? indexof : text.Length).Trim().Split(';');

            IRCTags instance = (IRCTags)Activator.CreateInstance(type);
            instance.rawData = info;
            instance.properties = properties;
            instance.Parse();
            return instance;
        }
    }
}
