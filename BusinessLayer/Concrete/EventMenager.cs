using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class EventMenager : IEventService
    {
        private readonly IEventDal _eventDal;

        public EventMenager(IEventDal eventDal)
        {
            _eventDal = eventDal;
        }

        public void Delete(Event t)
        {
            _eventDal.Delete(t);
        }

        public Event GetById(int id)
        {
            return _eventDal.GetById(id);
        }

        public List<Event> GetListAll()
        {
            return _eventDal.GetListAll();
        }

        public void Insert(Event t)
        {
            _eventDal.Insert(t);
        }

        public void Update(Event t)
        {
            _eventDal.Update(t);
        }
    }
}
