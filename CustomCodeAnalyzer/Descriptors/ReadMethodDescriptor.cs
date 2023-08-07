using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomCodeAnalyzer.Descriptors
{
    public static class ReadMethodDescriptor
    {
        public static DiagnosticDescriptor Create()
        {
            return new DiagnosticDescriptor(ReadMethodDescriptor.Id, ReadMethodDescriptor.Title,
                ReadMethodDescriptor.Message, DescriptorConstants.Usage, DiagnosticSeverity.Error, true);
        }

        public const string Id = "RM001";
        public const string Message = "BaseEntity içeren sınıfların Oku Metotları '{0}' List döndermemelidir.";
        public const string Title = "List Koleksiyonu Tespit edildi.";
    }
}
