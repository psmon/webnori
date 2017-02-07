using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonModule;
using CommonModule.data;
using Nancy;

using System.Configuration;

#pragma warning disable 1998

namespace MainService.controllers
{
    public class BaseController : NancyModule
    {
        protected string parentSpace = "";
                
        public BaseController()
        {                        
            After += async (ctx, ct) =>
            {                
                // Modify ctx.Response
                if (ctx.Response.ContentType == "text/html")
                    ctx.Response.ContentType = "text/html; charset=utf-8";
            };
        }
        
        public string GetBodyString()
        {
            var body = this.Request.Body;
            int length = (int)body.Length; // this is a dynamic variable
            byte[] data = new byte[length];
            body.Read(data, 0, length);
            string dataStr = System.Text.Encoding.UTF8.GetString(data);            
            return dataStr;
        }

        public IDictionary<string, object> GetPayLoad()
        {
            var postData = Utils.ToJson(GetBodyString());
            IDictionary<string, object> payload = (IDictionary<string, object>)postData;
            return payload;
        }
    }
}
