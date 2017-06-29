using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWriter
{
    public interface ICloser : IDisposable
    {
        void CloseTag();
    }
}
