using System.Collections;
using System.Text.Json;
using Core.Utilities.Results;
using FluentValidation.Results;

namespace Core.Extensions
{
    public class ErrorDetails : Result
    {

        public int StatusCode { get; set; }

        public ErrorDetails(string message) : base(false, message)
        {

        }


        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
