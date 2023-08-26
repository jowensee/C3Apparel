using NReco.PdfGenerator;

namespace C3Apparel.PDF
{
    public class PDFGenerator
    {
        private readonly HtmlToPdfConverter _converter;
        public PDFGenerator()
        {
            _converter = new HtmlToPdfConverter();
            _converter.License.SetLicenseKey(
                "PDF_Generator_Bin_Examples_Pack_253554090481",
                "buSCpDvzE9rFtnJq4xox96uCcjNB75I5FvvgaiUWI9iJzplaeMHnnkpEUOmYXGV4MXkzbs+R77PbGIy4FBaLB4nFcCatlhi4tF5EZsy3N4xo2JhOv0PDMgeY+921OwwKbmUuRJEjK8GL05xgO7tLWyS32gT986YJf2jH/D3XYvQ="
            );
        }

        public byte[] GeneratePDF(string url)
        {
            _converter.Orientation = PageOrientation.Landscape;
            
            //removed header as requested
            //_converter.PageHeaderHtml = "<header style=\"text-align:center;font-family:'Segoe UI'\">C-3 Apparel</header>";
            return _converter.GeneratePdfFromFile(url,null);
            
        }
        
        
    }
}