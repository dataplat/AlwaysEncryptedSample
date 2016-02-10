using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace AlwaysEncryptedSample.Models
{
    [DebuggerDisplay("Id: {Id} Name: {Name}")]
    public sealed class CreditCardNetwork
    {
        [Key]
        public CreditCardNetworks Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(16)]
        public string Name { get; set; }

        private static ReadOnlyCollection<CreditCardNetwork> _networks;

        static CreditCardNetwork()
        {
            _networks = new List<CreditCardNetwork>
            {
                new CreditCardNetwork{ Id = CreditCardNetworks.Amex, Name = "American Express"},
                new CreditCardNetwork{ Id = CreditCardNetworks.Visa, Name = "Visa"},
                new CreditCardNetwork{ Id = CreditCardNetworks.MasterCard, Name = "Master Card"},
                new CreditCardNetwork{ Id = CreditCardNetworks.Discover, Name = "Discover"},
            }.AsReadOnly();
        }

        public static IList<CreditCardNetwork> GetNetworks()
        {
            return _networks;
        }
    }
}