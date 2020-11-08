using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsParser
{
	public interface IShopParser
	{
		public ParallelQuery<Product> GetProducts(IDocument document);
	}
}
