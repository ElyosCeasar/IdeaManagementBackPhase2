﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Mapper
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class IdeaManagmentDatabaseEntities : DbContext
    {
        public IdeaManagmentDatabaseEntities()
            : base("name=IdeaManagmentDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<COMMENT_POINTS> COMMENT_POINTS { get; set; }
        public DbSet<COMMITTEE_VOTE_DETAIL> COMMITTEE_VOTE_DETAIL { get; set; }
        public DbSet<IDEA_COMMENTS> IDEA_COMMENTS { get; set; }
        public DbSet<IDEA_POINTS> IDEA_POINTS { get; set; }
        public DbSet<IDEA_SELECTED> IDEA_SELECTED { get; set; }
        public DbSet<IDEA_STATUS> IDEA_STATUS { get; set; }
        public DbSet<IDEA> IDEAS { get; set; }
        public DbSet<USER> USERS { get; set; }
    }
}