using Newtonsoft.Json;
using ProductsEntities;
using System.IO;
using Xunit;

namespace ProductsWebApi_Test
{
	public class Deserialization
	{
		[Fact]
		public void TestProductDeserialization()
		{
			var text = File.ReadAllText("Files/Product.json");
			var product = JsonConvert.DeserializeObject<Product>(text);

			Assert.Equal(2, product.Id);
		}
	}
}