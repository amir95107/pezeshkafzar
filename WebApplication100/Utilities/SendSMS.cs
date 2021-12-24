using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace MyEshop
{
    public class SendSMS
    {
        public static string[] SendVerificationCode(string mobile)
        {
            Random rnd = new Random();
            int code = rnd.Next(10000, 100000);
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync("api/send/shared/1d3f0980cff247cc947a34c83afb4a02",
                    new { bodyId = 48024, to = mobile, args = new[] { code.ToString() } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                string[] arr = new string[] { code.ToString(), response };
                return arr;
            }
        }
        public static string[] SendLoginVerificationCode(string mobile, string userName)
        {
            Random rnd = new Random();
            int code = rnd.Next(10000, 100000);
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync("api/send/shared/1d3f0980cff247cc947a34c83afb4a02",
                    new { bodyId = 48025, to = mobile, args = new[] { userName, code.ToString() } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                string[] arr = new string[] { code.ToString(), response };
                return arr;
            }
        }

        public static string SendSuccessOrderMessage(string mobile,string name,string traceCode)
        {
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync("api/send/shared/1d3f0980cff247cc947a34c83afb4a02",
                    new { bodyId = 48174, to = mobile, args = new[] { name, traceCode } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                return response;
            }
        }

        public static string SendDiscountMessage(string mobile, string name, string discountCode,string maxValue,string date)
        {
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync("api/send/shared/1d3f0980cff247cc947a34c83afb4a02",
                    new { bodyId = 48170, to = mobile, args = new[] { name, discountCode,maxValue,date } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                return response;
            }
        }

        public static string[] SendSellerVerificationCode(string mobile)
        {
            Random rnd = new Random();
            int code = rnd.Next(10000, 100000);
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync("api/send/shared/1d3f0980cff247cc947a34c83afb4a02",
                    new { bodyId = 49167, to = mobile, args = new[] { code.ToString() } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                string[] arr = new string[] { code.ToString(), response };
                return arr;
            }
        }

        public static string[] ConfirmSeller(string mobile, string name)
        {
            Uri apiBaseAddress = new Uri("https://console.melipayamak.com");
            using (HttpClient client = new HttpClient() { BaseAddress = apiBaseAddress })
            {
                var result = client.PostAsJsonAsync("api/send/shared/1d3f0980cff247cc947a34c83afb4a02",
                    new { bodyId = 49177, to = mobile, args = new[] { name } }).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                string[] arr = new string[] { response };
                return arr;
            }
        }
    }
}