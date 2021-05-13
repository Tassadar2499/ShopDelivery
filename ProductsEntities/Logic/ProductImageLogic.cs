using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ShopsDbEntities.Logic
{
	public class ProductImageLogic
	{
		private const string BROWSER_HEADER = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";
		private const string USER_AGENT = "user-agent";

		private readonly string _connection;
		private readonly string _containerName;
		public ProductImageLogic(IConfiguration configuration)
		{
			_connection = configuration.GetConnectionString("BlobConnection");
			_containerName = configuration.GetSection("ConnectionObjNames")["BlobContainer"];
		}

		public IEnumerable<Product> LoadProductImagesToBlob(IEnumerable<(Product Product, string SiteUrl)> productsUrlCollection)
			=> productsUrlCollection.AsParallel().Select(LoadProductImageToBlob);

		private Product LoadProductImageToBlob((Product Product, string SiteUrl) productInfo)
		{
			var (product, siteUrl) = productInfo;
			var blobServiceClient = new BlobServiceClient(_connection);
			var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

			using var webClient = new WebClient();
			webClient.Headers.Add(USER_AGENT, BROWSER_HEADER);
			using var memoryStream = webClient.OpenRead(siteUrl);

			var imageName = $"{product.Id}.jpg";
			containerClient.UploadBlob(imageName, memoryStream);
			product.ImageUrl = $"{containerClient.Uri.AbsoluteUri}/{imageName}";

			return product;
		}
	}
}
