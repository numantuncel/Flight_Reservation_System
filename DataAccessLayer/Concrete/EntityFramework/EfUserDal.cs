using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repository;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfUserDal : GenericRepository<User>, IUserDal
    {
        private readonly FlightReservation_Context _context;

        public EfUserDal(FlightReservation_Context context)
            : base(context) // Sadece _context'i geçiyoruz
        {
            _context = context; // Ekstra bir atama yapmaya gerek yok
        }


        public User FindByEmail(string email)
        {
            // E-posta adresine göre kullanıcıyı bul
            return _context.Set<User>().FirstOrDefault(user => user.Email == email);
        }
    }
}
