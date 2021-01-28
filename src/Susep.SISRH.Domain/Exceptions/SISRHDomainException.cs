using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Domain.Exceptions
{
    public class SISRHDomainException : Exception
    {
        public SISRHDomainException()
        { }

        public SISRHDomainException(string message)
            : base(message)
        { }

        public SISRHDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
