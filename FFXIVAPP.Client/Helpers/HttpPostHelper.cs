﻿// FFXIVAPP.Client
// HttpPostHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FFXIVAPP.Client.Helpers
{
    public static class HttpPostHelper
    {
        public enum PostType
        {
            Json,
            Form
        }

        /// <summary>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsValidJson(string data)
        {
            try
            {
                var valid = JObject.Parse(data);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="postData"></param>
        public static string Post(string url, PostType type, string postData)
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
            switch (type)
            {
                case PostType.Json:
                    httpWebRequest.ContentType = "application/json";
                    break;
                case PostType.Form:
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    break;
            }
            httpWebRequest.ContentLength = Encoding.UTF8.GetByteCount(postData);
            httpWebRequest.Method = "POST";
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                    streamWriter.Flush();
                    streamWriter.Close();
                    var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                return "{\"result\":\"success\"}";
            }
        }
    }
}