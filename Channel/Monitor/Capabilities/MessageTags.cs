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

namespace IRCnect.Channel.Monitor.Capabilities
{
    /// <summary>
    /// IRC Tags holder to parse string tag into property name and value
    /// </summary>
    public abstract class MessageTags
    {
        /// <summary>
        /// Use a static property infos to reduce reflections lookups
        /// </summary>
        /// <typeparam name="T">Any MessageTags</typeparam>
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
        protected string[] properties { get; private set; }

        /// <summary>
        /// IRC Tags holder to parse string tag into property name and value
        /// </summary>
        public MessageTags() { }

        /// <summary>
        /// Parse string tag into property name and value
        /// <para>ie. @tag-one=value;tag-two=value</para>
        /// </summary>
        /// <param name="tags">IRC Message tags string ie. @tag-name=value</param>
        public abstract void Parser(string[] tags);

        /// <summary>
        /// Parse string tag into property name and value
        /// </summary>
        /// <param name="tag">Key value pairs key=valye</param>
        /// <param name="valueAction">How to get the value</param>
        protected void DefaultParser(string[] tag, Func<PropertyInfo, string, object> valueAction)
        {
            string tagName = tag[0];
            string tagValue = tag.Length == 2 ? tag[1] : string.Empty;

            string propName = MakePropName(tagName);
      
            if (propertyInfos.TryGetValue(propName, out PropertyInfo propertyInfo) && propertyInfo.CanWrite)
            {
                object value = valueAction(propertyInfo, tagValue);
                propertyInfo.SetValue(this, value, null);
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
        public override string ToString()
        {
            return string.Join(";", properties);
        }

        /// <summary>
        /// Converts tag name to property name for reflections lookup
        /// <para>example: display-name or display_name would be converted displayName</para>
        /// </summary>
        /// <param name="tagName">The tag name which may contain - and a letter display-name for example</param>
        /// <returns></returns>
        protected static string MakePropName(string tagName)
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
        public static T GetTags<T>(string info) where T : MessageTags, new()
        {
            string text = info.TrimStart('@');
            int indexof = text.IndexOf(' ');
            string[] properties = text.Substring(0, indexof > -1 ? indexof : text.Length).Trim().Split(';');

            /*for (int i = 0; i < properties.Length; i++)
            {
                Console.WriteLine($"{properties[i]}");
            }*/

            T instance = new T();
            instance.Parser(properties);
            instance.properties = properties;
            return instance;
        }

        /// <summary>
        /// Gets the topic prototype properties
        /// <para>ie. @tag-one=value;tag-two=value;</para>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info">Semicolon delimied list of key value pairs tag-one=value</param>
        /// <returns>Expected prototype properties</returns>
        public static MessageTags GetTags(Type type, string info)
        {
            if (type == null) return null;

            string text = info.TrimStart('@');
            int indexof = text.IndexOf(' ');
            string[] properties = text.Substring(0, indexof > -1 ? indexof : text.Length).Trim().Split(';');

            /*for (int i = 0; i < properties.Length; i++)
            {
                Console.WriteLine($"{properties[i]}");
            }*/
            MessageTags instance = (MessageTags)Activator.CreateInstance(type);
            instance.Parser(properties);
            instance.properties = properties;
            return instance;
        }
    }
}
