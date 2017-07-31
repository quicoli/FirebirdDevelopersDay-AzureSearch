using Microsoft.Azure.Search.Models;

namespace AcervoAtigos.Search
{
    [SerializePropertyNamesAsCamelCase]
    public class Artigos
    {
        public string Id { get; set; }
        public string Edicao { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
    }
}