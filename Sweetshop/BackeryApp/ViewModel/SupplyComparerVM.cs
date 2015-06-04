using SupplyStock.Utils;

namespace BackeryApp.ViewModel
{
    public class SupplyComparerVM
    {
        #region Constructor

        public SupplyComparerVM(string supplyName, Currency costPerUnit, double maxAmount, Currency selectedCostPerUnit, Measurement definedAmount)
        {
            Name = supplyName;
            UnitCost = costPerUnit;
            SelectedCost = selectedCostPerUnit;
            MaxCostPerUnitCUC = maxAmount;
            DefinedAmount = definedAmount;
            Cost = costPerUnit*definedAmount.AmountInUnit;
            
        }

        #endregion
        #region Properties

        public string Name { get; set; }
        public Currency UnitCost { get; set; }
        public Currency SelectedCost { get; set; }
        public double MaxCostPerUnitCUC { get; set; }
        public Currency Cost { get; set; }
        public Measurement DefinedAmount { get; set; }

        public Currency Difference { get { return (UnitCost - SelectedCost)*DefinedAmount.AmountInUnit; } }
        public double Proportion
        {
            get { return UnitCost.AmountCUC / MaxCostPerUnitCUC; }
        }


        #endregion
    }
}
