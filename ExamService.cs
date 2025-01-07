using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class PrincipalService
{
    private readonly ExamManagementDbContext _context;

    public PrincipalService(ExamManagementDbContext context)
    {
        _context = context;
    }

    public IEnumerable<StudentDto> SearchStudents(string name)
    {
        // Placeholder for Solr integration. Replace with Solr query.
        return _context.Students
            .Where(s => EF.Functions.Like(s.Name, $"%{name}%"))
            .Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
                RollNumber = s.RollNumber,
                House = s.House,
                ClassName = s.Class.Name,
                SeatInfo = s.SeatAllocation == null ? "Not Allocated" : $"Room {s.SeatAllocation.Classroom.Name}, Row {s.SeatAllocation.Row}, Column {s.SeatAllocation.Column}"
            }).ToList();
    }

    public bool AllocateSeat(SeatAllocationRequestDto request)
    {
        // Validation rules here
        var seatAllocations = _context.SeatAllocations.Where(sa => sa.ClassroomId == request.ClassroomId).ToList();
        var student = _context.Students.Find(request.StudentId);

        if (seatAllocations.Any(sa => sa.Row == request.Row || sa.Column == request.Column && sa.Student.ClassId == student.ClassId))
            throw new Exception("Same class students cannot sit in the same row or column.");

        if (seatAllocations.Any(sa => Math.Abs(sa.Row - request.Row) <= 1 && Math.Abs(sa.Column - request.Column) <= 1 && sa.Student.House == student.House))
            throw new Exception("Students of the same house cannot sit adjacent to each other.");

        // Allocate seat
        var seatAllocation = new SeatAllocation
        {
            ClassroomId = request.ClassroomId,
            Row = request.Row,
            Column = request.Column,
            StudentId = request.StudentId
        };

        _context.SeatAllocations.Add(seatAllocation);
        _context.SaveChanges();
        return true;
    }

    public void AddScore(int studentId, int subjectId, int score)
    {
        var studentSubject = _context.StudentSubjects.FirstOrDefault(ss => ss.StudentId == studentId && ss.SubjectId == subjectId);
        if (studentSubject == null)
        {
            throw new Exception("Student is not enrolled in this subject.");
        }

        studentSubject.Score = score;
        _context.SaveChanges();
    }
}

public class StudentService
{
    private readonly ExamManagementDbContext _context;

    public StudentService(ExamManagementDbContext context)
    {
        _context = context;
    }

    public StudentDto GetStudentDetails(int studentId)
    {
        var student = _context.Students
            .Where(s => s.Id == studentId)
            .Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
                RollNumber = s.RollNumber,
                House = s.House,
                ClassName = s.Class.Name,
                SeatInfo = s.SeatAllocation == null ? "Not Allocated" : $"Room {s.SeatAllocation.Classroom.Name}, Row {s.SeatAllocation.Row}, Column {s.SeatAllocation.Column}"
            }).FirstOrDefault();

        return student;
    }
}
