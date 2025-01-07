using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// Database Context
public class ExamManagementDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<StudentSubject> StudentSubjects { get; set; }
    public DbSet<SeatAllocation> SeatAllocations { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Your_Connection_String_Here");
    }
}

// DTOs
public class StudentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RollNumber { get; set; }
    public string House { get; set; }
    public string ClassName { get; set; }
    public string SeatInfo { get; set; } // Seat details if allocated
}

public class SeatAllocationRequestDto
{
    public int ClassroomId { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public int StudentId { get; set; }
}
