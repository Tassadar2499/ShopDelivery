using Newtonsoft.Json;
using ProductsEntities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsParser
{
	internal class Program
	{
		private const string SETTING_PATH = "Settings/Setting.json";

		private static void Main(string[] args)
		{
			var text = File.ReadAllText(SETTING_PATH);
			var setting = JsonConvert.DeserializeObject<Setting>(text);
			var executor = new MainParser();

			var products = setting.Shops
				.AsParallel()
				.SelectMany(executor.GetProductsByShop)
				.ToArray();

			var productData = new ProductData() { Products = products };

			using var webLogic = new WebLogic(setting.WebApiUrl);
			webLogic.CreateOrUpdateProducts(productData);
		}
	}
}