using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShopsDbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ProductsWebApi.Models.Logic
{
	public class ProductImageLogic
	{
		private const string BLOB_CONNECTION = "BlobConnection";
		private const string CONNECTION_OBJ_NAMES = "ConnectionObjNames";
		private const string BLOB_CONTAINER = "BlobContainer";
		private const string BROWSER_HEADER = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";
		private const string USER_AGENT = "user-agent";

		private readonly ILogger<ProductImageLogic> _logger;
		private readonly string _connection;
		private readonly string _containerName;

		public ProductImageLogic(ILogger<ProductImageLogic> logger, IConfiguration configuration)
		{
			_logger = logger;
			_connection = configuration.GetConnectionString(BLOB_CONNECTION);
			_containerName = configuration.GetSection(CONNECTION_OBJ_NAMES)[BLOB_CONTAINER];
		}

		public IEnumerable<Product> LoadProductImagesToBlob(IEnumerable<(Product Product, string SiteUrl)> productsUrlCollection)
			=> productsUrlCollection
			.AsParallel()
			.Select(LoadProductImageToBlobSafety)
			.Where(p => p != null);

		private Product LoadProductImageToBlobSafety((Product Product, string SiteUrl) productInfo)
		{
			Product product = null;
			try
			{
				product = LoadProductImageToBlob(productInfo);
			}
			catch (WebException ex)
			{
				_logger.LogError($"Web exception in uploading image to blob - {ex}");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in uploading image to blob - {ex}");
			}

			return product;
		}

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