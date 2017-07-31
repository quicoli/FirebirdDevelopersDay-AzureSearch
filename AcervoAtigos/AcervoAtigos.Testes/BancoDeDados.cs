using System.Data;
using AcervoAtigos.Persistencia.MapeamentoNh;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace AcervoAtigos.Testes
{
    [TestFixture]
    public class BancoDeDados
    {
        [Test]
        public void PodeAtualizarBancoDeDados()
        {
            var fluentConfig = Fluently.Configure()
                .CurrentSessionContext<ThreadLocalSessionContext>()
                .Database(new FirebirdConfiguration()
                    .ConnectionString(
                        x =>
                            x.Is(@"User=SYSDBA;Password=masterkey;Database=D:\Documentos\FDD 14\Demo\Database\artigos.fdb;DataSource=localhost;Port=3050;Dialect=3;Charset=WIN1252;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;"))
                    .IsolationLevel(IsolationLevel.ReadCommitted)
                    .AdoNetBatchSize(20)
                    .ShowSql().FormatSql())
                .Mappings(mapper =>
                {
                    mapper.FluentMappings
                        .AddFromAssemblyOf<ArtigoMapping>();

                });

            Assert.DoesNotThrow(() =>
            {
                var nhConfiguration = fluentConfig.BuildConfiguration();
                var sessionFactory = nhConfiguration.BuildSessionFactory();

                using (var session = sessionFactory.OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        new SchemaUpdate(nhConfiguration).Execute(true, true);

                        tx.Commit();
                    }
                }
            });
        }
    }
}