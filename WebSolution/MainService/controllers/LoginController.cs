using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin.Hosting;
using Nancy;
using MainService.test;

using CommonModule;
using CommonModule.data;

using System.Configuration;


#pragma warning disable 1998

namespace MainService.controllers
{

    public class LoginController : BaseController
    {
        protected DBLoginHandler dbhandler;

        public LoginController()
        {
            parentSpace = "/api/login";
            dbhandler = new DBLoginHandler(ConfigurationManager.AppSettings.Get("ConectionDBString"));

            Get["",true] = async (parameters, ct) => {
                //Default
                PageInfo pageInfo = new PageInfo();
                pageInfo.langcode = "ko_KR";
                pageInfo.langfile = string.Format("/js/lang/ko_KR.js");
                return View["public/index.html", pageInfo];
            };

            Get["/{langID}", true] = async (parameters, ct) => {
                PageInfo pageInfo = new PageInfo();
                pageInfo.langcode = parameters.langID;
                pageInfo.langfile = string.Format("/js/lang/{0}.js", parameters.langID);
                return View["public/index.html", pageInfo];
            };


            Post[parentSpace + "/createUser", true] = async (parameters, ct) =>
            {                
                IDictionary<string, object> payload = GetPayLoad();
                int socialType = (int)payload["socialType"];
                string socialID = (string)payload["socialID"];
                string nickUtf8 = (string)payload["nick"];

                Encoding enc = new UTF32Encoding(true, true);
                //System.Text.Encoding utf8 = System.Text.Encoding.UTF8;                                
                string nick = System.Net.WebUtility.UrlDecode(nickUtf8);
                //string picurl = (string)payload["picurl"];

                Random r = new Random();
                int imgNumber = r.Next(1, 72);
                string picurl = String.Format("/img/avatar/default/avartar_{0:00}.png", imgNumber);
                
                string email = (string)payload["email"];
                string pw = (string)payload["pw"];

                pw = Utils.DecryptStringAES(pw);
                pw = Utils.GetMD5Hash(pw);

                bool isValidEmail = true;
                if (socialType == 0)
                {
                    isValidEmail = isValidEmail && Utils.IsValidEmail(socialID);
                };
                isValidEmail = isValidEmail && Utils.IsValidEmail(email);
                if (!isValidEmail)
                {
                    return new WebException("ApiError_InvalidEmail", (int)ErrorCode.ApiError_InvalidEmail);
                }

                int createResult = -1;
                Task t = Task.Run(() => {
                    createResult = dbhandler.sp_createuser(socialType, socialID, nick, picurl, email, pw);
                });
                await t;
                if (createResult != 0)
                {
                    return new WebException("Create failed", createResult);
                }

                return new WebOK();
            };

            Post[parentSpace + "/deleteUser", true] = async (parameters, ctor) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                string authToken = (string)payload["authtoken"];
                DBLoginInfo loginInfo = null;

                Task taskCheckLogin = Task.Run(() => {
                    loginInfo = dbhandler.sp_validtoken(authToken, 1);
                });
                await taskCheckLogin;

                if(loginInfo == null)
                    return new WebException("Login failed", 401);

                string firstID = loginInfo.SocialID.Split('@')[0].Substring(0, 5);
                if(firstID=="guest")
                    return new WebException("Cant't delete guest", 401);                

                int deleteResult = -1;
                Task deleteUser = Task.Run(() =>
                {
                    deleteResult = dbhandler.sp_deleteuser(loginInfo.SocialType, loginInfo.SocialID);
                });
                await deleteUser;

                if (deleteResult !=0 )
                    return new WebException("Deleted failed", 500);

                return new WebOK();
            };

            Post[parentSpace + "/loginUser", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int socialType = (int)payload["socialType"];
                string socialID = (string)payload["socialID"];
                string pw = (string)payload["pw"];
                if (socialType != 0)
                {
                    return new WebException("ApiError_NotAlow SocialLogin", 401);
                }
                pw = Utils.DecryptStringAES(pw);
                pw = Utils.GetMD5Hash(pw);

                bool isValidEmail = true;
                if (socialType == 0)
                {
                    isValidEmail = isValidEmail && Utils.IsValidEmail(socialID);
                };                
                if (!isValidEmail)
                {
                    return new WebException("ApiError_InvalidEmail", (int)ErrorCode.ApiError_InvalidEmail);
                }

                LoginOK result = null;
                Task t = Task.Run(() => {
                    result = dbhandler.sp_loginuser(socialType, socialID, pw);
                });
                await t;                
                return result;
            };

            Post[parentSpace + "/loginUserSocial", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int socialType = (int)payload["socialType"];                
                string authToken = (string)payload["authToken"];
                if (socialType == 0)
                {
                    return new WebException("ApiError_NotAlow NormalLogin", 401);
                }
                LoginOK resultValue = new LoginOK();
                resultValue.statusCode = -401;

                //FB Mode
                if (socialType == 1)
                {
                    DBLoginInfo fbInfo = null;
                    Task t = Task.Run(() => {
                        fbInfo = FBAPI.GetUserData(authToken);
                    });
                    await t;
                    Random r = new Random();
                    int imgNumber = r.Next(1, 72);
                    string picurl = String.Format("/img/avatar/default/avartar_{0:00}.png", imgNumber);
                    if (fbInfo!=null)
                    {
                        resultValue = dbhandler.sp_loginSocial(socialType, fbInfo.SocialID, fbInfo.Nick, picurl, fbInfo.Email);
                    }
                }                
                return resultValue;
            };

            Post[parentSpace + "/validate", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                string authToken = (string)payload["authtoken"];
                DBLoginInfo loginInfo = null;

                Task t = Task.Run(() => {
                    loginInfo = dbhandler.sp_validtoken(authToken,1);
                });
                await t;

                if (loginInfo == null)
                {
                    return new WebException("Login failed", 401);
                }                                
                return loginInfo.ToString();
            };

            Post["/test2/{id}/add/{category}"] = parameters => {
                return "post test";
            };

        }
    }
}
