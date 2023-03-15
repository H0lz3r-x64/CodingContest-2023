namespace com.knapp.CodingContest.operations
{
    public class CostFactors
    {
        public double UnfinishedOrderLinesPenalty => 20_000.0;

        public double UnfinishedOrderLinesCost => 2.0;

        public double ShipmentBaseCosts => 1.0;

        public double ShipmentCostsPerSizeAndDistance => 0.00083;
    }
}
