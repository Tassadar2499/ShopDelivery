using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsParser
{
	public record Product
	{
		public string Name { get; init; }
		public float Price { get; init; }
		public float? Mass { get; init; }
		public string ImgContent { get; init; }

		public Product(string name, float price, float? mass, string imgContent)
			=> (Name, Price, Mass, ImgContent) = (name, price, mass, imgContent);
	}
}
