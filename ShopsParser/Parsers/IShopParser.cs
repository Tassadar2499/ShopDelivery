using AngleSharp.Dom;
using ProductsEntities;
using System.Linq;

namespace ShopsParser
{
	public interface IShopParser
	{
		public ParallelQuery<Product> GetProducts(IDocument document);
	}
}