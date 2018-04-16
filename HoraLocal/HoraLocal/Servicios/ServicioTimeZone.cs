using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using HoraLocal.Helpers;

namespace HoraLocal.Servicios
{
    public static class ServicioTimeZone
    {
        static DateTime fecha1970 = new DateTime(1970, 1, 1);
        static DateTime fecha1970Utc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        static HttpClient Cliente = new HttpClient();

        static CancellationTokenSource source = new CancellationTokenSource();
        public static CancellationToken token = source.Token;

        public static async Task<DateTime?> ObtenerHoraLocal(double latitud, double longitud, CancellationToken cancellationToken)
        {
            try
            {
                var timestamp = (Int32)(DateTime.UtcNow.Subtract(fecha1970)).TotalSeconds;
                var timeZoneApiUrl = $"https://maps.googleapis.com/maps/api/timezone/json?location={latitud},{longitud}&timestamp={timestamp}&key={Constantes.TimeZoneApiKey}";

                using (var request = new HttpRequestMessage(HttpMethod.Get, timeZoneApiUrl))
                {
                    using (var response = await Cliente.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                    {
                        var stream = await response.Content.ReadAsStreamAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var timeZoneInfo = DeserializeJsonFromStream<Modelos.TimeZoneInfo>(stream);

                            var timestampActual = timestamp + timeZoneInfo.dstOffset + timeZoneInfo.rawOffset;
                            var fechaLocal = fecha1970Utc.AddSeconds(timestampActual);
                            return fechaLocal;
                        }

                        var content = await StreamToStringAsync(stream);
                        throw new ApiException
                        {
                            StatusCode = (int)response.StatusCode,
                            Content = content
                        };
                    }
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }

        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }

        public class ApiException : Exception
        {
            public int StatusCode { get; set; }

            public string Content { get; set; }
        }
    }
}
