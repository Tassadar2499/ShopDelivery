using HarabaSourceGenerators.Common.Attributes;
using ShopsDbEntities;
using System.Collections.Generic;
using System.Linq;

namespace ShopsDbLogic.Logic
{
	[Inject]
	public partial class ProductsLogicBase
	{
		private readonly MainDbContext _context;
		public MainDbContext Context => _context;
		public IQueryable<Product> Products => Context.Products;

		public IQueryable<Product> GetProductsByIdSet(HashSet<long> idSet)
			=> Products.Where(p => idSet.Contains(p.Id));
	}
}