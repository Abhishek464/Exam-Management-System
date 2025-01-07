using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// Data Models
public class Student
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int RollNumber { get; set; }
    public string House { get; set; } // Blue, Green, Red, Yellow
    public int ClassId { get; set; }
    public Class Class { get; set; }
    public ICollection<StudentSubject> StudentSubjects { get; set; }
    public SeatAllocation SeatAllocation { get; set; }
}

public class Class
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } // e.g., Class 1, Class 2
    public ICollection<Student> Students { get; set; }
}

public class Subject
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<StudentSubject> StudentSubjects { get; set; }
}

public class StudentSubject
{
    [Key]
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    public int Score { get; set; }
}

public class SeatAllocation
{
    [Key]
    public int Id { get; set; }
    public int ClassroomId { get; set; }
    public Classroom Classroom { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
}

public class Classroom
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } // e.g., Room 1, Room 2
    public ICollection<SeatAllocation> SeatAllocations { get; set; }
}
