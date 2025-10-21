using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFICamarAPI.Application.Common.Models
{
    public class Result
    {
        public bool Succeed { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string[] Messages { get; set; }
        public object Optional { get; set; }
        public Result(bool succeed, string status, string statusCode, IEnumerable<string> messages, object optional)
        {
            Succeed = succeed;
            Status = status;
            StatusCode = statusCode;
            Messages = messages?.ToArray() ?? Array.Empty<string>();
            Optional = optional;
        }

        public static Result Success(string status = "Success", string statusCode = "", IEnumerable<string> messages = null, object optional = null)
        {
            return new Result(true, status, statusCode, messages ?? Array.Empty<string>(), optional);
        }

        public static Result Failure(string status = "Failed", string statusCode = "", IEnumerable<string> messages = null, object optional = null)
        {
            return new Result(false, status, statusCode, messages ?? Array.Empty<string>(), optional);
        }
    }
}
