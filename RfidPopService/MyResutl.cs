using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RfidPopService
{
    class MyResult
    {
        private string res;
        private System.Data.DataTable DT131;

        public MyResult(string res, System.Data.DataTable DT131)
        {
            // TODO: Complete member initialization
            this.res = res;
            this.DT131 = DT131;
        }
    }
}
