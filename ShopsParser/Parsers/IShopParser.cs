using AngleSharp.Dom;
using ShopsDbEntities;
using System.Linq;

namespace ShopsParser
{
	public interface IShopParser
	{
		public ParallelQuery<Product> GetProducts(IDocument document);
	}
}