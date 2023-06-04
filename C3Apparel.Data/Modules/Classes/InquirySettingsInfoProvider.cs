
using System;
using System.Collections.Generic;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Sql;

namespace C3Apparel.Data.Modules.Classes{
    public class InquirySettingsInfoProvider :BaseRepository, IInquirySettingsInfoProvider
    {
        public InquirySettingsInfoProvider(IConfigurationService configurationService) : base(configurationService)
        {
        }
        public InquirySettingsInfo GetSettingsByGuid(Guid guid)
        {
            var sSql =
                $"SELECT TOP 1 * FROM C3_InquirySettings WHERE {nameof(InquirySettingsInfo.InquirySettingsGuid)} = '{guid.ToSafeString()}'";

            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return null;

            }

            var row = ds.Tables[0].Rows[0];

            return new InquirySettingsInfo
            {
                InquirySettingsJsonString = row[nameof(InquirySettingsInfo.InquirySettingsJsonString)].ToSafeString()
            };
        }

    }
}