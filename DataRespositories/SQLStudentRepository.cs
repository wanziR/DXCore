using CMS5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS5.Infrastructure;

namespace CMS5.DataRespositories
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public SQLStudentRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 接口实现-通过id获取学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Student GetStudentById(int id)
        {
            return _context.Students.Find(id);
        }

        /// <summary>
        /// 接口实现-获取所有学生信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Student> GetAllStudent()
        {
            return _context.Students;
        }

        /// <summary>
        /// 接口实现-添加学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Student Insert(Student student)
        {
            _context.Add(student);
            _context.SaveChanges();
            return student;
        }

        /// <summary>
        /// 接口实现-修改学生信息
        /// </summary>
        /// <param name="updateStudent"></param>
        /// <returns></returns>
        public Student Update(Student updateStudent)
        {
            var student = _context.Students.Attach(updateStudent);
            _context.SaveChanges();
            return updateStudent;
        }

        /// <summary>
        /// 接口实现-删除学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Student Delete(int id)
        {
            Student student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            return student;
        }

        
    }
}
