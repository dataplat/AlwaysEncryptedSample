using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlwaysEncryptedSample.Models
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
        public virtual CreditCardNetwork Network { get; set; }
        [Required]
        [MaxLength(25)]
        [Column(TypeName = "VARCHAR")]
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