using Newtonsoft.Json;
using StarWars.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace StarWars
{
    public class Core
    {
        // İsteklerin HTTP metodunu temsil eden enum.
        private enum HttpMethod
        {
            GET,
            POST
        }

        private static readonly string apiUrl = "https://swapi.dev/api/";

        private readonly string _proxyName;

        public Core(string proxyName)
        {
            _proxyName = proxyName;
        }

        public Core()
        {
        }

        // Verilen bir endpoint'e GET HTTP isteği yapar ve sonucu döndürür.
        private static string Request(string endpoint, HttpMethod httpMethod)
        {
            return Request(endpoint, httpMethod, null);
        }

        // Verilen bir endpoint'e GET veya POST HTTP isteği yapar ve sonucu döndürür.
        private static string Request(string endpoint, HttpMethod httpMethod, string data)
        {
            string result = string.Empty;
            string url = apiUrl + endpoint;

            // HTTP isteği için HttpWebRequest nesnesi oluşturulur ve isteğin yapılacağı URL belirtilir.
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = httpMethod.ToString();

            // Eğer HTTP metodu POST ise ve data parametresi boş değilse, isteğin gövdesine veri eklenir.
            if (httpMethod == HttpMethod.POST && !string.IsNullOrEmpty(data))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                httpWebRequest.ContentLength = bytes.Length;
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }

            // HTTP isteği gönderilir ve yanıt alınır.
            try
            {
                // HTTP isteği gönderilir ve yanıt alınır.
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                // Hata durumunda özel bir hata mesajı döndürebilir veya hatayı loglayabilirsiniz.
                // Örneğin:
                Console.WriteLine("HTTP isteğinde bir hata oluştu: " + ex.Message);
                // veya
                // throw new Exception("HTTP isteğinde bir hata oluştu", ex);
            }

            return result;
        }

        // Verilen bir Dictionary'nin anahtar-değer çiftlerini birleştirerek bir parametre dizisi olarak döndürür.
        private static string SerializeDictionary(Dictionary<string, string> dictionary)
        {
            StringBuilder parameters = new StringBuilder();

            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                // Her bir key-value çifti, parametre olarak gönderilen dictionary'den alınır.
                // Ve bu çiftler, "key=value&" formatında birleştirilir ve parameters değişkenine eklenir.
                parameters.Append(keyValuePair.Key + "=" + keyValuePair.Value + "&");
            }

            // Oluşturulan parametre dizisinin sonundaki "&" karakteri silinir.
            return parameters.ToString().TrimEnd('&');
        }

        // Belirtilen endpoint'ten tek bir öğe alır ve döndürür.
        public static T GetSingle<T>(string endpoint, Dictionary<string, string>? parameters = null) where T : Entity
        {
            string serializedParameters = "";
            if (parameters != null)
            {
                // Parametreler varsa, SerializeDictionary metodu kullanılarak parametrelerin URL formatına dönüştürülür.
                serializedParameters = "?" + SerializeDictionary(parameters);
            }
            return GetSingleByUrl<T>(endpoint + serializedParameters);
        }

        // Belirtilen bir endpoint'ten birden çok öğe alır.
        public EntityResults<T> GetMultiple<T>(string endpoint) where T : Entity
        {
            return GetMultiple<T>(endpoint, null);
        }

        // Belirtilen bir endpoint'ten birden çok öğe alır ve parametreleri kullanır.
        public EntityResults<T> GetMultiple<T>(string endpoint, Dictionary<string, string> parameters) where T : Entity
        {
            string serializedParameters = "";
            if (parameters != null)
            {
                // Parametreler varsa, SerializeDictionary metodu kullanılarak parametrelerin URL formatına dönüştürülür.
                serializedParameters = "?" + SerializeDictionary(parameters);
            }

            // Oluşturulan URL ile HTTP GET isteği yapılır ve gelen JSON yanıtı alınır.
            string json = Request(endpoint + serializedParameters, HttpMethod.GET);
            EntityResults<T> swapiResponse = JsonConvert.DeserializeObject<EntityResults<T>>(json);
            return swapiResponse;
        }

        // Verilen URL'deki sorgu parametrelerini alır ve anahtar-değer çiftleri olarak döndürür.
        public NameValueCollection GetQueryParameters(string dataWithQuery)
        {
            // URL sorgu parametrelerini ayıklamak için kullanılır.
            NameValueCollection result = new NameValueCollection();

            // URL, sorgu parametreleri ve diğer kısımlarına ayrılır.
            string[] parts = dataWithQuery.Split('?');

            // Sorgu parametreleri mevcutsa işleme devam edilir.
            if (parts.Length > 1)
            {
                string QueryParameter = parts[1];
                if (!string.IsNullOrEmpty(QueryParameter))
                {
                    // Sorgu parametreleri '&' işaretine göre ayrılır.
                    string[] p = QueryParameter.Split('&');

                    // Her bir sorgu parametresi, anahtar-değer çiftleri halinde result nesnesine eklenir.
                    foreach (string s in p)
                    {
                        if (s.Contains('='))
                        {
                            // Anahtar ve değer arasındaki '=' işaretine göre ayrılır.
                            string[] temp = s.Split('=');
                            result.Add(temp[0], temp[1]);
                        }
                        else
                        {
                            // Eğer '=' işareti yoksa, parametre sadece anahtar olarak kabul edilir ve değeri boş olarak atanır.
                            result.Add(s, string.Empty);
                        }
                    }
                }
            }
            return result;
        }

        // Belirtilen bir varlık adı ve sayfa numarasına göre, tüm sayfalama öğelerini alır.
        public EntityResults<T> GetAllPaginated<T>(string entityName, string pageNumber = "1") where T : Entity
        {
            // Sayfalama için kullanılan parametreleri bir dictionary'de toplar
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("page", pageNumber);

            // GetMultiple metodu çağrılarak veri alınır
            EntityResults<T> result = GetMultiple<T>(entityName, parameters);

            // Sayfalama bilgileri güncellenir
            result.next_page_no = String.IsNullOrEmpty(result.next) ? null : GetQueryParameters(result.next)["page"];
            result.previous_page_no = String.IsNullOrEmpty(result.previous) ? null : GetQueryParameters(result.previous)["page"];

            return result;
        }

        // Belirtilen URL'den tek bir öğe alır.
        public static T GetSingleByUrl<T>(string url) where T : Entity
        {
            // Verilen URL üzerinden HTTP GET isteği yapılır ve JSON yanıtı alınır
            string json = Request( url, HttpMethod.GET);

            // JSON yanıtı belirtilen türdeki bir öğeye dönüştürülür
            T swapiResponse = JsonConvert.DeserializeObject<T>(json);

            return swapiResponse;
        }
    }
}
