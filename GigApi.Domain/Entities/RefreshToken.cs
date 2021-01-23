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

        public string JwtId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool Used { get; set; }

        public bool Invalidated { get; set; }

        public string UserId { get; set; }

        public GigUser User { get; set; }
    }
}
