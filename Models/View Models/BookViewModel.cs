using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShelf.Models.View_Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public List<int> GenreIds { get; set; }
        public List<SelectListItem> Genres { get; set; }
    }
}
