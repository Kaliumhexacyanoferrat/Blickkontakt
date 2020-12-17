using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Blickkontakt.Office.Model
{

    [Table("announce")]
    public class Announce
    {

        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("number")]
        public int Number { get; set; }

        [Column("customer")]
        public int CustomerId { get; set; }

        [Column("notes")]
        public string Notes { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("modified")]
        public DateTime Modified { get; set; }

        public virtual Customer Customer { get; set; }

    }

}

#nullable enable