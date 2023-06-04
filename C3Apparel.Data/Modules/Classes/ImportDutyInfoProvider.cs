using System.Collections.Generic;
using System.Data;
using System.Linq;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Sql;

namespace C3Apparel.Data.Modules.Classes
{

    public class ImportDutyInfoProvider : BaseRepository, IImportDutyInfoProvider
    {
        public ImportDutyInfoProvider(IConfigurationService configurationService) : base(configurationService)
        {
        }
        public IEnumerable<ImportDutyInfo> Get()
        {
            var sSql = @"SELECT [ImportDutyID]
                              ,[ImportDutyGuid]
                              ,[ImportDutyLastModified]
                              ,[ImportDutyAustralia]
                              ,[ImportDutyNewZealand]
                          FROM [dbo].[C3_ImportDuty]";

            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return Enumerable.Empty<ImportDutyInfo>();
            }

            var brands = new List<ImportDutyInfo>();
            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var row = ds.Tables[0].Rows[i];
                
                brands.Add(CreateImportInfo(row));
            }
            
            return brands;
        }
        private ImportDutyInfo CreateImportInfo(DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            return new ImportDutyInfo
            {
                ImportDutyAustralia = row[nameof(ImportDutyInfo.ImportDutyAustralia)].ToDecimal(),
                ImportDutyNewZealand = row[nameof(ImportDutyInfo.ImportDutyNewZealand)].ToDecimal()

            };
        }
    }
}