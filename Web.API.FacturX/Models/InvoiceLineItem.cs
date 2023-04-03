namespace LIB_ZUGFeRD.Models {

    public class InvoiceLineItem
    {
        public string ProductID { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public float Quantity { get; set; }

        public float Total { get; set; }

        public QuantityCodes UnitCode { get; set; }
   }
}
