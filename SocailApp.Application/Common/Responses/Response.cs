using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocailApp.Application.Common.Responses
{
    public class Response<T>
    {
        public static Response<T> Fail(Error error, T data = default) => new Response<T>(false, data, error);
        public static Response<T> Success(T data) => new Response<T>(false, data, null);

        private Response(bool succeeded, T data, Error error)
        {
            Succeeded = succeeded;
            Data = data;
            Error = error;
        }

        public bool Succeeded { get; set; }
        public T Data { get; set; }
        public Error Error { get; set; }
    }

    public class Error
    {
        #region Properties
        public string RequestId { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }

        private Dictionary<string, object> _details = new Dictionary<string, object>();
        public IReadOnlyDictionary<string, object> Details => _details;

        #endregion

        private Error(string title, string message)
        {
            Title = title;
            Message = message;
        }

        #region Factories
        public static Error Create(string title, string message, Dictionary<string, object>? details)
        {
            Error error = new Error(title, message);
            if (details != null)
                error._details = details;
            return error;
        }

        public static Error Create(string title, string message)
        {
            return Create(title, message, null);
        }
        #endregion

        #region Methods

        public void SetRequestId(string requestId)
        {
            RequestId = requestId;
        }

        public void AddError(string key, string value)
        {
            _details.Add(key, value);
        }

        #endregion
    }
}
