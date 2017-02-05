namespace BlackNails.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlackNails.DAL.DbContexts>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DAL.DbContexts context)
        {
            //  This method will be called after migrating to the latest version.

            context.Users.AddOrUpdate(
                u => u.UserName,
                new UserModel { UserName = "超级管理员", UserNickName = "超级管理员", Role = "超级管理员", RegistrationTime = System.DateTime.Now, Password = "88888888", LoginTime = System.DateTime.Now }
                );
        }
    }
}
