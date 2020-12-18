using System;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Blickkontakt.Office.Model
{

    #region Data structures

    public enum AnnounceStatus : short
    {

        /// <summary>
        /// The announce has been created but requires manual work
        /// before it can published in a letter.
        /// </summary>
        New = 0,

        /// <summary>
        /// The announce is prepared to be published with the next
        /// letter.
        /// </summary>
        Prepared = 1,

        /// <summary>
        /// The announce has been published with the most recent letter.
        /// </summary>
        Published = 2,

        /// <summary>
        /// The announce is no longer visible because there are already
        /// new ones, but contacts might still be mediated.
        /// </summary>
        Expired = 3,

        /// <summary>
        /// The customer no longer wants to be mediated so we are
        /// not allowed to mediate his or her contact information.
        /// </summary>
        Withdrawn = 4

    }

    #endregion

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

        [Column("status")]
        public AnnounceStatus Status { get; set; }

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