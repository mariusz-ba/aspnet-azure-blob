using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System;

namespace AzureBlob.Contracts
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FileContentTypeAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field only accepts files with following content types: {1}";

        private readonly string _contentTypes;
        
        public FileContentTypeAttribute(string contentTypes) : base(DefaultErrorMessage)
        {
            _contentTypes = contentTypes;
        }

        private string ContentTypesFormatted =>
            string.Join(", ", _contentTypes
                .Replace(" ", string.Empty)
                .Split(","));

        public override bool IsValid(object value) =>
            value is null || value is IFormFile file && ValidateContentType(file);

        public override string FormatErrorMessage(string name) =>
            string.Format(ErrorMessageString, name, ContentTypesFormatted);

        private bool ValidateContentType(IFormFile file) =>
            _contentTypes.Contains(file.ContentType.ToLower());
    }
}