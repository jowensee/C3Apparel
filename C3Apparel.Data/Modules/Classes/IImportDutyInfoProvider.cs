using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IImportDutyInfoProvider
    {
        ImportDutyInfo Get();
        void Set(ImportDutyInfo importDutyInfo);
    }
}