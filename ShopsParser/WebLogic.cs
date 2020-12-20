using ShopsDbEntities;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace ShopsParser
{
	public class WebLogic : IDisposable
	{
		private readonly HttpClient _httpClient;
		private readonly string _url;

		public WebLogic(string url)
		{
			_httpClient = new HttpClient();
			_url = url;
		}

		public void CreateOrUpdateProducts(ProductData productData)
		{
			var url = $"{_url}/product/createorupdate";
			var responseMessage = _httpClient.PostAsJsonAsync(url, productData).Result;

			if (responseMessage.StatusCode != HttpStatusCode.OK)
			{
				var result = responseMessage.Content.ReadAsStringAsync().Result;
				throw new WebException($"Error in send request {result}");
			}
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_httpClient?.Dispose();
		}
	}
}