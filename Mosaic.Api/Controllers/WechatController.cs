using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mosaic.DTO;
using Mosaic.ServiceContracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mosaic.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Wechat/[action]")]
    public class WechatController : Controller
    {

        private IConfiguration configuration;
        private readonly IUserInfoService userInfoService;

        public WechatController(IConfiguration configuration,IUserInfoService userInfoService)
        {
            this.configuration = configuration;
            this.userInfoService = userInfoService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ResultData(string content)
        {
            ViewBag.Content = content;
            return View();
        }
        public class LoginData
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        [HttpPost]
        public UserInfoDataObject CheckLogin(LoginData loginData)
        {
            //UserInfoDataObject user = this.userInfoService.CheckUser(loginData.UserName, loginData.Password);
            UserInfoDataObject user = this.userInfoService.GetList().First();
            return user;
        }

        [HttpGet]
        public IActionResult Wx([FromForm] string username,[FromForm] string password)
        {

            string url = UriHelper.GetDisplayUrl(Request);
            //string url = "http://dy.sap-unis.com/api/Wechat/GetAccessToken";
            IConfigurationSection weChatConfiguration= this.configuration.GetSection("WechatConfig");
            //AccessToken
            string appid = weChatConfiguration.GetSection("Appid").Value;
            string appSecret = weChatConfiguration.GetSection("AppSecret").Value;
            string accessTokenURL = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appSecret;
            string accessTokenString = this.GetURL(accessTokenURL);
            JObject jo = (JObject)JsonConvert.DeserializeObject(accessTokenString);
            string accessToken = jo["access_token"].ToString();
            //sign
            string SignURL = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + accessToken + "&type=jsapi";
            string sign = this.GetURL(SignURL);
            JObject jsonObject = (JObject)JsonConvert.DeserializeObject(sign);
            string ticket = jsonObject["ticket"].ToString();
            //string urlweixin = "http://dy.sap-unis.com/newest/weixin.html";
            //string urlweixin = "http://dy.sap-unis.com/";
            string noncestr = "Wm3WZYTPz0wzccnW";
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数
                        
         
            string str1 = "jsapi_ticket=" + ticket + "&noncestr=Wm3WZYTPz0wzccnW&timestamp="+timeStamp+ "&url="+url;
            string sha1 = this.GetSHA1(str1);
           DataRequest data = new DataRequest() { AccessToken = accessToken, Sign = sign, Appid = appid, URL = url, TimeStamp = timeStamp
                , Noncestr = noncestr,Str=sha1

            };
            ViewBag.Data = data;
            return View();
        }

        private string  GetSHA1(string  data)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] buf = Encoding.UTF8.GetBytes(data);
            byte[] result = sha1.ComputeHash(buf);
            string sha = string.Join("",result.Select(p=>p.ToString("x2"))); //BitConverter.ToString(result).Replace("-", "");
            return sha;
        }
        private string GetURL(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            return result;
        }
        public IActionResult ChangButton()
        {
            return View();
        }
    }
    public class DataRequest
    {
        public string Appid { get; set; }
        public string AccessToken { get; set; }
        public string Sign { get; set; }
        public long TimeStamp { get; set; }
        public string Noncestr { get; set; }
        public string URL { get; set; }
        public string Str { get; set; }
    }
}