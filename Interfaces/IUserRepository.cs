﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IUserRepository
    {
        User GetUserByID(int _userID);
        List<User> GetAllUsers();
    }
}
