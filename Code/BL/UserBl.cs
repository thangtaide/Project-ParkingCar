using System;
using DAL;

namespace BL
{
    public class UserBl
    {
        private UserDal dal = new UserDal();
        public int Login(string userName, string password){
            return dal.Login(userName, password);
        }
    }
}
