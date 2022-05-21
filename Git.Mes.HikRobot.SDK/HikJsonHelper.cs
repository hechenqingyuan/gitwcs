/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2016-11-14 22:12:23
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2016-11-14 22:12:23       情缘
*********************************************************************************/

using Git.Framework.DataTypes.ExtensionMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Mes.HikRobot.SDK
{
    public static class HikJsonHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string SerializeObject(object value)
        {
            if (value != null)
            {
                IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                return JsonConvert.SerializeObject(value, Formatting.None, timeConverter);
            }
            return string.Empty;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                return JsonConvert.DeserializeObject<T>(value, timeConverter);
            }
            return default(T);
        }


        /// <summary>
        /// 扩展字符串方法，直接将字符串转换为JSON对象
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static JObject ToJsonObject(this string Result)
        {
            if (Result.IsEmpty())
            {
                return null;
            }

            JObject param = JObject.Parse(Result);

            return param;
        }

        /// <summary>
        /// 扩展字符串方法，直接将字符串转换为JSON数组
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static JArray ToJsonArray(this string Result)
        {
            if (Result.IsEmpty())
            {
                return null;
            }

            JArray param = JArray.Parse(Result);

            return param;
        }

        /// <summary>
        /// 扩展对象方法,将对象直接转换为JSON对象
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static JObject ToJsonObject<T>(this T Result) where T : class
        {
            if (Result == null)
            {
                return null;
            }

            JObject param = JObject.Parse(HikJsonHelper.SerializeObject(Result));

            return param;
        }

        /// <summary>
        /// 扩展对象方法，将集合直接转换为JSON数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static JArray ToJsonArray<T>(this IEnumerable<T> Result)
        {
            if (Result.IsNullOrEmpty())
            {
                return null;
            }

            JArray param = JArray.Parse(HikJsonHelper.SerializeObject(Result));

            return param;
        }
    }
}
