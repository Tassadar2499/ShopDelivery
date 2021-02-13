using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsDbEntities.Entities.UsersInfo
{
	public class UsersAddress
	{
		public long Id { get; set; }
		public long UserId { get; set; }
		public long AddressId { get; set; }
	}
}
