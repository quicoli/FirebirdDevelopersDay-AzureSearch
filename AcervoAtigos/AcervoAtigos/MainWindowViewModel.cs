using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AcervoAtigos.Entidades;
using AcervoAtigos.Search;
using AcervoAtigos.ViewModels;
using JulMar.Windows.Interfaces;
using JulMar.Windows.Mvvm;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using NHibernate.Linq;
using NHibernate.Util;
using Novacode;

namespace AcervoAtigos
{
    public class MainWindowViewModel: ViewModelNh
    {
        private BindingList<Artigo> _artigos;
        private string _edicao;
        private string _titulo;
        private string _autor;
        private BindingList<Autor> _autores;
        private Artigo _artigoSelecionado;
        private bool _destacar;
        private BindingList<Artigos> _artigosAzure;
        private string _texto;
        private Artigos _artigoAzureSelecionado;
        private BindingList<HitHighlight> _textosHighlight;
        private readonly List<Hit> _hits;

        public DelegateCommand CmdLocalizar { get; set; }
        public DelegateCommand CmdUploadDocumentos { get; set; }
        public DelegateCommand CmdLocalizarAzure { get; set; }

        public MainWindowViewModel()
        {
            Hits = new List<Hit>();
            CmdLocalizar = new DelegateCommand(DoLocalizar, CanDoLocalizar);
            CmdUploadDocumentos = new DelegateCommand(async () => await DoUploadDocumentos(), CanDoUploadDocumentos);
            CmdLocalizarAzure = new DelegateCommand(async () => await DoLocalizarAzure(), CanDoLocalizarAzure);
        }

        private bool CanDoUploadDocumentos()
        {
            return !EstaOcupado;
        }
        public async Task DoUploadDocumentos()
        {
            EstaOcupado = true;

            var pasta = ConfigurationManager.AppSettings["PastaArtigos"];
            Artigos[] listaArtigos;
            using (var tx = Session.BeginTransaction())
            {
                listaArtigos = Session.Query<Artigo>()
                    .Select(x => new Artigos()
                    {
                        Id = x.Id.ToString(),
                        Edicao = x.Edicao.ToString(),
                        Titulo = x.Titulo,
                        Conteudo = ExtrairTexto(pasta, x.Arquivo)
                    }).ToArray();
                tx.Commit();
            }

            if (listaArtigos.Any())
            {
                var serviceClient = App.CreateAdminSearchServiceClient();
                var indexClient = serviceClient.Indexes.GetClient("artigos");
                var batch = IndexBatch.MergeOrUpload(listaArtigos);
                //Delete pode excluir os documentos com os ids passados
                //var batch = IndexBatch.Delete(listaArtigos);
                await indexClient.Documents.IndexAsync(batch);
                Resolve<IMessageVisualizer>().Show("Atenção", "Upload de documentos encerrado");
            }

            EstaOcupado = false;
        }



        private async Task DoLocalizarAzure()
        {
            SearchParameters parameters = new SearchParameters()
            {
                HighlightFields = new List<string>() { "conteudo"},
                HighlightPreTag = "<font color=\"red\"> <em>",
                HighlightPostTag = "</em></font>",
                SearchMode = SearchMode.All

            };

            EstaOcupado = true;
            var serviceClient = App.CreateQuerySearchServiceClient();
            var indexClient = serviceClient.Indexes.GetClient("artigos");
            var response = await indexClient.Documents.SearchAsync<Artigos>(Texto, parameters);
            ArtigosAzure = new BindingList<Artigos>(response.Results.Select(x => x.Document).ToList());
            var hits = response.Results.Select(x => x.Highlights).ToList();

            Hits = new List<Hit>();
            int idx = 0;
            foreach (var item in hits)
            {
                if (item == null) continue;

                var objHit = new Hit {Id = long.Parse(ArtigosAzure[idx].Id)};
                foreach (var value in item.Values)
                {
                    foreach (var str in value)
                    {
                        objHit.Textos.Add(new HitHighlight() { Texto = str });
                    }
                }
                Hits.Add(objHit);
                idx++;
            }

            EstaOcupado = false;
        }

        private bool CanDoLocalizarAzure()
        {
            return !EstaOcupado && !string.IsNullOrEmpty(Texto);
        }


        private string ExtrairTexto(string pasta, string nomeArquivoDocX)
        {
            var arquivo = Path.Combine(pasta, nomeArquivoDocX);
            var doc = DocX.Load(arquivo);
            var conteudo = doc.Text;

            return conteudo;
        }

