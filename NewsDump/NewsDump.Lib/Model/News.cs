using NewsDump.Lib.Data;
using System;

namespace NewsDump.Lib.Model
{
    public class News : BaseModel
    {
        public string Link { get; set; }
        public string NewsTitle { get; set; }
        public string NewsIntro { get; set; }
        public string NewsBody { get; set; }
        public DateTime PublishDate { get; set; }
        public string SiteName { get; set; }

        public override string ToString() => this.NewsTitle;
    }
}
