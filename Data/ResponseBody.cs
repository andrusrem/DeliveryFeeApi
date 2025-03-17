using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Data
{
    [ExcludeFromCodeCoverage]
    public class ResponseBody
    {
        private string? _message;
        public decimal? Total { get; set; }
        public bool Forbitten { get; set; } = false;
        public string? ErrorMessage 
        { 
            get {  return _message; }
            set
            {
                if (Forbitten == true)
                {
                    value = "Usage of selected vehicle type is forbidden";
                    _message = value;
                }
            }
        }

    }
}