        private void DoLocalizar()
        {
            Expression<Func<Artigo, bool>> filtro = PredicateBuilder.True<Artigo>();
            if (!string.IsNullOrWhiteSpace(Edicao))
            {
                filtro = filtro.And(x => x.Edicao == int.Parse(Edicao));
            }
            if (!string.IsNullOrWhiteSpace(Titulo))
            {
                filtro = filtro.And(x => x.Titulo.Contains(Titulo));
            }
            if (!string.IsNullOrWhiteSpace(Autor))
            {
                filtro = filtro.And(x => x.Autores.Count(y => y.Autor.Nome.Contains(Autor))> 0);
            }
                
            using (var tx = Session.BeginTransaction())
            {
                var qry = Session.Query<Artigo>()
                    .Where(filtro);
                qry.FetchMany(a => a.Autores).ThenFetch(b => b.Autor).ToFuture();

                Artigos = new BindingList<Artigo>(qry.ToList());
                ArtigoSelecionado = Artigos.FirstOrDefault();
                tx.Commit();
            }
        }

        
        private bool CanDoLocalizar()
        {
            return !string.IsNullOrWhiteSpace(Edicao) || !string.IsNullOrWhiteSpace(Titulo) ||
                   !string.IsNullOrWhiteSpace(Autor);
        }

        public BindingList<HitHighlight> TextosHighlight
        {
            get { return _textosHighlight; }
            set { _textosHighlight = value; RaisePropertyChanged(()=> TextosHighlight);}
        }

        private List<Hit> Hits { get; set; }
        

        public BindingList<Autor> Autores
        {
            get { return _autores; }
            set { _autores = value; RaisePropertyChanged(()=> Autores);}
        }

        public bool Destacar
        {
            get { return _destacar; }
            set { _destacar = value; RaisePropertyChanged(()=>Destacar); }
        }

        public BindingList<Artigos> ArtigosAzure
        {
            get { return _artigosAzure; }
            set
            {
                _artigosAzure = value;
                RaisePropertyChanged(()=>ArtigosAzure);
            }
        }

        public Artigos ArtigoAzureSelecionado
        {
            get { return _artigoAzureSelecionado; }
            set
            {
                _artigoAzureSelecionado = value;
                RaisePropertyChanged(()=>ArtigoAzureSelecionado);

                if (_artigoAzureSelecionado == null || string.IsNullOrEmpty(_artigoAzureSelecionado.Id))
                {
                    Autores = new BindingList<Autor>();
                    TextosHighlight = new BindingList<HitHighlight>();
                    return;
                }

                using (var tx = Session.BeginTransaction())
                {
                    var artigo = Session.Get<Artigo>(long.Parse(_artigoAzureSelecionado.Id));
                    Autores = new BindingList<Autor>(artigo.Autores.Select(x=> x.Autor).ToList());
                    tx.Commit();
                }

                var obj = Hits.FirstOrDefault(x => x.Id == long.Parse(_artigoAzureSelecionado.Id));
                TextosHighlight = obj == null
                    ? new BindingList<HitHighlight>()
                    : new BindingList<HitHighlight>(obj.Textos);
            }
        }

        public string Texto
        {
            get { return _texto; }
            set { _texto = value; RaisePropertyChanged(()=> Texto); }
        }

        public Artigo ArtigoSelecionado
        {
            get { return _artigoSelecionado; }
            set
            {
                _artigoSelecionado = value;
                RaisePropertyChanged(()=> ArtigoSelecionado);
                Autores = _artigoSelecionado == null
                    ? new BindingList<Autor>()
                    : new BindingList<Autor>(_artigoSelecionado.Autores.Select(x => x.Autor).ToList());
            }
        }

        public BindingList<Artigo> Artigos
        {
            get { return _artigos; }
            set { _artigos = value; RaisePropertyChanged(()=>Artigos); }
        }

        public string Edicao
        {
            get { return _edicao; }
            set { _edicao = value; RaisePropertyChanged(()=>Edicao); }
        }

        public string Titulo
        {
            get { return _titulo; }
            set { _titulo = value; RaisePropertyChanged(()=> Titulo); }
        }

        public string Autor
        {
            get { return _autor; }
            set { _autor = value; RaisePropertyChanged(()=>Autor); }
        }

        
    }
}