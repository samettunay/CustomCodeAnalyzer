using CustomCodeAnalyzer.Descriptors;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace CustomCodeAnalyzer.EntityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ReadAllMethodAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor rule =
        ReadAllMethodDescriptor.Create();
        private static readonly string methodBeginingName = "Getir";

        public override void Initialize(AnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.ConfigureGeneratedCodeAnalysis(
                GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            context.RegisterCompilationStartAction(compilationContext =>
            {
                compilationContext.RegisterSymbolAction(syntaxNodeAnalysisContext =>
                {
                    AnalyzeOperationAction(
                        syntaxNodeAnalysisContext);
                }, SymbolKind.Method);

            });
        }

        private static void AnalyzeOperationAction(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;
            string methodName = methodSymbol.Name;

            bool isListType = methodSymbol.ReturnsVoid == false && methodSymbol.ReturnType.ToString().StartsWith("System.Collections.Generic.List<");

            var containingType = methodSymbol.ContainingType;
            while (containingType != null)
            {
                if (containingType.BaseType?.SpecialType == SpecialType.None && containingType.BaseType?.Name == "BaseEntity")
                {
                    if (methodName.Contains(methodBeginingName) && methodName.IndexOf(methodBeginingName).Equals(0) && !isListType)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            rule, methodSymbol.Locations[0], methodSymbol.Name));
                    }
                    break;
                }
                containingType = containingType.BaseType;
            }
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(rule);
    }
}
