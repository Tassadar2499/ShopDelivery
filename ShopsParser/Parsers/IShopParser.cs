using AngleSharp.Dom;
using ShopsDbEntities.Entities.ProductEntities;
using System.Linq;

namespace ShopsParser
{
	public interface IShopParser
	{
		public ParallelQuery<ParsedProduct> GetProducts(IDocument document);
	}
}