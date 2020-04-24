using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShelf.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookGenre> BookGenres { get; set; }
    }
}
