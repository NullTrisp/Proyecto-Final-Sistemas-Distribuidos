using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProjectService.Models
{
    public interface IUser
    {
        string Username
        {
            get; set;
        }

        string Password
        {
            get; set;
        }
    }
    public class User : RealmObject, IUser
    {
        [PrimaryKey]
        [Indexed]
        public string Username { get; set; }

        public string Password { get; set; }

        public static User Read(string user, string password)
        {

        }

        public 

    }
}