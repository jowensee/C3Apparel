using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IImportDutyInfoProvider
    {
        IEnumerable<ImportDutyInfo> Get();
    }
}