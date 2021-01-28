using Microsoft.EntityFrameworkCore;
using Susep.SISRH.Domain.AggregatesModel;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;
using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using Susep.SISRH.Infrastructure.EntityConfigurations;
using Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao;
using Susep.SISRH.Domain.AggregatesModel.ObjetoAggregate;

namespace Susep.SISRH.Infrastructure.Contexts
{
    public class SISRHDbContext : DbContext
    {
        public SISRHDbContext(DbContextOptions<SISRHDbContext> options) : base(options) { }


        public DbSet<CatalogoDominio> CatalogoDominio { get; set; }

        public DbSet<Catalogo> Catalogo { get; set; }
        public DbSet<ItemCatalogo> ItemCatalogo { get; set; }

        public DbSet<PactoTrabalho> PactoTrabalho { get; set; }
        public DbSet<PactoTrabalhoAtividade> PactoTrabalhoAtividade { get; set; }
        public DbSet<PactoTrabalhoSolicitacao> PactoTrabalhoSolicitacao { get; set; }
        public DbSet<PactoTrabalhoHistorico> PactoTrabalhoHistorico { get; set; }
        public DbSet<PactoAtividadePlanoObjeto> PactoAtividadePlanoObjeto { get; set; }

        public DbSet<PlanoTrabalho> PlanoTrabalho { get; set; }
        public DbSet<PlanoTrabalhoAtividade> PlanoTrabalhoAtividade { get; set; }
        public DbSet<PlanoTrabalhoAtividadeItem> PlanoTrabalhoAtividadeItem { get; set; }
        public DbSet<PlanoTrabalhoAtividadeCandidato> PlanoTrabalhoAtividadeCandidato { get; set; }
        public DbSet<PlanoTrabalhoAtividadeCandidatoHistorico> PlanoTrabalhoAtividadeCandidatoHistorico { get; set; }

        public DbSet<PlanoTrabalhoMeta> PlanoTrabalhoMeta { get; set; }
        public DbSet<PlanoTrabalhoReuniao> PlanoTrabalhoReuniao { get; set; }
        public DbSet<PlanoTrabalhoCusto> PlanoTrabalhoCusto { get; set; }
        public DbSet<PlanoTrabalhoHistorico> PlanoTrabalhoHistorico { get; set; }
        public DbSet<PlanoTrabalhoObjeto> PlanoTrabalhoObjeto { get; set; }
        public DbSet<PlanoTrabalhoObjetoAssunto> PlanoTrabalhoObjetoAssunto { get; set; }

        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Unidade> Unidade { get; set; }

        public DbSet<PessoaModalidadeExecucao> PessoaModalidadeExecucao { get; set; }
        public DbSet<UnidadeModalidadeExecucao> UnidadeModalidadeExecucao { get; set; }

        public DbSet<Assunto> Assunto { get; set; }

        public DbSet<Objeto> Objeto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies(false);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Schema dbo
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.ApplyConfiguration(new PessoaEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UnidadeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FeriadoEntityTypeConfiguration());            

            //Schema ProgramaGestao
            modelBuilder.ApplyConfiguration(new CatalogoDominioEntityTypeConfiguration());
            
            modelBuilder.ApplyConfiguration(new CatalogoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogoItemCatalogoEntityTypeConfiguration());            
            modelBuilder.ApplyConfiguration(new ItemCatalogoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ItemCatalogoAssuntoEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PactoTrabalhoEntityTypeConfiguration()); 
            modelBuilder.ApplyConfiguration(new PactoTrabalhoAtividadeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PactoTrabalhoSolicitacaoEntityTypeConfiguration());            
            modelBuilder.ApplyConfiguration(new PactoTrabalhoHistoricoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PactoTrabalhoAtividadeAssuntoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PactoAtividadePlanoObjetoEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PlanoTrabalhoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoHistoricoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoAtividadeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoAtividadeItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoAtividadeCriterioEntityTypeConfiguration());            
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoAtividadeCandidatoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoAtividadeCandidatoHistoricoEntityTypeConfiguration());            
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoAtividadeAssuntoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoMetaEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoReuniaoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoCustoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoObjetoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanoTrabalhoObjetoAssuntoEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PessoaModalidadeExecucaoEntityTypeConfiguration());
            
            modelBuilder.ApplyConfiguration(new UnidadeModalidadeExecucaoEntityTypeConfiguration());
           
            modelBuilder.ApplyConfiguration(new AssuntoEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new ObjetoEntityTypeConfiguration());

        }
    }
}

