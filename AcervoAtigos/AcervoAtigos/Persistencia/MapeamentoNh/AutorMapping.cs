using AcervoAtigos.Entidades;
using FluentNHibernate.Mapping;

namespace AcervoAtigos.Persistencia.MapeamentoNh
{
    public class AutorMapping: ClassMap<Autor>
    {
        public AutorMapping()
        {
            Id(x => x.Id).GeneratedBy.HiLo("50");
            Map(x => x.Nome).Length(150).Index("AUTOR_IDX_01");
            Map(x => x.Endereco).Length(250);
            Map(x => x.Coordenadas).Length(150);
        }
    }
}