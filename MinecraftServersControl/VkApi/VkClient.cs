using PinkJson2;
using PinkJson2.KeyTransformers;
using PinkJson2.Serializers;
using System;
using System.Text.RegularExpressions;
using VkApi.Services;

namespace VkApi
{
    public sealed class VkClient
    {
        public string Token { get; }
        public GroupsVkService Groups { get; }
        public MessagesVkService Messages { get; }

        public const string Version = "5.131";
        public const string EndPoint = "https://api.vk.com/method/";

        public static ObjectSerializerOptions ObjectSerializerOptions { get; } = new ObjectSerializerOptions();

        static VkClient()
        {
            var snakeCaseKeyTransformer = new SnakeCaseKeyTransformer();

            ObjectSerializerOptions.TypeConverter.Register(typeof(Enum), new TypeConversion((object obj, Type targetType, ref bool handled) =>
            {
                if (obj is string type)
                {
                    handled = true;

                    type = char.ToUpper(type[0]) + type.Substring(1);
                    type = Regex.Replace(type, "_.", m => char.ToUpper(m.Value[1]).ToString());
                    if (Enum.TryParse(targetType, type, out object typeObj))
                        return typeObj;
                    else
                        return Enum.ToObject(targetType, -1);
                }
                if (obj is int number)
                {
                    handled = true;

                    return Enum.ToObject(targetType, number);
                }

                return null;
            }, (object obj, Type targetType, ref bool handled) =>
            {
                var enumType = obj.GetType();
                var attributes = (EnumDeserializerTypeAttribute[])enumType.GetCustomAttributes(typeof(EnumDeserializerTypeAttribute), false);
                if (attributes.Length == 0)
                    throw new Exception($"{enumType} does not have {nameof(EnumDeserializerTypeAttribute)} set");
                var enumValueType = attributes[0].Type;

                if (enumValueType == typeof(int))
                {
                    handled = true;
                    return (int)obj;
                }
                if (enumValueType == typeof(string))
                {
                    handled = true;
                    var name = Enum.GetName(enumType, obj);
                    if (name == null)
                        return null;
                    return snakeCaseKeyTransformer.TransformKey(name);
                }
                

                return null;
            }));
        }

        public VkClient(string token)
        {
            Token = token;
            Groups = new GroupsVkService(this);
            Messages = new MessagesVkService(this);
        }

        public VkRequest CreateRequest(string service, string method)
        {
            var request = new VkRequest(EndPoint + service + '.' + method);
            request.SetParameter("access_token", Token);
            request.SetParameter("v", Version);
            return request;
        }
    }
}
