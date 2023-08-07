using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using CustomCodeAnalyzer.Descriptors;
using System.Collections.Immutable;
using System.Data;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace CustomCodeAnalyzer.EntityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ReadMethodAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor rule =
        ReadMethodDescriptor.Create();
        private static readonly string methodBeginingName = "Oku";

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
                    if (methodName.Contains(methodBeginingName) && methodName.IndexOf(methodBeginingName).Equals(0) && isListType)
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
