using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Lib.Data
{
    public abstract class BaseModel
    {
        public int? Id { get; set; }
        public override string ToString() => Id.ToString();
    }
}
