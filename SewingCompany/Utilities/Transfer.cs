﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingCompany.Utilities
{
    public static class Transfer
    {

        public static User LoggedUser;

        //Order stuff
        public static List<OrderList> OrderItems = new List<OrderList>();
        public static Order CurrentOrder;

    }
}
