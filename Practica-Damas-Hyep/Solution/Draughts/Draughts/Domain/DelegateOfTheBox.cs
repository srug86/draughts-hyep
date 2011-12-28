using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    public class DelegateOfTheBox
    {
        public delegate void BoxDelegate();
        public event BoxDelegate switchBox;

        public object changeContentsBox
        {
            set
            {
                switchBox();
            }
        }
    }
}
