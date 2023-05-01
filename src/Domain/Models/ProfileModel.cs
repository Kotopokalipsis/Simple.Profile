using System;

namespace Domain.Models
{
    public record ProfileModel
    {
        public Guid UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Country { get; set; }
    }
}