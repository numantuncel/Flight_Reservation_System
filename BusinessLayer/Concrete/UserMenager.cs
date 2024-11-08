using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserMenager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserMenager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public void Delete(User t)
        {
            _userDal.Delete(t);
        }

        public User FindByEmail(string email)
        {
            return _userDal.FindByEmail(email);
        }

        public User GetById(int id)
        {
            return _userDal.GetById(id);
        }

        public List<User> GetListAll()
        {
            return _userDal.GetListAll();
        }

        public void Insert(User t)
        {
            _userDal.Insert(t);
        }

        public void Update(User t)
        {
            _userDal.Update(t);
        }
    }
}
