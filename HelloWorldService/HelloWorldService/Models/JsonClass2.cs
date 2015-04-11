using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloWorldService.Models.Exercises
{
    public class Course
    {
        public int CourseId;
        public string Name;
    }

    public class Student
    {
        public int StudentId;
        public string Name;
        public Course[] Courses;
    }

    public class Exercise2
    {

        public void Test()
        {
            var students = new Student[]
            {
                new Student
                {
                    StudentId = 101,
                    Name = "Steve",
                    Courses = new[]
                    {
                        new Course {CourseId = 101, Name = "C#"},
                        new Course {CourseId = 201, Name = "Advanced C#"}
                    }
                },
                new Student
                {
                    StudentId = 201,
                    Name = "Dave"
                }
            };

//            string jsonRequest = @"
//[
//    {
//        "StudentId": 101,
//        "Name": "Steve",
//        "Courses": [
//            { "CourseId": 101, "Name": "C#" },
//            { "CourseId": 201, "Name": "Advanced C#" }
//        ]
//    },
//    { "StudentId": 201, "Name": "Dave" }
//]";
        }
    }
}