using AcervoAtigos.Entidades;
using FluentNHibernate.Mapping;

namespace AcervoAtigos.Persistencia.MapeamentoNh
{
    public class ArtigoAutorMapping: ClassMap<ArtigoAutor>
    {
        public ArtigoAutorMapping()
        {
            Id(x => x.Id).GeneratedBy.HiLo("50");
            References(x => x.Artigo).ForeignKey("FK_ARTIGO");
            References(x => x.Autor).ForeignKey("FK_AUTOR");
        }
    }
}