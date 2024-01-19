using NLog;
using System;
using System.Globalization;
using System.IO;
//using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Net;

namespace LilaSharp
{
    /// <summary>
    /// Class for making requests to lichess.
    /// </summary>
    public class LilaRequest
    {
        /// <summary>
        /// Initializes the <see cref="LilaRequest"/> class.
        /// </summary>
        static LilaRequest()
        {
            //Necessary for https transport calls.
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private const string DefaultUA = "LilaSharp";

        private static Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// LilaRequest content types for accept headers
        /// </summary>
        public enum ContentType
        {
            /// <summary>
            /// Any content accepted
            /// </summary>
            Any,

            /// <summary>
            /// Only html data
            /// </summary>
            Html,

            /// <summary>
            /// Only json responses
            /// </summary>
            Json
        }

        private Uri uri;

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>
        /// The encoding.
        /// </value>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the cookies.
        /// </summary>
        /// <value>
        /// The cookies.
        /// </value>
        public CookieCollection Cookies { get; set; }

        public System.Net.CookieCollection ConvertCookies(CookieCollection wsCookies)
        {
            var netCookies = new System.Net.CookieCollection();

            foreach (Cookie wsCookie in wsCookies)
            {
                var netCookie = new System.Net.Cookie(wsCookie.Name, wsCookie.Value)
                {
                    Domain = wsCookie.Domain,
                    Path = wsCookie.Path,
                    Secure = wsCookie.Secure,
                    HttpOnly = wsCookie.HttpOnly,
                    Expires = wsCookie.Expires,
                    Comment = wsCookie.Comment,
                    CommentUri = wsCookie.CommentUri != null ? new Uri(wsCookie.CommentUri.ToString()) : null,
                    Discard = wsCookie.Discard,
                    Port = wsCookie.Port,
                    Version = wsCookie.Version
                };

                // if (!string.IsNullOrEmpty(wsCookie.MaxAge) && double.TryParse(wsCookie.MaxAge, out double maxAge))
                // {
                //     netCookie.Expires = DateTime.Now.AddSeconds(maxAge);
                // }

                netCookies.Add(netCookie);
            }

            return netCookies;
        }

        /// <summary>
        /// Gets the specified content type.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        public async Task<LilaResponse> Get(ContentType contentType)
        {
            Uri host = new Uri("https://lichess.org");
            Uri absolute = uri.IsAbsoluteUri ? uri : new Uri(host, uri);

            System.Net.HttpWebRequest wreq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(absolute);

            wreq.Method = "GET";
            switch (contentType)
            {
                case ContentType.Any:
                    wreq.Accept = "*/*";
                    break;
                case ContentType.Html:
                    wreq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    break;
                case ContentType.Json:
                    wreq.Accept = "application/vnd.lichess.v2+json";
                    break;
            }

            wreq.AllowAutoRedirect = true;

            wreq.Headers["Accept-Encoding"] = "gzip, deflate";
            wreq.Headers["Accept-Language"] = string.Format("{0}", Culture.IetfLanguageTag);
            wreq.Headers["X-Requested-With"] = "XMLHttpRequest";

            wreq.Host = absolute.Host;
            wreq.UserAgent = UserAgent ?? DefaultUA;

            wreq.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;
            wreq.CookieContainer = new System.Net.CookieContainer();

            if (Cookies != null)
            {
                wreq.CookieContainer.Add(ConvertCookies(Cookies));
            }

            try
            {
                System.Net.HttpWebResponse result = await wreq.GetResponseAsync() as System.Net.HttpWebResponse;
                return new LilaResponse(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to execute GET request to \"{0}\"", ex);
                return new LilaResponse();
            }
        }

        /// <summary>
        /// Posts the specified content type.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Keys and values must be the same length.</exception>
        public async Task<LilaResponse> Post(ContentType contentType, string[] keys, object[] values)
        {
            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Keys and values must be the same length.");
            }

            StringBuilder bldr = new StringBuilder();
            for (int i = 0; i < keys.Length; i++)
            {
                if (i != 0)
                {
                    bldr.Append('&');
                }

                bldr.Append(keys[i]);
                bldr.Append('=');
                if (values != null)
                {
                    bldr.Append(values[i]);
                }
            }

            return await Post(contentType, bldr.ToString());
        }

        /// <summary>
        /// Posts the specified content type.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public async Task<LilaResponse> Post(ContentType contentType, string data)
        {
            return await Post(contentType, Encoding.GetBytes(data));
        }

        /// <summary>
        /// Posts the specified content type.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public async Task<LilaResponse> Post(ContentType contentType, byte[] data)
        {
            Uri host = new Uri("https://lichess.org");
            Uri absolute = uri.IsAbsoluteUri ? uri : new Uri(host, uri);

            System.Net.HttpWebRequest wreq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(absolute);

            wreq.Method = "POST";
            switch (contentType)
            {
                case ContentType.Any:
                    wreq.Accept = "*/*";
                    break;
                case ContentType.Html:
                    wreq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    break;
                case ContentType.Json:
                    wreq.Accept = "application/vnd.lichess.v2+json";
                    break;
            }

            wreq.AllowAutoRedirect = false;

            wreq.Headers["Accept-Language"] = string.Format("{0}", Culture.IetfLanguageTag);
            wreq.Headers["Origin"] = string.Format("{0}{1}", absolute.Scheme, absolute.Host);
            wreq.Headers["X-Requested-With"] = "XMLHttpRequest";

            wreq.Host = absolute.Host;
            wreq.UserAgent = UserAgent ?? DefaultUA;

            wreq.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;
            wreq.CookieContainer = new System.Net.CookieContainer();

            if (Cookies != null)
            {
                wreq.CookieContainer.Add(ConvertCookies(Cookies));
            }

            if (data.Length > 0)
            {
                switch (contentType)
                {
                    case ContentType.Json:
                        wreq.ContentType = "application/json; charset=UTF-8";
                        break;
                    default:
                        wreq.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                        break;
                }

            }

            wreq.ContentLength = data.Length;

            try
            {
                if (data.Length > 0)
                {
                    using (Stream stream = await wreq.GetRequestStreamAsync())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                return new LilaResponse(await wreq.GetResponseAsync() as System.Net.HttpWebResponse);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex, "Failed to execute POST request.");
                return new LilaResponse();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaRequest"/> class.
        /// </summary>
        /// <param name="uri">The relative or absolute URI.</param>
        public LilaRequest(Uri uri)
        {
            this.uri = uri;

            Encoding = Encoding.UTF8;
            Culture = CultureInfo.CurrentCulture;
            Cookies = new CookieCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaRequest"/> class.
        /// </summary>
        /// <param name="uri">The relative or absolute URI.</param>
        /// <param name="encoding">The encoding.</param>
        public LilaRequest(Uri uri, Encoding encoding)
        {
            this.uri = uri;

            Encoding = encoding;
            Culture = CultureInfo.CurrentCulture;
            Cookies = new CookieCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaRequest"/> class.
        /// </summary>
        /// <param name="uri">The relative or absolute URI.</param>
        /// <param name="culture">The culture.</param>
        public LilaRequest(Uri uri, CultureInfo culture)
        {
            this.uri = uri;

            Encoding = Encoding.UTF8;
            Culture = culture;
            Cookies = new CookieCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaRequest"/> class.
        /// </summary>
        /// <param name="uri">The relative or absolute URI.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="culture">The culture.</param>
        public LilaRequest(Uri uri, Encoding encoding, CultureInfo culture)
        {
            this.uri = uri;

            Encoding = encoding;
            Culture = culture;
            Cookies = new CookieCollection();
        }
    }
}
