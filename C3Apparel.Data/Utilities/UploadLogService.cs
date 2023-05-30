using System;
using C3Apparel.Data.CustomTables;

namespace C3Apparel.Data.Utilities
{
    public class UploadLogService
    {
        public string _fileName { get; }

        public UploadLogService(string fileName)
        {
            _fileName = fileName;
        }

        public void LogInformation(string message, string details = "", int lineNumber = 0)
        {
            var log = new PricingImportLogItem
            {
                MessageType = "Info",
                Message = lineNumber == 0 ? message : $"Line {lineNumber}: {message}",
                Details = details,
                ItemCreatedUser = "",//TODO add user
                ItemCreatedWhen = DateTime.Now,
                ItemGUID = Guid.NewGuid()
            };
            
            //TODO: add insert to db
            //log.Insert();
        }
        
        public void LogError(string message, string details = "", int lineNumber = 0)
        {
            var log = new PricingImportLogItem
            {
                MessageType = "Error",
                Message = lineNumber == 0 ? message : $"Line {lineNumber}: {message}",
                Details = details,
                ItemCreatedUser = "", //TODO add user
                ItemCreatedWhen = DateTime.Now,
                ItemGUID = Guid.NewGuid()
            };
            
            //TODO: add insert to db
            //log.Insert();
        }
    }
}