namespace AcervoAtigos.Entidades
{
    public class Autor: Entidade<Autor>
    {
        public virtual string Nome { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string Coordenadas { get; set; }
      
    }
}