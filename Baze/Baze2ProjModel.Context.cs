﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Baze
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BazeProjekatEntities : DbContext
    {
        public BazeProjekatEntities()
            : base("name=BazeProjekatEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Avion> Avion { get; set; }
        public virtual DbSet<Karta> Karta { get; set; }
        public virtual DbSet<Let> Let { get; set; }
        public virtual DbSet<Obavestenje> Obavestenje { get; set; }
        public virtual DbSet<Osoba> Osoba { get; set; }
        public virtual DbSet<Pilot> Pilot { get; set; }
        public virtual DbSet<Pista> Pista { get; set; }
        public virtual DbSet<Putnik> Putnik { get; set; }
        public virtual DbSet<Radnik> Radnik { get; set; }
        public virtual DbSet<Upravljati> Upravljati { get; set; }
    }
}
