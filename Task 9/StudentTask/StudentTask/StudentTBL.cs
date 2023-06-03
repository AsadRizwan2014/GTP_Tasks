namespace StudentTask
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentTBL")]
    public partial class StudentTBL
    {
        [Key]
        [StringLength(50)]
        public string student_id { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(20)]
        public string phone_number { get; set; }

        [StringLength(500)]
        public string course { get; set; }
    }
}
