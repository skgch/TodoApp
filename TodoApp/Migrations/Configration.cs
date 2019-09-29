using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using TodoApp.Models;

namespace TodoApp.Migrations
{
    internal sealed class Configration : DbMigrationsConfiguration<TodoApp.Models.TodoesContext>
    {
        public Configration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TodoApp.Models.TodoesContext";
        }

        protected override void Seed(TodoesContext context)
        {
            User admin = new User()
            {
                Id = 1,
                UserName = "admin",
                Password = new CustomMembershipProvider().GeneratePasswodHash("admin", "password"),
                Roles = new List<Role>()
            };

            Role administrators = new Role()
            {
                Id = 1,
                RoleName = "Administrators",
                Users = new List<User>()
            };

            Role users = new Role()
            {
                Id = 2,
                RoleName = "Users",
                Users = new List<User>()
            };

            admin.Roles.Add(administrators);
            administrators.Users.Add(admin);

            context.Users.AddOrUpdate(user => user.Id, new User[] { admin });
            context.Roles.AddOrUpdate(role => role.Id, new Role[] { administrators, users });
        }
    }
}