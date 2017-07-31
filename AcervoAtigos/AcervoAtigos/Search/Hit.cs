using System.Collections.Generic;
using NHibernate.Mapping;

namespace AcervoAtigos.Search
{
    public class Hit
    {
        public long Id { get; set; }
        public List<HitHighlight> Textos { get; set; }

        public Hit()
        {
            Textos = new List<HitHighlight>();
        }
    }
}