using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace StudentTask
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<StudentTBL> StudentTBLs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentTBL>()
                .Property(e => e.student_id)
                .IsUnicode(false);

            modelBuilder.Entity<StudentTBL>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<StudentTBL>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<StudentTBL>()
                .Property(e => e.phone_number)
                .IsUnicode(false);

            modelBuilder.Entity<StudentTBL>()
                .Property(e => e.course)
                .IsUnicode(false);
        }
    }
}
