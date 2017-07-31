using System.Collections;
using System.Collections.Generic;

namespace AcervoAtigos.Entidades
{
    public class Artigo: Entidade<Artigo>
    {
        public virtual int Edicao { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string Arquivo { get; set; }
        public virtual IList<ArtigoAutor> Autores { get; set; }
        public Artigo()
        {
            Autores = new List<ArtigoAutor>();
        }
    }
}