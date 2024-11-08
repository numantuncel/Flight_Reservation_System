using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repository;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfTicketDal : GenericRepository<Ticket>, ITicketDal
    {
        public EfTicketDal(FlightReservation_Context context) : base(context)
        {
        }
    }
}
