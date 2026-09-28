using Application.Models;

namespace JiblaDental.Areas.Admin.Models
{
    public class NewsViewModel
    {
        public NewsDto? News { get; set; }
        public List<NewsImageDto>? NewsImages { get; set; }
    }
}
