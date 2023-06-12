
using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Sql;
using C3Apparel.Data.Utilities;

namespace C3Apparel.Data.Modules.Classes
{
    public class PriceSettingsInfoProvider : BaseRepository, IPriceSettingsInfoProvider
    {
        public PriceSettingsInfoProvider(IConfigurationService configurationService) : base(configurationService)
        {
        }
        public IEnumerable<PriceSettingsInfo> Get()
        {
            var sSql = "SELECT * FROM C3_PriceSettings";

            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return Enumerable.Empty<PriceSettingsInfo>();
            }

            var results = new List<PriceSettingsInfo>();

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var row = ds.Tables[0].Rows[i];
                
                results.Add(new PriceSettingsInfo
                {
                   PriceSettingsName = row[nameof(PriceSettingsInfo.PriceSettingsName)].ToSafeString(),
                   PriceSettingsCodeName = row[nameof(PriceSettingsInfo.PriceSettingsCodeName)].ToSafeString(),
                   ColumnHeaderText1 = row[nameof(PriceSettingsInfo.ColumnHeaderText1)].ToSafeString(),
                   ColumnHeaderText2 = row[nameof(PriceSettingsInfo.ColumnHeaderText2)].ToSafeString(),
                   C3MarginPercent = row[nameof(PriceSettingsInfo.C3MarginPercent)].ToDecimal(),
                   AUFreightSurcharge = row[nameof(PriceSettingsInfo.AUFreightSurcharge)].ToDecimal(),
                   NZFreightSurcharge = row[nameof(PriceSettingsInfo.NZFreightSurcharge)].ToDecimal(),
                   AUFreightPerKg = row[nameof(PriceSettingsInfo.AUFreightPerKg)].ToDecimal(),
                   NZFreightPerKg = row[nameof(PriceSettingsInfo.NZFreightPerKg)].ToDecimal(),
                   Weight = row[nameof(PriceSettingsInfo.Weight)].ToDecimal(),
                   
                   
                });
            }

            return results;
        }

        public void Update(PriceSettingsInfo priceSettings)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@C3MarginPercent", priceSettings.C3MarginPercent },
                { "@AUFreightSurcharge", priceSettings.AUFreightSurcharge },
                { "@NZFreightSurcharge", priceSettings.NZFreightSurcharge },
                { "@AUFreightPerKg", priceSettings.AUFreightPerKg },
                { "@NZFreightPerKg", priceSettings.NZFreightPerKg },
                { "@Weight", priceSettings.Weight },
                { "@ColumnHeaderText1", priceSettings.ColumnHeaderText1 },
                { "@ColumnHeaderText2", priceSettings.ColumnHeaderText2 },
                { "@PriceSettingsCodeName", priceSettings.PriceSettingsCodeName }
            };
            
            var sSql = 
                $@"UPDATE C3_PriceSettings SET C3MarginPercent = @C3MarginPercent, AUFreightSurcharge = @AUFreightSurcharge,
                NZFreightSurcharge = @NZFreightSurcharge, AUFreightPerKg=@AUFreightPerKg, NZFreightPerKg=@NZFreightPerKg,
                Weight = @Weight, ColumnHeaderText1=@ColumnHeaderText1, ColumnHeaderText2=@ColumnHeaderText2
                WHERE PriceSettingsCodeName = @PriceSettingsCodeName";
            
            ExecuteCommand(sSql, parameters);

        }
    }
}