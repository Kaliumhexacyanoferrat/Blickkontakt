using System;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Blickkontakt.Office.Model
{

    [Table("account")]
    public class Account
    {

        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("is_admin")]
        public bool Admin { get; set; }

        [Column("is_active")]
        public bool Active { get; set; }

        [Column("display_name")]
        public string DisplayName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("modified")]
        public DateTime Modified { get; set; }

    }

}

#nullable enable