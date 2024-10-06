using MH.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MH.DataAccess
{
    public class MHDbContext : DbContext
    {
        public MHDbContext(DbContextOptions<MHDbContext> options) : base(options)
        {

        }

        public virtual DbSet<SystemUsers> SystemUsers { get; set; }
        public virtual DbSet<UserSession> UserSession { get; set; }
        public virtual DbSet<ForgotPasswordToken> ForgotPasswordToken { get; set; }
        public virtual DbSet<Roles> Role { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Actions> Actions { get; set; }
        public virtual DbSet<RolePermissionMapping> RolePermissionMapping { get; set; }
        public virtual DbSet<RoleActionMapping> RoleActionMapping { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemUsers>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.ToTable("SystemUsers");
            });
            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.HasKey(x => x.UserSessionID);
                entity.ToTable("UserSession");
            });
            modelBuilder.Entity<ForgotPasswordToken>(entity =>
            {
                entity.HasKey(x => x.ForgotPassWordID);
                entity.ToTable("ForgotPasswordToken");
            });
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(x => x.RoleID);
                entity.ToTable("Role");
            });
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(x => x.PermissionID);
                entity.ToTable("Permission");
            });
            modelBuilder.Entity<Actions>(entity =>
            {
                entity.HasKey(x => x.ActionID);
                entity.ToTable("Actions");
            });
            modelBuilder.Entity<RolePermissionMapping>(entity =>
            {
                entity.HasKey(x => x.RolePermissionMappingID);
                entity.ToTable("RolePermissionMapping");
            });
            modelBuilder.Entity<RoleActionMapping>(entity =>
            {
                entity.HasKey(x => x.RoleActionMappingID);
                entity.ToTable("RoleActionMapping");
            });
        }
    }
}
