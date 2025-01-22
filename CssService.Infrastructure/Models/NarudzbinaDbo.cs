namespace CssService.Infrastructure.Models
{
    public class NarudzbinaDbo
    {
        public string AcKey { get; set; }
        public string AcReceiver { get; set; }
        public string AcDocType { get; set; }
        public DateTime AdDate { get; set; }
        public DateTime AdDateValid { get; set; }
        public int? AnClerk { get; set; }
        public decimal AnValue { get; set; }
        public decimal AnForPay { get; set; }
        public string AcWarehouse { get; set; }
        public string AcStatus { get; set; }

        public NarudzbinaDbo(string acKey, string acReceiver, string acDocType, DateTime adDate, DateTime adDateValid, int anClerk, decimal anValue, decimal anForPay, string acWarehouse, string acStatus)
        {
            AcKey = acKey;
            AcReceiver = acReceiver;
            AcDocType = acDocType;
            AdDate = adDate;
            AdDateValid = adDateValid;
            AnClerk = anClerk;
            AnValue = anValue;
            AnForPay = anForPay;
            AcWarehouse = acWarehouse;
            AcStatus = acStatus;
        }
    }
}