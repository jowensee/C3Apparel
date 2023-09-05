using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Features.Admin.ProductPricing.CSV;
using C3Apparel.PDF;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace C3Apparel.Features.PriceList;

public class PriceListFileService : IPriceListFileService
{
    public const string PriceListMainPath = "/price-lists";
    
    private readonly IPriceListPriceInfoProvider _priceListPriceInfoProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private List<string> _allPriceLists;
    private readonly string _priceListMainPath;
    
    
    public PriceListFileService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IPriceListPriceInfoProvider priceListPriceInfoProvider)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _priceListPriceInfoProvider = priceListPriceInfoProvider;
        _priceListMainPath = $"{_webHostEnvironment.WebRootPath}\\price-lists";
        GetPriceListFiles();
    }
    
    private void GetPriceListFiles()
    {
        if (!Directory.Exists(_priceListMainPath))
        {
            return;
        }

        _allPriceLists = Directory.GetFiles(_priceListMainPath).ToList();

    }

    public string GetPriceListFileName(string brandCodeName, string currency, string fileType)
    {
        if (fileType == PriceListConstants.FILE_TYPE_PDF)
        {
            return $"PriceList{brandCodeName}_{currency}{fileType}";
        }
        
        if (fileType == PriceListConstants.FILE_TYPE_CSV)
        {
            return $"productpricing{brandCodeName}_{currency.ToLower()}{fileType}";
        }

        return string.Empty;
    }
    
    public string GetPriceListFile(string brandCodeName, string currency, string fileType)
    {
        if (_allPriceLists.IsNullOrEmpty())
        {
            return string.Empty;
        }
        var fileName = GetPriceListFileName(brandCodeName, currency, fileType);
        var exist = _allPriceLists.Any(a => a.IndexOf(GetPriceListFileName(brandCodeName, currency, fileType),
            StringComparison.InvariantCultureIgnoreCase) > -1);

        if (exist)
        {
            return $"{PriceListMainPath}/{fileName}";
        }

        return string.Empty;

    }

    public string GeneratePDFFile(int brandId, string brandCodeName, string currency)
    {
        EnsureMainPath();
        
        var pdfGenerator = new PDFGenerator();

        var baseUrl = $"{(_httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://")}{_httpContextAccessor.HttpContext.Request.Host.Value}"; ;

        var fileName = GetPriceListFileName(brandCodeName, currency, PriceListConstants.FILE_TYPE_PDF);    
        var bytes = pdfGenerator.GeneratePDF($"{baseUrl}/print-version?brandid={brandId}&currency={currency}");

        var finalFilePath = $"{_priceListMainPath}\\{fileName}";
        
        File.WriteAllBytes(finalFilePath, bytes);

        
        return string.Empty;
    }

    public string GenerateCSVFile(int brandId, string brandCodeName, string currency)
    {
        var prices = _priceListPriceInfoProvider.GetAllPrices(1, currency, brandId);

        var priceLists = prices.Select(a => new CSVPriceListItem
        {
            Brand = a.PriceBrandName,
            Collection = a.PriceCollection,
            Colours = a.PriceColours,
            Sizes = a.PriceSizes,
            Style = a.PriceC3Style,
            Description = a.PriceDescription,
            FreightSurcharge1 = a.PriceCol1FreightSurcharge,
            MinimumOrderQty1 = a.PriceCol1MOQUnit,
            Price1 = a.PriceCol1UnitPrice,
            MinimumOrderQty2 = a.PriceCol2MOQUnit,
            Price2 = a.PriceCol2UnitPrice,
            MinimumOrderQty3 = a.PriceCol3MOQUnit,
            Price3 = a.PriceCol3UnitPrice,
            MinimumOrderQty4 = a.PriceCol4MOQUnit,
            Price4 = a.PriceCol4UnitPrice
        });
        
        var fileName = GetPriceListFileName(brandCodeName, currency, PriceListConstants.FILE_TYPE_CSV);    

        using (var streamWriter = new StreamWriter($"{_priceListMainPath}\\{fileName}"))
        {
            using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                //csvWriter.Context.RegisterClassMap<ProductItemCSVMap>();
                csvWriter.WriteRecords(priceLists);
                streamWriter.Flush();
                    
            }
        }

        return string.Empty;
    }

    private void EnsureMainPath()
    {
        if (!Directory.Exists(_priceListMainPath))
        {
            Directory.CreateDirectory(_priceListMainPath);
        }
    }
}