using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IState
    {
        void WriteState();
        void ReadState();
        void UpdateState();
        void DeleteState();
    }
}
