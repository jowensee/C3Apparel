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
        public ImportDutyInfo Get()
        {
            var sSql = @"SELECT TOP 1 [ImportDutyID]
                              ,[ImportDutyGuid]
                              ,[ImportDutyLastModified]
                              ,[ImportDutyAustralia]
                              ,[ImportDutyNewZealand]
                          FROM [dbo].[C3_ImportDuty]";

            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return null;
            }

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var row = ds.Tables[0].Rows[i];
                
                return CreateImportInfo(row);
            }
            
            return null;
        }

        public void Set(ImportDutyInfo importDutyInfo)
        {
            var sSql = @"SELECT *
                          FROM [dbo].[C3_ImportDuty]";

            var parameters = new Dictionary<string, object>
            {
                { "@importDutyAU", importDutyInfo.ImportDutyAustralia },
                { "@importDutyNZ", importDutyInfo.ImportDutyNewZealand }
                
            };
            
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                sSql =
                    $@"INSERT INTO C3_ImportDuty (ImportDutyGuid, ImportDutyLastModified, ImportDutyAustralia, ImportDutyNewZealand)
                            VALUES (NEWID(), GETDATE(), @importDutyAU, @importDutyNZ)";
            }
            else
            {
                sSql =
                    $@"UPDATE C3_ImportDuty SET ImportDutyLastModified = GETDATE(), ImportDutyAustralia = @importDutyAU, ImportDutyNewZealand = @importDutyNZ";
            }
            
            ExecuteCommand(sSql, parameters);
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