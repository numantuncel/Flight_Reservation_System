using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        User FindByEmail(string email); // Kullanıcıyı e-posta ile bulma metodu
    }
}
