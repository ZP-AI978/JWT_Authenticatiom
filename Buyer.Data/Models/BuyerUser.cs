﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Context.Models
{
    public partial class BuyerUser
    {
        public BuyerUser()
        {
            BuyerUserRoles = new HashSet<BuyerUserRole>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string DisplayName { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? ModiefiedDate { get; set; }

        public virtual ICollection<BuyerUserRole> BuyerUserRoles { get; set; }
    }
}