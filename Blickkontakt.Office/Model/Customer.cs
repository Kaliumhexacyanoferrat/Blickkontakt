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

        [Column("number")]
        public int Number { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("mail")]
        public string Mail { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("street")]
        public string Street { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("zip")]
        public string Zip { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("modified")]
        public DateTime Modified { get; set; }

    }

}

#nullable enable