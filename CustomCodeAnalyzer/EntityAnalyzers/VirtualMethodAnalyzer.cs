using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using CustomCodeAnalyzer.Descriptors;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CustomCodeAnalyzer.EntityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class VirtualMethodAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor rule =
        VirtualMethodDescriptor.Create();

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

            var containingType = methodSymbol.ContainingType;
            while (containingType != null)
            {
                if (containingType.BaseType?.SpecialType == SpecialType.None && containingType.BaseType?.Name == "BaseEntity")
                {
                    if (!methodSymbol.IsVirtual)
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
