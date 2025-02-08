namespace GenericUtility.Models
{
    public class Base64DecoderViewModel
    {
        public string InputText { get; set; } = "";
        public string? DecodedText { get; set; }
    }
    public class Base64EncoderViewModel
    {
        public string InputText { get; set; } = "";
        public string? EncodedText { get; set; }
    }
    public class RemoveSpacesViewModel
    {
        public string InputText { get; set; } = "";
        public string? ProcessedText { get; set; }
    }
    public class ChangeCaseViewModel
    {
        public string InputText { get; set; } = ""; public string? ProcessedText { get; set; }
        public string CaseOption { get; set; } = "uppercase"; // Default option
    }
    public class HtmlFormatterViewModel
    {
        public string InputHtml { get; set; } = "";
        public string? FormattedHtml { get; set; }
    }
    public class CssFormatterViewModel
    {
        public string InputCss { get; set; } = "";
        public string? FormattedCss { get; set; }
    }
    public class CurrencyConverterViewModel
    {
        public decimal Amount { get; set; }
        public string FromCurrency { get; set; } = "USD";
        public string ToCurrency { get; set; } = "EUR";
        public decimal? ConvertedAmount { get; set; }
    }
    public class UnitConverterViewModel
    {
        public decimal InputValue { get; set; }
        public string FromUnit { get; set; } = "meters";
        public string ToUnit { get; set; } = "feet";
        public decimal? ConvertedValue { get; set; }
    }
    public class DateTimeConverterViewModel
    {
        public DateTime InputDateTime { get; set; } = DateTime.Now;
        public string FromTimeZone { get; set; } = "UTC";
        public string ToTimeZone { get; set; } = "UTC";
        public DateTime? ConvertedDateTime { get; set; }
    }
    public class JsonBeautifierViewModel
    {
        public string InputJson { get; set; } = "";
        public string? BeautifiedJson { get; set; }
    }
    public class JsBeautifierViewModel
    {
        public string InputJs { get; set; } = "";
        public string? BeautifiedJs { get; set; }
    }
    public class CssBeautifierViewModel
    {
        public string InputCss { get; set; } = "";
        public string? BeautifiedCss { get; set; }
    }
    public class HtmlBeautifierViewModel
    {
        public string InputHtml { get; set; } = "";
        public string? BeautifiedHtml { get; set; }
    }
    public class JsonFormatterViewModel
    {
        public string InputJson { get; set; }
        public string FormattedJson { get; set; }
    }
    public class SqlFormatterViewModel
    {
        public string InputSql { get; set; }  // Holds the input SQL from the user
        public string FormattedSql { get; set; }  // Holds the formatted SQL output
    }
    public class TextManipulationViewModel
    {
        public string InputText { get; set; }  // Holds the input text from the user
        public string ManipulatedText { get; set; }  // Holds the result of the text manipulation
    }
    public class CodeObfuscatorViewModel
    {
        public string InputCode { get; set; } = "";
        public string? ObfuscatedCode { get; set; }
    }
    public class RegexTesterViewModel
    {
        public string InputText { get; set; } = "";
        public string Pattern { get; set; } = "";
        public bool IsMatch { get; set; }
        public string? ErrorMessage { get; set; }
    }
    public class RegexReplaceViewModel
    {
        public string InputText { get; set; } = "";
        public string Pattern { get; set; } = "";
        public string Replacement { get; set; } = "";
        public string? ResultText { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class Tutorial
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}