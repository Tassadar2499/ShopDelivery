using System.ComponentModel.DataAnnotations;

namespace CouriersWebService.Data
{
	public class AuthData
	{
		[Required]
		[Display(Name = "Login")]
		public string Login { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }
	}
}