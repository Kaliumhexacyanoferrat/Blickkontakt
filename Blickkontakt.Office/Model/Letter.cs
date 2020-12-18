using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Blickkontakt.Office.Model
{

    #region Status

    public enum LetterStatus : ushort
    {

        /// <summary>
        /// The letter can be changed.
        /// </summary>
        New = 0,

        /// <summary>
        /// The letter has been given out and can no longer be changed.
        /// </summary>
        Published = 1

    }

    #endregion

    [Table("letter")]
    public class Letter
    {

        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("status")]
        public LetterStatus Status { get; set; }

        [Column("published")]
        public DateTime? Published { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("modified")]
        public DateTime Modified { get; set; }

        public virtual ICollection<LetterAnnounce> Announces { get; }

    }

}

#nullable enable