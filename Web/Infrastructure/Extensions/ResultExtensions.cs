using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FluentValidation.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web.Infrastructure.HttpContentFormatters;
using Web.Models;

namespace Web.Infrastructure.Extensions
{
    public static class ResultExtensions
    {
        public static string ToJson(this Result result)
        {
            return JsonConvert.SerializeObject(result);
        }

        public static string ToJson(this ValidationError validationError)
        {
            return JsonConvert.SerializeObject(validationError);
        }

        public static Result ToResult<T>(this T data)
        {
            return new Result
            {
                Success = data != null,
                Message = new Message
                {
                    MessageType = data != null ? MessageType.Success : MessageType.Error,
                },
                
                Model = data,
                HttpStatusCode = data != null ? HttpStatusCode.OK : HttpStatusCode.InternalServerError
            };
        }

        public static Result ToResult<T>(this T data, string content)
        {
            return new Result
            {
                Success = data != null,
                Message = new Message
                {
                    MessageType = data != null ? MessageType.Success : MessageType.Error,
                    Content = content
                },

                Model = data,
                HttpStatusCode = data != null ? HttpStatusCode.OK : HttpStatusCode.InternalServerError
            };
        }

        public static Result ToResult<T>(this T data, string title, string content)
        {
            return new Result
            {
                Success = data != null,
                Message = new Message
                {
                    MessageType = data != null ? MessageType.Success : MessageType.Error,
                    Title = title,
                    Content = content
                },

                Model = data,
                HttpStatusCode = data != null ? HttpStatusCode.OK : HttpStatusCode.InternalServerError
            };
        }

        public static Result ToResult<T>(this IEnumerable<T> data)
        {
            return new Result
            {
                Success = data.Any(),
                Message = new Message
                {
                    MessageType = data.Any() ? MessageType.Success : MessageType.Error
                },
                Model = data,
                HttpStatusCode = data.Any() ? HttpStatusCode.OK : HttpStatusCode.InternalServerError
            };
        }

        public static Result ToResult(this IEnumerable<ValidationError> errors)
        {
            var currentErrors = errors.ToList();
            var result = new Result
            {
                Success = !currentErrors.Any(),
                
            };

            if (currentErrors.Any())
            {
                var message = new Message
                {
                    Title = "Error",
                    Content = "There were validation errors. Please see form for more details.",
                    DisplayMessage = true,
                    IsNotify = true,
                    IsSystem = true,
                    MessageType = MessageType.Error
                };
                result.HttpStatusCode = HttpStatusCode.BadRequest;
                result.Message = message;
                result.ValidationErrors = currentErrors;
            }

            return result;
        }

        public static JsonResult ToJsonResult(this ValidationResult validationResult)
        {
            var result = new Result();
           
            if (!validationResult.IsValid)
            {
                result.Success = false;
               
                result.Message = new Message
                {
                    Title = "Error",
                    Content = "There are validation errors.",
                    DisplayMessage = true,
                    IsSystem = true,
                    IsNotify = true
                };
               
                result.ValidationErrors = validationResult.Errors.Select(e => JsonConvert.DeserializeObject<ValidationError>(e.ErrorMessage)).ToList();
            }
            else
            {
                result.Success = true;
            }

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public static JsonResult ToJsonResult(string message, string title, bool success = true, bool displayMessage = true)
        {
            var result = new Result()
            {
                Success = success,

                Message = new Message()
                {
                    Content = message,
                    DisplayMessage = displayMessage,
                    IsSystem = true,
                    IsNotify = true
                }
            };

            return result.ToJsonResult();
        }
        public static JsonResult ToSuccessResult(string message, string title,  bool displayMessage = true)
        {
            return ToJsonResult(message, title, true, displayMessage);
        }

        public static JsonResult ToErrorResult(string message, string title, bool displayMessage = true)
        {
            return ToJsonResult(message, title, false, displayMessage);
        }

        public static JsonResult ToJsonResult(this Result result, bool modelToFormModel = false)
        {
           
            if (modelToFormModel && result.Model != null)
            {
                var json = JsonConvert.SerializeObject(result.Model);
                var jss = new JavaScriptSerializer();
                var o = jss.Deserialize<Dictionary<string, object>>(json);
                result.FormModel = new Dictionary<string, string>();
                BuildVariablesList(o, "", result.FormModel);
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public static HttpResponseMessage ToHttpResponseMessageJson(this Result result)
        {
            if (result.Model != null)
            {
                var json = JsonConvert.SerializeObject(result.Model);

                if (result.ModelIsForForm)
                {
                    var jss = new JavaScriptSerializer();
                    var o = jss.Deserialize<Dictionary<string, object>>(json);
                    result.FormModel = new Dictionary<string, string>();
                    BuildVariablesList(o, "", result.FormModel); 
                }
            }

            return new HttpResponseMessage(result.HttpStatusCode)
            {
                ReasonPhrase = result.Message != null && !string.IsNullOrEmpty(result.Message.Content) ? result.Message.Content : string.Empty,
                Content = new JsonContent(JObject.FromObject(result)) 

            };
        }

        private static string AppendToPathString(string path, object part, string delimeter = "_")
        {
            return path.Trim().Length == 0 ? part.ToString() : path + delimeter + part;
        }

        public static void BuildVariablesList(object obj, string path, Dictionary<string, string> result)
        {
            if (obj is ArrayList)
            {
                var arrayObj = obj as ArrayList;
                for (var i = 0; i < arrayObj.Count; i++)
                {
                    BuildVariablesList(arrayObj[i], AppendToPathString(path, i), result);
                }
            }
            else if (obj is Dictionary<string, object>)
            {
                var dictObject = obj as Dictionary<string, object>;
                foreach (var entry in dictObject)
                {

                    if (entry.Value is Dictionary<string, object>)
                    {
                        BuildVariablesList(entry.Value as Dictionary<string, object>, AppendToPathString(path, entry.Key), result);
                    }
                    else if (entry.Value is ArrayList)
                    {
                        BuildVariablesList(entry.Value as ArrayList, AppendToPathString(path, entry.Key), result);
                    }
                    else
                    {
                        result.Add(AppendToPathString(path, entry.Key),
                            entry.Value != null ? entry.Value.ToString() : string.Empty);
                    }
                }
            }
        }
    }
}
