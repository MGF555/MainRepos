using Exercises.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exercises.Models.Data;
using Exercises.Models.ViewModels;

namespace Exercises.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult List()
        {
            var model = StudentRepository.GetAll();

            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var viewModel = new StudentVM();
            viewModel.SetCourseItems(CourseRepository.GetAll());
            viewModel.SetMajorItems(MajorRepository.GetAll());
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add(StudentVM studentVM)
        {
            studentVM.Student.Courses = new List<Course>();
            var model = StudentRepository.GetAll();

            foreach (var id in studentVM.SelectedCourseIds)
                studentVM.Student.Courses.Add(CourseRepository.Get(id));

            studentVM.Student.Major = MajorRepository.Get(studentVM.Student.Major.MajorId);
            studentVM.Student.StudentId = model.Max(s => s.StudentId) + 1;
            studentVM.Student.Address = new Address();
            studentVM.Student.Address.AddressId = studentVM.Student.StudentId;

            StudentRepository.Add(studentVM.Student);

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = StudentRepository.Get(id);
            model.Address.AddressId = id;
            var editModel = new StudentVM();
            editModel.Student = model;
            editModel.SetCourseItems(CourseRepository.GetAll());
            editModel.SetMajorItems(MajorRepository.GetAll());
            return View(editModel);
        }

        [HttpPost]
        public ActionResult Edit(StudentVM editModel)
        {
            editModel.Student.Major = MajorRepository.Get(editModel.Student.Major.MajorId);

            StudentRepository.Edit(editModel.Student);
            StudentRepository.SaveAddress(editModel.Student.StudentId, editModel.Student.Address);
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var student = StudentRepository.Get(id);
            return View(student);
        }

        [HttpPost]
        public ActionResult Delete(Student student)
        {
            StudentRepository.Delete(student.StudentId);
            return RedirectToAction("List");
        }
    }
}