using System;
using System.Collections.Generic;
using System.Text;

namespace MyCensus.Services.Interfaces
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetworkConnection();
    }
}
