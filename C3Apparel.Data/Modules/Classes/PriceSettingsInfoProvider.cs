
using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Sql;

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

    }
}