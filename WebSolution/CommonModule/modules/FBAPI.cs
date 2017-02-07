using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.Xml;

using CommonModule.data;

namespace CommonModule
{
    public class FBAPI
    {
        static public DBLoginInfo GetUserData(string accessToken)
        {
            DBLoginInfo resultValue = new DBLoginInfo();
            try
            {
                WebClient webclient = new WebClient();
                string URL = string.Format("https://graph.facebook.com/v2.8/me?access_token={0}&fields=id%2Cname%2Cemail&format=json&method=get&pretty=0", accessToken);
                string response = webclient.DownloadString(URL);
                object responseObj = new JavaScriptSerializer().DeserializeObject(response);
                IDictionary<string, object> root_payload = (IDictionary<string, object>)responseObj;
                foreach (string nodeKey in root_payload.Keys)
                {
                    if (nodeKey.Length > 1)
                    {
                        object nodeObj = root_payload[nodeKey];
                        if (nodeObj is String)
                        {
                            string value = nodeObj as String;

                            switch (nodeKey)
                            {
                                case "id":
                                    resultValue.SocialID = value;
                                    break;
                                case "name":
                                    resultValue.Nick = value;
                                    break;
                                case "email":
                                    resultValue.Email = value;
                                    break;
                            }

                        }
                    }
                }

            }
            catch(System.Exception ex)
            {
                resultValue = null;
            }                        
            return resultValue;
        }
    }
}
