using Newtonsoft.Json;
using RestSharp;
using RoomManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManagement.Control
{
    public static class API
    {
        public static ResponseAPI<T> Execute<T>(string url, object header, object query, object body, Method method, bool xmlHeader = false) where T : new()
        {
            try
            {
                RestClient client = new RestClient(url);
                RestRequest request = new RestRequest(method);

                if (header != null)
                {
                    var properties = from p in header.GetType().GetProperties()
                                     where p.GetValue(header, null) != null
                                     select p;

                    foreach (var item in properties)
                    {
                        request.AddHeader(item.Name, item.GetValue(header, null).ToString());
                    }
                }

                if (query != null)
                {
                    var properties = from p in query.GetType().GetProperties()
                                     where p.GetValue(query, null) != null
                                     select p;

                    foreach (var item in properties)
                    {
                        request.AddQueryParameter(item.Name, item.GetValue(query, null).ToString());
                    }

                }

                if (body != null)
                {
                    string strBody = JsonConvert.SerializeObject(body, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    if (!xmlHeader)
                        request.AddParameter("application/json", strBody, ParameterType.RequestBody);
                    else
                        request.AddParameter("application/xml", strBody, ParameterType.RequestBody);
                }

                IRestResponse response = client.Execute(request);

                List<System.Net.HttpStatusCode> StatusCodeSuccess = new List<System.Net.HttpStatusCode>();

                StatusCodeSuccess.Add(System.Net.HttpStatusCode.OK);
                StatusCodeSuccess.Add(System.Net.HttpStatusCode.Created);
                StatusCodeSuccess.Add(System.Net.HttpStatusCode.Accepted);

                if (StatusCodeSuccess.Contains(response.StatusCode))
                {
                    return new ResponseAPI<T>()
                    {
                        StatusCode = 200,
                        Object = JsonConvert.DeserializeObject<T>(response.Content),
                        Message = response.Content
                    };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new ResponseAPI<T>()
                    {
                        StatusCode = (int)response.StatusCode,
                        Object = default(T),
                        Message = ""
                    };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    return new ResponseAPI<T>()
                    {
                        StatusCode = (int)response.StatusCode,
                        Object = default(T),
                        Message = response.ErrorMessage
                    };
                }
                else
                {
                    return new ResponseAPI<T>()
                    {
                        StatusCode = (int)response.StatusCode,
                        Object = default(T),
                        Message = response.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<T>()
                {
                    StatusCode = 500,
                    Object = default(T),
                    Message = ex.Message
                };
            }
        }
    }
}
