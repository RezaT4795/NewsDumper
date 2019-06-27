using NewsDump.Lib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Lib.Operations.Sites.Interface
{
    interface IDumper
    {
        void RunAndSave();
        News ExtractNews(string html);
    }
}
