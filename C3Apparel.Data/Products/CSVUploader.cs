using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Sql;
using C3Apparel.Data.Utilities;
using CsvHelper;
using Newtonsoft.Json;

namespace C3Apparel.Data.Products
{
    public class CSVUploader
    {
        private Stream _stream;
        private string _fileName;
        private readonly IConfigurationService _configurationService;
        public CSVUploader( IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void Set(string fileName, Stream stream)
        {
            
            _stream = stream;
            _fileName = fileName;
        }

        public List<CSVProductItem> RetrievePricingsFromCSV()
        {
            using (var reader = new StreamReader(_stream))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<CSVProductItem>().ToList();
            }
        }

        public (bool success, string message) SavePricings(int brandID, List<CSVProductItem> pricings, bool deleteAll)
        {
            
            var logger = new UploadLogService(_fileName);

            var brandProvider = new BrandInfoProvider(_configurationService);
            
            var brandInfo = brandProvider.GetBrand(brandID);

            if (brandInfo == null)
            {
                logger.LogError($"Brand Id {brandID} not found.");
                return (false, $"Brand Id {brandID} not found.");
            }

            try
            {
                if (deleteAll)
                {
                    //TODO delete all
                    //ConnectionHelper.ExecuteNonQuery($"DELETE FROM C3_ProductPricing WHERE {nameof(ProductPricingInfo.ProductPricingSupplierID)}={brandID}", null,
                    //    QueryTypeEnum.SQLQuery);
                }

                var lineNumber = 0;
                var linesNotProcessed = 0;
                foreach (var pricing in pricings)
                {
                    lineNumber++;

                    if (!IsItemValid(pricing))
                    {
                        logger.LogInformation($"Item not valid", JsonConvert.SerializeObject(pricing) ,lineNumber);
                        continue;
                    }
                    ProductPricingInfo product = null;
                    
                    var isNew = true;
                    if (pricing.Action == "Add")
                    {
                        if (!deleteAll)
                        {
                            var productProvider = new ProductPricingInfoProvider(_configurationService);

                            product = productProvider.GetProductPricingByC3Style(brandID, pricing.C3Style);

                            if (product != null)
                            {
                                linesNotProcessed++;
                                logger.LogInformation($"Add action: {pricing.C3Style} in brand {brandInfo.BrandDisplayName} already exists.", 
                                    JsonConvert.SerializeObject(pricing), lineNumber);
                                continue;
                            }
                        }
                        
                        product = new ProductPricingInfo
                        {
                            ProductPricingGuid = Guid.NewGuid(),
                            ProductPricingLastModified = DateTime.Now,
                            ProductPricingStatus = pricing.Status,
                            ProductPricingSupplierID  = brandID,
                            ProducPricingDescription = pricing.Description,
                            ProductPricingCollection = pricing.Collection,
                            ProductPricingC3Style = pricing.C3Style,
                            ProductPricingSupplierStyle = pricing.SupplierStyle,
                            ProductPricingCoo = pricing.COO,
                            ProductPricingGroup = pricing.ProductGroups,
                            ProductPricingSizes = pricing.Sizes,
                            ProductPricingColours = pricing.Colours,
                            ProductPricingColourDesc = pricing.ColourDescription,
                            ProductPricingC3BuyPrice = pricing.C3BuyPrice,
                            ProductPricingSKUWeight = pricing.SKUWeight,
                            ProductPricingC3OverrideWeight = pricing.C3OverrideWeight
                        };    
                        
                        if (isNew)
                        {
                            //TODO Insert product
                            //product.Insert();   
                        }
                        
                    }else if (pricing.Action == "Update")
                    {
                        var productProvider = new ProductPricingInfoProvider(_configurationService);

                        product = productProvider.GetProductPricingByC3Style(brandID, pricing.C3Style);

                        if (product != null)
                        {
                   
                            product.ProductPricingLastModified = DateTime.Now;
                            product.ProductPricingStatus = pricing.Status;
                            product.ProducPricingDescription = pricing.Description;
                            product.ProductPricingCollection = pricing.Collection;
                            product.ProductPricingC3Style = pricing.C3Style;
                            product.ProductPricingCoo = pricing.COO;
                            product.ProductPricingGroup = pricing.ProductGroups;
                            product.ProductPricingSizes = pricing.Sizes;
                            product.ProductPricingColours = pricing.Colours;
                            product.ProductPricingColourDesc = pricing.ColourDescription;
                            product.ProductPricingC3BuyPrice = pricing.C3BuyPrice;
                            product.ProductPricingSKUWeight = pricing.SKUWeight;
                            product.ProductPricingC3OverrideWeight = pricing.C3OverrideWeight;
                            
                            //TODO: Update
                            //product.Update();
                            
                        }
                        else
                        {
                            linesNotProcessed++;
                            logger.LogInformation($"Update action: {pricing.C3Style} in brand {brandInfo.BrandDisplayName} Not found", 
                                JsonConvert.SerializeObject(pricing), lineNumber);
                        }

                    }else if (pricing.Action == "Delete")
                    {
                        //TODO Delete
                        //ConnectionHelper.ExecuteNonQuery($@"DELETE FROM C3_ProductPricing WHERE {nameof(ProductPricingInfo.ProductPricingSupplierID)}={brandID}
                         //                                           AND {nameof(ProductPricingInfo.ProductPricingC3Style)}='{pricing.C3Style.Replace("'", "''")}'", null,
                         //   QueryTypeEnum.SQLQuery);
                    }
                 
                }
                
                logger.LogInformation($"Upload complete. {lineNumber} total lines read. {linesNotProcessed} lines not processed.");
                return (true, $"Upload complete. Total item read: {lineNumber}. Lines not processed: {linesNotProcessed}");
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace);
                
                return (false, $"{ex.Message}");
            }

            
        }

        private bool IsItemValid(CSVProductItem pricing)
        {
            return 
                   !string.IsNullOrWhiteSpace(pricing.Action) && 
                   !string.IsNullOrWhiteSpace(pricing.Description) && 
                   !string.IsNullOrWhiteSpace(pricing.Collection) && 
                   !string.IsNullOrWhiteSpace(pricing.Colours) && 
                   !string.IsNullOrWhiteSpace(pricing.Sizes) && 
                   !string.IsNullOrWhiteSpace(pricing.C3Style) && 
                   !string.IsNullOrWhiteSpace(pricing.ColourDescription) && 
                   !string.IsNullOrWhiteSpace(pricing.SupplierStyle) && 
                   pricing.C3BuyPrice > 0 &&
                   pricing.SKUWeight > 0 &&
                   pricing.C3OverrideWeight >0;
        }
    }
}