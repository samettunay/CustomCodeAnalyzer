using Microsoft.CodeAnalysis;

namespace CustomCodeAnalyzer.Descriptors
{
    public static class ReadAllMethodDescriptor
    {
        public static DiagnosticDescriptor Create()
        {
            return new DiagnosticDescriptor(ReadAllMethodDescriptor.Id, ReadAllMethodDescriptor.Title,
                ReadAllMethodDescriptor.Message, DescriptorConstants.Usage, DiagnosticSeverity.Error, true);
        }

        public const string Id = "RM002";
        public const string Message = "BaseEntity içeren sınıfların Getir Metotları '{0}' List döndermelidir.";
        public const string Title = "List Koleksiyonu Tespit edilmedi.";
    }
}
