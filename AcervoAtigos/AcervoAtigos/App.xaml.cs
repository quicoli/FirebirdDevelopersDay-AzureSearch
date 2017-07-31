using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AcervoAtigos.Persistencia.MapeamentoNh;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Azure.Search;
using NHibernate;
using NHibernate.Context;

namespace AcervoAtigos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ISessionFactory SessionFactory { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var fluentConfig = Fluently.Configure()
                .CurrentSessionContext<ThreadStaticSessionContext>()
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

            var nhConfiguration = fluentConfig.BuildConfiguration();
            SessionFactory = nhConfiguration.BuildSessionFactory();
            base.OnStartup(e);
        }

        public static SearchServiceClient CreateAdminSearchServiceClient()
        {
            string searchServiceName = "acervoartigos";
            string adminApiKey = "HERE-INSERT-YOUR-KEY";

            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
            return serviceClient;
        }

        public static SearchServiceClient CreateQuerySearchServiceClient()
        {
            string searchServiceName = "acervoartigos";
            string queryApiKey = "HERE-INSERT-YOUR-KEY";

            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(queryApiKey));
            return serviceClient;
        }
    }
}
