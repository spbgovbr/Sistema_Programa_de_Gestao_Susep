using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Application.Options
{
    public class LdapOptions
    {

        public string Url { get; set; }
        public int Port { get; set; }
        public string BindDN { get; set; }
        public string BindPassword { get; set; }
        public string SearchBaseDC { get; set; }
        public string SearchFilter { get; set; }

        public string SisrhIdAttributeFilter { get; set; }
        public string CpfAttributeFilter { get; set; }
        public string EmailAttributeFilter { get; set; }

    }
}
