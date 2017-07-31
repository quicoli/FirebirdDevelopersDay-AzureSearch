using AcervoAtigos.Entidades;
using FluentNHibernate.Mapping;

namespace AcervoAtigos.Persistencia.MapeamentoNh
{
    public class ArtigoMapping: ClassMap<Artigo>
    {
        public ArtigoMapping()
        {
            Id(x => x.Id).GeneratedBy.HiLo("50");
            Map(x => x.Edicao).Index("ARTIGO_IDX_01");
            Map(x => x.Titulo).Length(300).Index("ARTIGO_IDX_02");
            Map(x => x.Arquivo);
            HasMany(x => x.Autores)
                .Cascade
                .AllDeleteOrphan();
        }
    }
}