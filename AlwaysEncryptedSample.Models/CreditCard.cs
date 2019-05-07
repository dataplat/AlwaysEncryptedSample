using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlwaysEncryptedSample.Models
{
    public class CreditCard
    {
        public CreditCard()
        {
            //TODO: This is probably completely broken an needs tests to prove out.
            //Need to force this field modified to DateTime.Now every send.  
            ModifiedDate = DateTime.UtcNow;
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
        [Required]
        // ReSharper disable once InconsistentNaming
        public short CCV { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}