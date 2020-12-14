using System;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Blickkontakt.Office.Model
{

    [Table("customer")]
    public class Customer
    {

        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("modified")]
        public DateTime Modified { get; set; }

    }

}

#nullable enable