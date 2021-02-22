using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS5.Models;

namespace CMS5.DataRespositories
{
  
        public interface IStudentRepository
        {
        
        //通过id获取学生信息
        Student GetStudentById(int id);
        //获取所有学生信息
        IEnumerable<Student> GetAllStudent();
        //添加学生信息
        Student Insert(Student student);
        //修改学生信息
        Student Update(Student updateStudent);
        //删除学生信息
        Student Delete(int id);

    }
  
}
