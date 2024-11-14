using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config;

public class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder){
        builder.ToTable("Students");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.StudentName).HasMaxLength(250).IsRequired();
        builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
        builder.Property(n => n.Email).IsRequired().HasMaxLength(250);
        builder.HasIndex(n => n.Email).IsUnique();

        builder.HasData(new List<Student>(){
            new Student {
                Id = 1,
                StudentName = "John Doe",
                Email = "johndoe@example.com",
                Address = "123 Main St",
                DOB = new DateTime(2002, 12, 12)
            },
            new Student {
                Id = 2,
                StudentName = "Jane Doe",
                Email = "janedoe@example.com",
                Address = "245 Oxford St",
                DOB = new DateTime(2003, 11, 06)
            },
        });
    }
}
