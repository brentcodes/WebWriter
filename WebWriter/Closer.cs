using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWriter
{
    public class Closer : ICloser
    {
        Action _closer;
        bool _hasBeenClosed = false;
        internal Closer(Action closer)
        {
            _closer = closer;
        }

        public void CloseTag()
        {
            if (!_hasBeenClosed)
            {
                if (_closer != null)
                    _closer();
                _hasBeenClosed = true;
            }
        }

        public void Dispose()
        {
            CloseTag();
        }
    }
}
