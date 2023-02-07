using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCCorp.CrawlerCore.DTO
{
    public class ArticleDTO_BigData
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime Get_Time { get; set; }
        public string Get_Time_String { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Source_Id { get; set; }
        public string Image { get; set; }
        public string urlAmphtml { get; set; }
        public string ContentNoRemoveHtml { get; set; }
        public string Comment { get; set; }
        public string Author { get; set; }
        public DateTime Create_time { get; set; }
        public string Create_Time_String { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

    }
}
