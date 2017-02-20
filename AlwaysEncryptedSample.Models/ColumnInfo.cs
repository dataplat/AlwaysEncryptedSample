using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlwaysEncryptedSample.Models
{
    public sealed class ColumnInfo
    {
        /// <summary>Fully Qualified table name [db].[schema].[table]</summary>
        public string FQTN { get; set; }
        [Key, Column(Order=0)]
        public string Schema { get; set; }
        [Key, Column(Order = 1)]
        public string Table { get; set; }
        [Key, Column(Order = 2)]
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        /// <summary>Length of the column</summary>
        public short Length { get; set; }
        public string Collation { get; set; }
        public bool Nullable { get; set; }
        public string EncryptionType { get; set; }
        public string GeneratedAlways { get; set; }
    }
}