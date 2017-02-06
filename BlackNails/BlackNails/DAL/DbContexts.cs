using System.Data.Entity;
using BlackNails.Models;

namespace BlackNails.DAL
{
    /// <summary>
    /// 网站数据上下文
    /// <remarks>
    /// 创建：2016年3月2日
    /// </remarks>
    /// </summary>
    public class DbContexts : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<OutsideTroubleManModel> OutsideTroubleMans { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<AssessmentModel> Assessments { get; set; }
        public DbSet<MatchingModel> Matchings { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }


        /* 在程序包管理控制台输入“Enable-Migrations”命令启用迁移。
        * Migrations文件夹下的“Configuration.cs”，将构造函数中的“AutomaticMigrationsEnabled = false;”改为“AutomaticMigrationsEnabled = true;”
          * 再次在程序包管理控制台输入“Update-Database”或者 【add-migration ChildrenInfos】后在执行 “Update-Database” 命令来更新数据库。
         *             AutomaticMigrationDataLossAllowed = true;
        */
        public DbContexts()
            : base("BlackNailsContexts")
        {
            Database.SetInitializer<DbContexts>(new CreateDatabaseIfNotExists<DbContexts>());
            //Database.SetInitializer<DbContexts>(new DropCreateDatabaseIfModelChanges<DbContexts>());
           //Database.SetInitializer<DbContexts>(new DropCreateDatabaseAlways<DbContexts>());
        }
    }
}
