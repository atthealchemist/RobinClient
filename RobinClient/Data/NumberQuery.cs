using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobinClient
{
    public class Query
    {

        public decimal Number { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"[{Status}] {Number}";
        }

    }
}
