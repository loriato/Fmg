using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace Europa.Commons
{
    public static class HttpUtil
    {
        private static string XForwardedFor = "X-Forwarded-For";
        private static string RemoteAddr = "REMOTE_ADDR";

        public static string RequestIp(HttpRequestBase request)
        {
            List<string> ips = new List<string>();

            foreach (var key in request.Headers.AllKeys)
            {
                if (XForwardedFor == key)
                {
                    ips.Add(request.Headers[key]);
                }
            }
            // Fail Safe (With no Header)
            if (ips.Count == 0)
            {
                ips.Add(GetUserIP(request));
            }
            return String.Join(", ", ips);
        }

        private static string GetUserIP(HttpRequestBase request)
        {
            string ip = request.ServerVariables[XForwardedFor];
            if (String.IsNullOrEmpty(ip) || ip.Equals("unknown", StringComparison.OrdinalIgnoreCase))
            {
                ip = request.ServerVariables[RemoteAddr];
            }
            if (ip == "::1")
                ip = "127.0.0.1";
            return ip;
        }

        public static string RequestIpWithLinq(HttpRequestBase request)
        {
            List<string> ipsHeaders = request.Headers.AllKeys.Where(reg => reg.ToLower() == XForwardedFor).ToList();
            List<string> ips = new List<string>();
            foreach (var key in ipsHeaders)
            {
                ips.Add(request.Headers[key]);
            }
            return String.Join(", ", ips);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("not-fount");
        }

        public static string GetClientIp(HttpRequestBase request)
        {
            string ip = request.ServerVariables[XForwardedFor];
            if (String.IsNullOrEmpty(ip) || ip.Equals("unknown", StringComparison.OrdinalIgnoreCase))
            {
                ip = request.ServerVariables[RemoteAddr];
            }
            if (ip == "::1")
                ip = "127.0.0.1";
            return ip;
        }

        public static string GetClientIp(HttpContext context)
        {
            string ip = context.Request.ServerVariables[XForwardedFor];
            if (String.IsNullOrEmpty(ip) || ip.Equals("unknown", StringComparison.OrdinalIgnoreCase))
            {
                ip = context.Request.ServerVariables[RemoteAddr];
            }
            if (ip == "::1")
                ip = "127.0.0.1";
            return ip;
        }


    }
}
