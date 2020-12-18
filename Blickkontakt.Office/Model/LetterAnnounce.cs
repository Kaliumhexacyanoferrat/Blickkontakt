using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Blickkontakt.Office.Model
{

    [Table("letter_announce")]
    public class LetterAnnounce
    {

        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("letter")]
        public int LetterId { get; set; }

        [Column("announce")]
        public int AnnounceId { get; set; }

        [Column("order")]
        public int Order { get; set; }

        public virtual Announce Announce { get; set; }

        public virtual Letter Letter { get; set; }

    }

}

#nullable enable