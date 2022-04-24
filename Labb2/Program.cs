using Labb2.Contexts;
using Labb2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Labb2
{
    internal class Program
    {
        static private List<Teacher> teachersList = new List<Teacher>();
        static private List<Student> studentsList = new List<Student>();
        static private List<Course> coursesList = new List<Course>();
        static private List<Class> classesList = new List<Class>();
        static private List<StudentSchedule> studentScheduleList = new List<StudentSchedule>();

        static private bool quit;
        static void Main(string[] args)
        {
            Console.WriteLine("Loading data, please wait...");
            StartUp();
            do
            {
                Meny();

            } while (quit == false);
        }
        //static void SeedClass()
        //{
        //    using SchoolContext context = new SchoolContext();
        //    var class_ = new Class
        //    {
        //        ClassName = "3A"
        //    };
            
        //    context.Classes.Add(class_);
        //    context.SaveChanges();

        //}
        //static void SeedTeacher()
        //{
        //    using SchoolContext context = new SchoolContext();
        //    var teacher = new Teacher
        //    {
        //        TeacherName = "Anas Alhussain"
        //    };
          
        //    context.Teachers.Add(teacher);
        //    context.SaveChanges();
        //}
        //static void SeedCourse()
        //{
        //    using SchoolContext context = new SchoolContext();
        //    var course = new Course
        //    {
        //        CourseName = "Programmering 1"
        //    };
        //    context.Courses.Add(course);
        //    context.SaveChanges();  
        //}
        //static void SeedStudent()
        //{
        //    using SchoolContext context = new SchoolContext();
        //    var student = new Student
        //    {
        //        StudentName = "Nalle Puh",
        //        ClassId = 1

        //    };
          
        //    context.Students.Add(student);
        //    context.SaveChanges();
        //}
        //static void SeedStudentSchedule()
        //{
        //    using SchoolContext context = new SchoolContext();
        //    var studentSchedule = new StudentSchedule
        //    {
        //        CourseId = 1,
        //        TeacherId = 1,
        //        StudentId = 1,
        //    };
        //    context.StudentSchedules.Add(studentSchedule);
        //    context.SaveChanges();
        //}
        static void Meny()
        {
            try
            {
                Console.WriteLine("Welcome to the School Schedule, Please choose on option " +
                            "\n\nChoose an Option: " +
                            "\n1.Get all Teachers for a specific course. " +
                            "\n2.Get all Scheduleinfo." +
                            "\n3.Get all teachers and their students. " +
                            "\n4.Get all Students and teachers for a specific course" +
                            "\n5.Edit Course name" +
                            "\n6.Edit Teacher for a specific student" +
                            "\n7.Exit program");
                string choicestring = Console.ReadLine();
                int choice = int.Parse(choicestring);

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter the course you want to see the teaching teachers from: ");
                        var userInput = Console.ReadLine();
                        GetTeacherForSpecificCourse(userInput);
                        Console.ReadLine();
                        break;
                    case 2:
                        GetAllSchoolInfo();
                        Console.ReadLine();
                        break;
                    case 3:
                        GetAllTeachersAndTheirStudents();
                        Console.ReadLine();
                        break;
                    case 4:
                        Console.WriteLine("Enter the course you want to see the teaching teachers and students from: ");
                        var userInput1 = Console.ReadLine();
                        GetAllTeachersAndStudentsSpecificCourse(userInput1);
                        break;
                    case 5:
                        Console.WriteLine("Enter the coursename you want to edit: ");
                        var userInput2 = Console.ReadLine();
                        Console.WriteLine("Please enter the name you wanna change " + userInput2 + " too!: ");
                        var userInput3 = Console.ReadLine();    
                        EditCourseName(userInput2, userInput3);
                        coursesList.Clear();

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Loading new data, please wait...");
                        Console.ResetColor();

                        GetCoursesToList();

                        Console.WriteLine("Press any key to continue!");
                        Console.ReadLine(); 
                        break;
                    case 6: 
                        Console.WriteLine("Please enter the students name: ");
                        var studentsname = Console.ReadLine();

                        var outputlist = from studentSchedule in studentScheduleList
                                         join course in coursesList on studentSchedule.CourseId equals course.CourseId
                                         join teacher in teachersList on studentSchedule.TeacherId equals teacher.TeacherId
                                         join student in studentsList on studentSchedule.StudentId equals student.StudentId
                                         where student.StudentName == studentsname
                                         select new {courseName = course.CourseName, teacherName = teacher.TeacherName,};

                        if(outputlist != null)
                        {
                            Console.WriteLine(studentsname + " attends " + outputlist.Count() + " Courses: ");
                            foreach (var output in outputlist)
                            {
                                Console.WriteLine(output.courseName + " --- " + output.teacherName);
                            }
                            Console.WriteLine("Please enter the name of The Course where you want to change " + studentsname +"s Teacher");
                            var coursename = Console.ReadLine();
                            EditTeacherForSpecificStudent(studentsname,coursename);
                        }
                        else if (outputlist == null)
                        {
                            Console.WriteLine("No Student found with that name!");
                        }
                        teachersList.Clear();   
                        coursesList.Clear();
                        studentScheduleList.Clear();

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Loading new data, please wait...");
                        Console.ResetColor();

                        GetTeachersToList();
                        GetSchoolSchedulesToList();
                        GetCoursesToList();

                        Console.WriteLine("Press any key to continue!");
                        Console.ReadLine(); 
                        break;
                    case 7:
                        Console.WriteLine("Exiting Program, thank you for using...");
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Number, Please enter an option (1-7)");
                        break;

                }
                Console.Clear();

            }
            catch
            {
                Console.WriteLine("Error, Something went wrong!");
            }


        }
        static void StartUp()
        {
            GetTeachersToList();
            GetStudentsToList();
            GetCoursesToList();
            GetClassesToList();
            GetSchoolSchedulesToList();
        }
        static void GetTeachersToList()
        {
            try
            {
                using SchoolContext context = new SchoolContext();
                var DBcontent = context.Teachers;

                foreach (Teacher teacher in DBcontent)
                {
                    teachersList.Add(teacher);
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong while fetching the data for Teachers");
            }
        }
        static void GetStudentsToList()
        {
            try
            {
                using SchoolContext context = new SchoolContext();
                var DBcontent = context.Students;
                foreach (Student student in DBcontent)
                {
                    studentsList.Add(student);
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong while fetching the data for Students");
            }

        }
        static void GetCoursesToList()
        {
            try
            {
                using SchoolContext context = new SchoolContext();
                var DBcontent = context.Courses;
                foreach (Course course in DBcontent)
                {
                    coursesList.Add(course);
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong while fetching the data for Courses");
            }

        }
        static void GetClassesToList()
        {
            try
            {
                using SchoolContext context = new SchoolContext();
                var DBcontent = context.Classes;
                foreach (Class _class in DBcontent)
                {
                    classesList.Add(_class);
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong while fetching the data for Classes");
            }

        }
        static void GetSchoolSchedulesToList()
        {
            try
            {
                using SchoolContext context = new SchoolContext();
                var DBcontent = context.StudentSchedules;
                foreach (StudentSchedule schoolSchedule in DBcontent)
                {
                    studentScheduleList.Add(schoolSchedule);
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong while fetching the data for SchoolSchedule");
            }

        }
        static void GetTeacherForSpecificCourse(string inputCourse)
        {
            var outputlist = from studentSchedule in studentScheduleList
                             join course in coursesList on studentSchedule.CourseId equals course.CourseId
                             join teacher in teachersList on studentSchedule.TeacherId equals teacher.TeacherId
                             where course.CourseName.ToLower() == inputCourse.ToLower()
                             select new { courseName = course.CourseName, teacherName = teacher.TeacherName };

            if(outputlist.Any() == true)
            {
                foreach (var output in outputlist)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\tCoursename: " + output.courseName);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\tTeacher:  " + output.teacherName);
                    Console.ResetColor();
                }
            }
            else if (outputlist.Any() != true)
            {
                Console.WriteLine("No Result");
            }
        }
        static void GetAllSchoolInfo()
        {
            var outputlist = from studentSchedule in studentScheduleList
                             join course in coursesList on studentSchedule.CourseId equals course.CourseId
                             join teacher in teachersList on studentSchedule.TeacherId equals teacher.TeacherId
                             join student in studentsList on studentSchedule.StudentId equals student.StudentId
                             join _class in classesList on student.ClassId equals _class.ClassId
                             select new { courseName = course.CourseName, teacherName = teacher.TeacherName, className = _class.ClassName, studentName = student.StudentName };

            if (outputlist.Any() == true)
            {
                foreach (var output in outputlist)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\tCoursename: " + output.courseName);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\tTeacher:  " + output.teacherName);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\tClassname: " + output.className);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\tStudentname: " + output.studentName);
                    Console.ResetColor();
                }
            }
            else if (outputlist.Any() != true)
            {
                Console.WriteLine("No Result");
            }
        }
        static void GetAllTeachersAndTheirStudents()
        {
            var outputlist = from studentSchedule in studentScheduleList
                             join teacher in teachersList on studentSchedule.TeacherId equals teacher.TeacherId
                             join student in studentsList on studentSchedule.StudentId equals student.StudentId
                             join _class in classesList on student.ClassId equals _class.ClassId  
                             select new { teacherName = teacher.TeacherName, studentName = student.StudentName };

            if(outputlist.Any() == true)
            {
                foreach (var output in outputlist)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\tTeacher:  " + output.teacherName);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\tStudentname: " + output.studentName);
                    Console.ResetColor();
                }
            }
            else if (outputlist.Any() != true)
            {
                Console.WriteLine("No Result");
            }
        }
        static void GetAllTeachersAndStudentsSpecificCourse(string inputCourse)
        {
            var outputlist = from studentSchedule in studentScheduleList
                             join teacher in teachersList on studentSchedule.TeacherId equals teacher.TeacherId
                             join course in coursesList on studentSchedule.CourseId equals course.CourseId
                             join student in studentsList on studentSchedule.StudentId equals student.StudentId
                             join _class in classesList on student.ClassId equals _class.ClassId
                             where course.CourseName.ToLower() == inputCourse.ToLower()
                             select new { courseName = course.CourseName, teacherName = teacher.TeacherName, studentName = student.StudentName };

            if(outputlist.Any() == true)
            {
                foreach (var output in outputlist)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\tCoursename: " + output.courseName);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\tTeacher:  " + output.teacherName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\tStudentname: " + output.studentName);
                    Console.ResetColor();
                }
            }
            else if (outputlist.Any() != true)
            {
                Console.WriteLine("No Result");
            }
        }
        static void EditCourseName(string courseName,string newCourseName)
        {
           
            using SchoolContext context = new SchoolContext();
            var result = context.Courses.SingleOrDefault(c => c.CourseName.ToLower() == courseName.ToLower());
            if (result != null)
            {
                result.CourseName = newCourseName;
                context.SaveChanges();
                Console.WriteLine("Name successfully changed!");
            }
            else if(result == null)
            {
                Console.WriteLine("No results for the coursename of " + courseName + "!");
            }

        }
        static void EditTeacherForSpecificStudent(string studentName, string courseName)
        {
            int theStudentsId = 0;
            int theCoursesId = 0;


            Console.WriteLine("What Teacher do you want " + studentName + " to have in " + courseName + "?");
            var NewTeachersName = Console.ReadLine();

            using (SchoolContext context = new SchoolContext())
            {
                var outputList = from studentSchedule in context.StudentSchedules
                                 join student in context.Students on studentSchedule.StudentId equals student.StudentId
                                 join course in context.Courses on studentSchedule.CourseId equals course.CourseId
                                 where student.StudentName.ToLower() == studentName.ToLower() && course.CourseName.ToLower() == courseName.ToLower()
                                 select new
                                 {
                                     studentId = student.StudentId,
                                     courseId = course.CourseId,
                                 };
                foreach(var output in outputList)
                {
                    theStudentsId = output.studentId;
                    theCoursesId = output.courseId; 
                }
            };

            using (SchoolContext context1 = new SchoolContext())
            {
                var result = context1.Teachers.SingleOrDefault(t => t.TeacherName.ToLower() == NewTeachersName.ToLower());

                if (result == null)
                {
                    var teacher = new Teacher
                    {
                        TeacherName = NewTeachersName
                    };
                    context1.Add(teacher);
                    context1.SaveChanges();

                    var getnewid = context1.Teachers.SingleOrDefault(t => t.TeacherName.ToLower() == NewTeachersName.ToLower());

                    var newId = getnewid.TeacherId;

                    var changeTeacher = context1.StudentSchedules.SingleOrDefault(s => s.StudentId == theStudentsId && s.CourseId == theCoursesId);
                    if (changeTeacher == null)
                    {
                        Console.WriteLine("Something went wrong");
                    }
                    else if (changeTeacher != null)
                    {
                        changeTeacher.TeacherId = newId;
                        context1.SaveChanges();
                        Console.WriteLine("Teacher successfully changed!");
                    }

                }
                else if (result != null)
                {
                    var newId = result.TeacherId;

                    var changeTeacher = context1.StudentSchedules.SingleOrDefault(s => s.StudentId == theStudentsId && s.CourseId == theCoursesId);
                    if (changeTeacher == null)
                    {
                        Console.WriteLine("Something went wrong");
                    }
                    else if (changeTeacher != null)
                    {
                        changeTeacher.TeacherId = newId;
                        context1.SaveChanges();
                        Console.WriteLine("Teacher successfully changed!");
                    }
                }

            }
        }
    }
}
