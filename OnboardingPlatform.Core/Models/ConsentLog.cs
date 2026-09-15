using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnboardingPlatform.Core.Enums;


namespace OnboardingPlatform.Core.Models
{
    public class ConsentLog
    {
        [Key]
        public Guid ConsentId { get; set; } = Guid.NewGuid();

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string ConsentType { get; set; } = string.Empty;

        public DateTime GrantedAt { get; set; }

        public Channel Channel { get; set; }
    }
}
