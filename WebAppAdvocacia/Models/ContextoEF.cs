using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebAppAdvocacia.Models
{
    public class ContextoEF : DbContext
    {
        public ContextoEF():base("name=ContextoEF")
        {

        }
        public DbSet<Audiencia> Audiencias { get; set; }
        public DbSet<Custa> Custas { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
        public DbSet<Processo> Processos { get; set; }
        public DbSet<Tribunal> Tribunais { get; set; }
        public DbSet<Vara> Varas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<PessoaFisica>().ToTable("PessoaFisica");
            modelBuilder.Entity<PessoaJuridica>().ToTable("PessoaJuridica");
        }
    }
}