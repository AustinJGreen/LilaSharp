using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LilaSharp
{
    /// <summary>
    /// Class for handling responses from lichess.
    /// </summary>
    public class LilaResponse
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private HttpWebResponse wres;

        /// <summary>
        /// Checks the status.
        /// </summary>
        /// <param name="code">The expected code(s).</param>
        /// <returns>True if the status matches the expected code(s); otherwise false.</returns>
        public bool CheckStatus(HttpStatusCode code)
        {
            return wres != null && (wres.StatusCode & code) == wres.StatusCode;
        }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        /// <returns></returns>
        public CookieCollection GetCookies()
        {
            if (wres != null)
            {
                return wres.Cookies;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        public string GetHeader(string header)
        {
            if (wres != null)
            {
                return wres.GetResponseHeader(header);
            }

            return null;
        }

        /// <summary>
        /// Reads the response as a string synchronously. 
        /// </summary>
        /// <returns>The response read as a string.</returns>
        public string Read()
        {
            if (wres != null)
            {
                try
                {
                    using (StreamReader rdr = new StreamReader(wres.GetResponseStream()))
                    {
                        string raw = rdr.ReadToEnd();
                        return raw;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex, "Failed to read response from server.");
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads the response as a string asynchronously. 
        /// </summary>
        /// <returns>The response read as a string.</returns>
        public async Task<string> ReadAsync()
        {
            if (wres != null)
            {
                try
                {
                    using (StreamReader rdr = new StreamReader(wres.GetResponseStream()))
                    {
                        string raw = await rdr.ReadToEndAsync();
                        return raw;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex, "Failed to read response from server.");
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads the response as json.
        /// </summary>
        /// <typeparam name="T">The json type to parse</typeparam>
        public T ReadJson<T>()
        {
            if (wres != null)
            {
                try
                {
                    StreamReader rdr = new StreamReader(wres.GetResponseStream());
                    using (JsonReader jrdr = new JsonTextReader(rdr))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Error += OnJsonError;

                        return serializer.Deserialize<T>(jrdr);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex, "Failed to read response from server.");
                    return default(T);
                }
            }

            return default(T);
        }

        /// <summary>
        /// Called when a json parse error is detected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Newtonsoft.Json.Serialization.ErrorEventArgs"/> instance containing the event data.</param>
        private void OnJsonError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.ErrorContext.Error, "Failed to deserialize json.");
        }

        /// <summary>
        /// Initializes an empty instance of the <see cref="LilaResponse"/> class.
        /// </summary>
        public LilaResponse()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LilaResponse"/> class.
        /// </summary>
        /// <param name="wres">The http web response.</param>
        public LilaResponse(HttpWebResponse wres)
        {
            this.wres = wres;
        }
    }
}
