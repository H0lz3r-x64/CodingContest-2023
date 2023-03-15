using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.operations
{
    public interface IInfoSnapshot
    {
        public long TotalShipmentCount { get; }

        public double TotalSizeByDistance { get;}

        public double TotalDistance { get; }

        public double TotalSize { get; }

        public int UnfinishedOrderLineCount { get; }

        public double UnfinishedOrderLinesCost { get; }
        
        public double ShipmentsBaseCost { get; }

        public double ShipmentsSizeDistCost { get;}
        
        public double ShipmentsCosts { get; }

        public double TotalCost { get; }

    }
}
