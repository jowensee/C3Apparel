using System.Collections.Generic;

namespace BlankSiteCore.Features.PriceList;

public interface IPriceListFileService
{
    //List<string> GetPriceListFiles();
    string GetPriceListFileName(string brandCodeName, string currency, string fileType);
    string GetPriceListFile(string brandCodeName, string currency, string fileType);

    string GeneratePDFFile(int brandId, string brandCodeName, string currency);
    string GenerateCSVFile(int brandId, string brandCodeName, string currency);
}