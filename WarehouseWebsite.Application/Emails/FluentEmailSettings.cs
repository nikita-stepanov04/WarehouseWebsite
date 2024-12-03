using System.ComponentModel.DataAnnotations;

namespace WarehouseWebsite.Application.Emails
{
    public class FluentEmailSettings
    {
        [Required]
        public required string Host { get; set; }

        [Required]
        public required int Port { get; set; }

        [Required]
        public required string DefaultFromEmail { get; set; }
        
        [Required]
        public required bool EnableSsl { get; set; }

        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
