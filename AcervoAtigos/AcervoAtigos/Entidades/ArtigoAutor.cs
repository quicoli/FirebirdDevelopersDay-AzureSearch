using System.ComponentModel;

namespace AcervoAtigos.Entidades
{
    public class ArtigoAutor: Entidade<ArtigoAutor>
    {
        public virtual Autor Autor { get; set; }
        public virtual Artigo Artigo { get; set; }
    }
}