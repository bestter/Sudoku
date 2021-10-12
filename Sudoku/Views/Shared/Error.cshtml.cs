using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace BestterSudoku.Views.Shared
{
    [ResponseCache(Duration= 0, Location= ResponseCacheLocation.None, NoStore= true)]
[IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ExceptionMessage { get; set; }
        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public ErrorModel()
        {
        }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var exceptionHandlerPathFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error != null)
            {
                ExceptionMessage = $"Exception catch in {nameof(ErrorModel)}.{nameof(OnGet)}";
                if (exceptionHandlerPathFeature?.Error is FileNotFoundException fileNotFoundException)
                {
                    var fileName = fileNotFoundException.FileName != null ? $"File: {fileNotFoundException.FileName}" : string.Empty;
                    ExceptionMessage += Environment.NewLine + "File error thrown" + fileName;
                }                
                if (exceptionHandlerPathFeature?.Path == "/index")
                {
                    ExceptionMessage += " from home page";
                }

                if (_logger != null)
                {
                    _logger.LogError(ExceptionMessage);
                }
                Log.Fatal(exceptionHandlerPathFeature.Error, ExceptionMessage);
            }
        }
    }
}
