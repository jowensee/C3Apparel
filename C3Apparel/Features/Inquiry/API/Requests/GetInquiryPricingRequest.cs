using System.Collections.Generic;
using C3Apparel.Data.Pricing;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Content.API.Requests;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class EuroFreightSetting
    {
        [JsonProperty("weightInKg")]
        public decimal WeightInKg { get; set; }

        [JsonProperty("marginInDecimal")]
        public decimal MarginInDecimal { get; set; }

        [JsonProperty("auFreightPerKg")]
        public decimal AuFreightPerKg { get; set; }

        [JsonProperty("nzFreightPerKg")]
        public decimal NzFreightPerKg { get; set; }

        [JsonProperty("auFreightSurcharge")]
        public decimal AuFreightSurcharge { get; set; }

        [JsonProperty("nzFreightSurcharge")]
        public decimal NzFreightSurcharge { get; set; }
    }

    public class UsFreightSetting
    {
        [JsonProperty("weightInKg")]
        public decimal WeightInKg { get; set; }

        [JsonProperty("marginInDecimal")]
        public decimal MarginInDecimal { get; set; }

        [JsonProperty("auFreightPerKg")]
        public decimal AuFreightPerKg { get; set; }

        [JsonProperty("nzFreightPerKg")]
        public decimal NzFreightPerKg { get; set; }

        [JsonProperty("auFreightSurcharge")]
        public decimal AuFreightSurcharge { get; set; }

        [JsonProperty("nzFreightSurcharge")]
        public decimal NzFreightSurcharge { get; set; }
    }

   
    public class PricingSettings
    {
        [JsonProperty("rateAuUsd")]
        public decimal RateAuUsd { get; set; }

        [JsonProperty("rateAuEuro")]
        public decimal RateAuEuro { get; set; }

        [JsonProperty("rateNzUsd")]
        public decimal RateNzUsd { get; set; }

        [JsonProperty("rateNzEuro")]
        public decimal RateNzEuro { get; set; }

        [JsonProperty("dutyAU")]
        public decimal DutyAU { get; set; }

        [JsonProperty("dutyNZ")]
        public decimal DutyNZ { get; set; }

        [JsonProperty("euroFreightSettings")]
        public List<EuroFreightSetting> EuroFreightSettings { get; set; }

        [JsonProperty("usFreightSettings")]
        public List<UsFreightSetting> UsFreightSettings { get; set; }

        public decimal GetImportDuty(string targetCurrency)
        {
            if (targetCurrency == CurrencyConstants.AUD)
            {
                return DutyAU;
            }
            
            if (targetCurrency == CurrencyConstants.NZD)
            {
                return DutyNZ;
            }

            return 0;
        }
        
        public decimal GetExchangeRateValue(string regionCode, string targetCurrency)
        {
            if (regionCode == Region.CODE_EUROPE)
            {
                if (targetCurrency == CurrencyConstants.NZD)
                {
                    return RateNzEuro;
                }

                if (targetCurrency == CurrencyConstants.AUD)
                {
                    return RateAuEuro;
                }
            }
            
            if (regionCode == Region.CODE_US)
            {
                if (targetCurrency == CurrencyConstants.NZD)
                {
                    return RateNzUsd;
                }

                if (targetCurrency == CurrencyConstants.AUD)
                {
                    return RateAuUsd;
                }
            }

            return 0;
        }
    }

public class GetInquiryPricingRequest
{
    [JsonIgnore]
    [JsonProperty("saveButtonClicked")]
    public bool SaveButtonClicked { get; set; }
    
    [JsonProperty("brandID")]
    public string BrandID { get; set; }
    
    [JsonProperty("collection")]
    public string Collection { get; set; }

    [JsonProperty("targetCurrency")]
    public string TargetCurrency { get; set; }

    [JsonProperty("pricingSettings")]
    public PricingSettings PricingSettings { get; set; }

    [JsonIgnore]
    [JsonProperty("pageNumber")]
    public int PageNumber { get; set; }

    [JsonIgnore]
    [JsonProperty("itemsPerPage")]
    public int ItemsPerPage { get; set; }
}
