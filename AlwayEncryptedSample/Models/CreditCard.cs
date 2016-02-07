using System;
using System.ComponentModel.DataAnnotations;

namespace AlwayEncryptedSample.Models
{
    public class CreditCard
    {
        public CreditCard()
        {
            ModifiedDate = DateTime.Now;
        }

        [Key]
        public int CreditCardId { get; set; }
        [Required]
        [MaxLength(50)]
        public string CardType { get; set; }
        [Required]
        [MaxLength(25)]
        public string CardNumber { get; set; }
        [Required]
        public byte ExpMonth { get; set; }
        [Required]
        public short ExpYear { get; set; }
        public short CCV { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}