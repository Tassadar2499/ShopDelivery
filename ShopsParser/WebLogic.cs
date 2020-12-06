using ProductsEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ShopsParser
{
	public class WebLogic : IDisposable
	{
		private readonly HttpClient _httpClient;
		private readonly string _url;

		public WebLogic(string url)
		{
			_httpClient = new HttpClient();
			_url = $"{url}/product/create";
		}

		public void CreateProductRemote(Product product)
		{ 
			var message = _httpClient.PostAsJsonAsync(_url, product).Result;

			if (message.StatusCode != HttpStatusCode.OK)
				throw new WebException("Error in send request");
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			_httpClient?.Dispose();
		}
	}
}
