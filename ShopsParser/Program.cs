using Newtonsoft.Json;
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
			var executor = new Executor();

			var productsQuery = setting.Shops.AsParallel().SelectMany(executor.GetProductsByShop);

			var products = productsQuery.ToArray();
		}
	}
}