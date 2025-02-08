using GenericUtility.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace GenericUtility.Controllers
{
    public class ToolController : Controller
    {
        public IActionResult Base64Decoder()
        {
            return View(new Base64DecoderViewModel());
        }
        [HttpPost]
        public IActionResult Base64Decoder(Base64DecoderViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Decode the Base64 input and assign the result to the DecodedText property
                    model.DecodedText = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(model.InputText));
                }
                catch (FormatException)
                {
                    // Handle invalid Base64 format
                    ModelState.AddModelError(string.Empty, "Invalid Base64 format.");
                }
            }

            // Return the model with the decoded text (or errors)
            return View(model);
        }

        public IActionResult Base64Encoder()
        {
            return View(new Base64EncoderViewModel());
        }

        [HttpPost]
        public IActionResult Base64Encoder(Base64EncoderViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Encode the input text to Base64 and assign the result to the EncodedText property
                    model.EncodedText = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.InputText));
                }
                catch (Exception ex)
                {
                    // Handle any unexpected exceptions
                    ModelState.AddModelError(string.Empty, "An error occurred during encoding: " + ex.Message);
                }
            }

            // Return the model with the encoded text (or errors)
            return View(model);
        }


        public IActionResult ChangeCase()
        {
            return View(new ChangeCaseViewModel());
        }

        [HttpPost]
        public IActionResult ChangeCase(ChangeCaseViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CaseOption == "uppercase")
                {
                    model.ProcessedText = model.InputText.ToUpper();
                }
                else if (model.CaseOption == "lowercase")
                {
                    model.ProcessedText = model.InputText.ToLower();
                }
            }
            return View(model);

        }



        public IActionResult RemoveSpaces()
        {
            return View(new RemoveSpacesViewModel());
        }

        [HttpPost]
        public IActionResult RemoveSpaces(RemoveSpacesViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ProcessedText = model.InputText.Replace(" ", "");
            }
            return View(model);
        }



        public IActionResult JsonFormatter()
        {
            // Return an empty view model for the first load of the page
            return View(new JsonFormatterViewModel());
        }

        [HttpPost]
        public IActionResult JsonFormatter(JsonFormatterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Format the JSON and assign it to the FormattedJson property
                    model.FormattedJson = JsonConvert.SerializeObject(
                        JsonConvert.DeserializeObject(model.InputJson),
                        Formatting.Indented
                    );
                }
                catch
                {
                    // Handle invalid JSON format
                    ViewBag.Error = "Invalid JSON format.";
                    model.FormattedJson = null;  // Ensure to clear out any previous output
                }
            }

            // Return the model back to the view, with either formatted JSON or an error
            return View(model);
        }



        public IActionResult TextManipulation()
        {
            // Return an empty model for the first load of the page
            return View(new TextManipulationViewModel());
        }
        [HttpPost]
        public IActionResult TextManipulation(TextManipulationViewModel model, string action)
        {
            if (ModelState.IsValid)
            {
                switch (action)
                {
                    case "RemoveSpaces":
                        model.ManipulatedText = RemoveSpaces(model.InputText);
                        break;

                    case "ToUpperCase":
                        model.ManipulatedText = model.InputText.ToUpper();
                        break;

                    case "ToLowerCase":
                        model.ManipulatedText = model.InputText.ToLower();
                        break;

                    case "TitleCase":
                        model.ManipulatedText = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.InputText.ToLower());
                        break;

                    // Add more cases here as you add more text manipulation functions
                    default:
                        model.ManipulatedText = model.InputText;
                        break;
                }
            }

            return View(model);
        }

        private string RemoveSpaces(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : input.Replace(" ", string.Empty);
        }


        public IActionResult SqlFormatter()
        {
            // Return an empty model for the first load of the page
            return View(new SqlFormatterViewModel());
        }

        [HttpPost]
        public IActionResult SqlFormatter(SqlFormatterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Format the SQL and assign it to the FormattedSql property
                    model.FormattedSql = FormatSql(model.InputSql);
                }
                catch
                {
                    // Handle errors in SQL formatting
                    ViewBag.Error = "Invalid SQL query.";
                    model.FormattedSql = null;  // Clear any previous output in case of error
                }
            }

            // Return the model back to the view, with either formatted SQL or an error
            return View(model);
        }

        private string FormatSql(string inputSql)
        {
            // Example formatting logic (this can be extended based on your needs)
            // Basic formatting by adding newlines and indentation
            string formattedSql = inputSql;

            // Normalize spaces and newlines
            formattedSql = Regex.Replace(formattedSql, @"\s+", " ");
            formattedSql = Regex.Replace(formattedSql, @"\s*([,;()])\s*", "$1");
            formattedSql = Regex.Replace(formattedSql, @"(\bSELECT\b|\bFROM\b|\bWHERE\b|\bGROUP BY\b|\bORDER BY\b|\bINSERT INTO\b|\bVALUES\b|\bUPDATE\b|\bDELETE\b|\bJOIN\b)", "\n$1");
            formattedSql = formattedSql.Replace("(", " (\n").Replace(")", "\n)");

            return formattedSql;
        }


        public IActionResult HtmlFormatter()
        {
            return View(new HtmlFormatterViewModel());
        }

        [HttpPost]
        public IActionResult HtmlFormatter(HtmlFormatterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.FormattedHtml = FormatHtml(model.InputHtml);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error formatting HTML: {ex.Message}");
                }
            }
            return View(model);
        }

        private string FormatHtml(string inputHtml)
        {
            return inputHtml;
        }

        public IActionResult CssFormatter()
        {
            return View(new CssFormatterViewModel());
        }

        [HttpPost]
        public IActionResult CssFormatter(CssFormatterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.FormattedCss = FormatCss(model.InputCss);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error formatting CSS: {ex.Message}");
                }
            }

            return View(model);
        }

        private string FormatCss(string inputCss)
        {
            return inputCss;
        }




        public IActionResult CurrencyConverter()
        {
            return View(new CurrencyConverterViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> CurrencyConverter(CurrencyConverterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ConvertedAmount = await ConvertCurrency(model.Amount, model.FromCurrency, model.ToCurrency);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error converting currency: {ex.Message}");
                }
            }

            return View(model);
        }


        private async Task<decimal> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync($"https://api.exchangerate-api.com/v4/latest/{fromCurrency}");
                // Parse the response to get the exchange rate (example code, actual implementation may vary)
                var rate = 100;/* extract the conversion rate */;
                return amount * rate;
            }
        }


        /**
         * Displays the Unit Converter view.
         * 
         * @return View with a new instance of UnitConverterViewModel.
         */
        public IActionResult UnitConverter()
        {
            return View(new UnitConverterViewModel());
        }

        /**
         * Processes the unit conversion.
         * 
         * @param model The view model containing the input value and unit information.
         * @return View with the converted value or error messages.
         */
        [HttpPost]
        public IActionResult UnitConverter(UnitConverterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ConvertedValue = ConvertUnits(model.InputValue, model.FromUnit, model.ToUnit);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error converting units: {ex.Message}");
                }
            }

            return View(model);
        }

        /**
         * Converts the specified value from one unit to another.
         * 
         * @param inputValue The value to convert.
         * @param fromUnit The unit to convert from.
         * @param toUnit The unit to convert to.
         * @return The converted value.
         */
        private decimal ConvertUnits(decimal inputValue, string fromUnit, string toUnit)
        {
            // Implement your unit conversion logic here
            // For demonstration, returning the input value as-is
            return inputValue;
        }


        /**
 * Displays the DateTime Converter view.
 * 
 * @return View with a new instance of DateTimeConverterViewModel.
 */
        public IActionResult DateTimeConverter()
        {
            return View(new DateTimeConverterViewModel());
        }

        /**
         * Processes the DateTime conversion.
         * 
         * @param model The view model containing the input DateTime and timezone information.
         * @return View with the converted DateTime or error messages.
         */
        [HttpPost]
        public IActionResult DateTimeConverter(DateTimeConverterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ConvertedDateTime = ConvertDateTime(model.InputDateTime, model.FromTimeZone, model.ToTimeZone);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error converting DateTime: {ex.Message}");
                }
            }

            return View(model);
        }

        /**
         * Converts the specified DateTime from one timezone to another.
         * 
         * @param inputDateTime The DateTime to convert.
         * @param fromTimeZone The timezone to convert from.
         * @param toTimeZone The timezone to convert to.
         * @return The converted DateTime.
         */
        private DateTime ConvertDateTime(DateTime inputDateTime, string fromTimeZone, string toTimeZone)
        {
            var fromTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(fromTimeZone);
            var toTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(toTimeZone);
            var fromTime = TimeZoneInfo.ConvertTimeToUtc(inputDateTime, fromTimeZoneInfo);
            return TimeZoneInfo.ConvertTimeFromUtc(fromTime, toTimeZoneInfo);
        }


        /**
 * Displays the JSON Beautifier view.
 * 
 * @return View with a new instance of JsonBeautifierViewModel.
 */
        public IActionResult JsonBeautifier()
        {
            return View(new JsonBeautifierViewModel());
        }

        /**
         * Processes the JSON beautification.
         * 
         * @param model The view model containing the input JSON.
         * @return View with the beautified JSON or error messages.
         */
        [HttpPost]
        public IActionResult JsonBeautifier(JsonBeautifierViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.BeautifiedJson = JValue.Parse(model.InputJson).ToString(Formatting.Indented);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error beautifying JSON: {ex.Message}");
                }
            }

            return View(model);
        }

        /**
 * Displays the JS Beautifier view.
 * 
 * @return View with a new instance of JsBeautifierViewModel.
 */
        public IActionResult JsBeautifier()
        {
            return View(new JsBeautifierViewModel());
        }

        /**
         * Processes the JS beautification.
         * 
         * @param model The view model containing the input JS.
         * @return View with the beautified JS or error messages.
         */
        [HttpPost]
        public IActionResult JsBeautifier(JsBeautifierViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Implement JS beautification logic here
                    model.BeautifiedJs = model.InputJs; // For demonstration, returning input as-is
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error beautifying JS: {ex.Message}");
                }
            }

            return View(model);
        }
        /**
 * Displays the CSS Beautifier view.
 * 
 * @return View with a new instance of CssBeautifierViewModel.
 */
        public IActionResult CssBeautifier()
        {
            return View(new CssBeautifierViewModel());
        }

        /**
         * Processes the CSS beautification.
         * 
         * @param model The view model containing the input CSS.
         * @return View with the beautified CSS or error messages.
         */
        [HttpPost]
        public IActionResult CssBeautifier(CssBeautifierViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Implement CSS beautification logic here
                    model.BeautifiedCss = model.InputCss; // For demonstration, returning input as-is
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error beautifying CSS: {ex.Message}");
                }
            }

            return View(model);
        }
        /**
 * Displays the HTML Beautifier view.
 * 
 * @return View with a new instance of HtmlBeautifierViewModel.
 */
        public IActionResult HtmlBeautifier()
        {
            return View(new HtmlBeautifierViewModel());
        }

        /**
         * Processes the HTML beautification.
         * 
         * @param model The view model containing the input HTML.
         * @return View with the beautified HTML or error messages.
         */
        [HttpPost]
        public IActionResult HtmlBeautifier(HtmlBeautifierViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Implement HTML beautification logic here
                    model.BeautifiedHtml = model.InputHtml; // For demonstration, returning input as-is
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error beautifying HTML: {ex.Message}");
                }
            }

            return View(model);
        }


       

        public IActionResult CodeObfuscator()
        {
            return View(new CodeObfuscatorViewModel());
        }

        [HttpPost]
        public IActionResult CodeObfuscator(CodeObfuscatorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obfuscate the input code and assign the result to the ObfuscatedCode property
                    model.ObfuscatedCode = ObfuscateCode(model.InputCode);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during obfuscation
                    ModelState.AddModelError(string.Empty, $"Obfuscation error: {ex.Message}");
                }
            }

            // Return the model with the obfuscated code (or errors)
            return View(model);
        }

        private string ObfuscateCode(string inputCode)
        {
            // This is a simplified example of code obfuscation. You can implement your own obfuscation logic.
            char[] characters = inputCode.ToCharArray();
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i] = (char)(characters[i] + 1); // Shift each character by 1
            }
            return new string(characters);
        }
        public IActionResult RegexTester()
        {
            return View(new RegexTesterViewModel());
        }

        [HttpPost]
        public IActionResult RegexTester(RegexTesterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.IsMatch = System.Text.RegularExpressions.Regex.IsMatch(model.InputText, model.Pattern);
                }
                catch (Exception ex)
                {
                    model.ErrorMessage = $"Regex error: {ex.Message}";
                }
            }

            return View(model);
        }

        public IActionResult RegexReplace()
        {
            return View(new RegexReplaceViewModel());
        }

        [HttpPost]
        public IActionResult RegexReplace(RegexReplaceViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ResultText = System.Text.RegularExpressions.Regex.Replace(model.InputText, model.Pattern, model.Replacement);
                }
                catch (Exception ex)
                {
                    model.ErrorMessage = $"Regex error: {ex.Message}";
                }
            }

            return View(model);
        }

    }


}