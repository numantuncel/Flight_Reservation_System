using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using System;
using EntityLayer.Concrete;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Repository
{
    public class GenericRepository<T> : IGenericDal<T> where T : class, new()
    {
        private readonly FlightReservation_Context _context;

        public GenericRepository(FlightReservation_Context context)
        {
            _context = context; // FlightReservationSystem_Context'i burada alıyoruz
        }

        public void Delete(T t)
        {

            _context.Remove(t);
            _context.SaveChanges();
        }



        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }



        public List<T> GetListAll()
        {
            return _context.Set<T>().ToList();
        }

        public void Insert(T t)
        {
            _context.Add(t);
            _context.SaveChanges();
        }

        public void Update(T t)
        {
            _context.Update(t);
            _context.SaveChanges();
        }
    }
}
