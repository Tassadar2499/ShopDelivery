using System.ComponentModel.DataAnnotations;

namespace ShopDeliveryApplication.Models.Entities.Authorize
{
	public class ForgotPassword
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}