using Microsoft.CodeAnalysis;

namespace CustomCodeAnalyzer.Descriptors
{
    public static class VirtualMethodDescriptor
    {
        public static DiagnosticDescriptor Create()
        {
            return new DiagnosticDescriptor(VirtualMethodDescriptor.Id, VirtualMethodDescriptor.Title,
                VirtualMethodDescriptor.Message, DescriptorConstants.Usage, DiagnosticSeverity.Error, true);
        }

        public const string Id = "VM001";
        public const string Message = "BaseEntity içeren sınıfların Metotları '{0}' virtual olmalıdır.";
        public const string Title = "Virtual Metot Eksik";
    }
}
