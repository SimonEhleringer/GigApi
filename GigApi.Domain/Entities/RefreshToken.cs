using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        public Guid Token { get; set; }

        [Required]
        public string JwtId { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Required]
        public DateTime ExpiryTime { get; set; }

        public bool Used { get; set; }

        public bool Invalidated { get; set; }

        [Required]
        public string UserId { get; set; }

        public GigUser User { get; set; }
    }
}
