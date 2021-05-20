using ShopsDbEntities.Entities.ProductEntities;

namespace ShopsDbEntities.Logic.Comparers
{
	public static class ProductDataComparer
	{
		public static bool IsParsedProductEqualToProduct(ParsedProduct parsedProduct, Product product)
			=> parsedProduct.Name == product.Name
				&& parsedProduct.SubCategory == product.SubCategory
				&& parsedProduct.Category == product.Category
				&& parsedProduct.ShopType == product.ShopType;
	}
}