using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        
        public string? ErrorMessage { get; set; }
        public object? ReturnValue { get; set; }

        public Response() { 
        }
        public Response(object responseValue, string errorMessage) 
        {
            ReturnValue = responseValue;
            ErrorMessage = errorMessage;
        }

        public string GetSerilizeResponse() {
            return JsonSerializer.Serialize(this);
        }
    }
}
