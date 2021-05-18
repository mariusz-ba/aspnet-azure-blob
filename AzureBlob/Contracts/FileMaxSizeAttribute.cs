using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace AzureBlob.Contracts
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FileMaxSizeAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The maximum allowed size for {0} is {1}";
        
        private enum SizeUnit
        {
            Byte,
            KiloByte,
            MegaByte,
            GigaByte,
            TeraByte
        }

        private static readonly Dictionary<SizeUnit, string> Units = new()
        {
            [SizeUnit.Byte] = "B",
            [SizeUnit.KiloByte] = "KB",
            [SizeUnit.MegaByte] = "MB",
            [SizeUnit.GigaByte] = "GB",
            [SizeUnit.TeraByte] = "TB"
        };
        
        private readonly int _maxSizeInBytes;

        public FileMaxSizeAttribute(int maxSizeInBytes) : base(DefaultErrorMessage)
        {
            _maxSizeInBytes = maxSizeInBytes;
        }

        private string MaxSizeFormatted
        {
            get
            {
                var size = _maxSizeInBytes;
                var unit = 0;

                while (size >= 1024)
                {
                    size /= 1024;
                    unit++;
                }

                return Units.TryGetValue((SizeUnit) unit, out var unitName)
                    ? $"{size} {unitName}"
                    : $"{_maxSizeInBytes} {Units.GetValueOrDefault(SizeUnit.Byte)}";
            }
        }

        public override bool IsValid(object value)
            => value is null || value is IFormFile file && ValidateFileSize(file);

        public override string FormatErrorMessage(string name) =>
            string.Format(ErrorMessageString, name, MaxSizeFormatted);

        private bool ValidateFileSize(IFormFile file) => file.Length <= _maxSizeInBytes;
    }
}