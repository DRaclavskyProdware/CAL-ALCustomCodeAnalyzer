using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0011CreateBook : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0011CreateBook);


    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(CheckForSaveAsPdfUsage, SyntaxKind.InvocationExpression);
    }

    private static void CheckForSaveAsPdfUsage(SyntaxNodeAnalysisContext ctx)
    {
        if (ctx.Node is not InvocationExpressionSyntax invocation)
            return;

        // Try to get the symbol info for the invocation
        var symbolInfo = ctx.SemanticModel.GetSymbolInfo(invocation);
        var symbol = symbolInfo.Symbol;

        // If the symbol is missing (method doesn't exist), fallback to syntax-based detection
        string methodName = null;
        if (symbol is IMethodSymbol methodSymbol)
        {
            methodName = methodSymbol.Name;
        }
        else
        {
            var expr = invocation.Expression;
            if (expr is MemberAccessExpressionSyntax memberAccess)
            {
                methodName = memberAccess.Name.Identifier.ValueText;
            }
            else if (expr is IdentifierNameSyntax identifier)
            {
                methodName = identifier.Identifier.ValueText;
            }
        }

        if (string.Equals(methodName, "CreateBook", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.Rule0011CreateBook,
                invocation.GetLocation()));
        }
    }


    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0011CreateBook = new(
            id: "CC0011",
            title: "Usage of CreateBook function",
            messageFormat: "Replace with CreateNewBook function. Use tExcelSheetCode for inspiration how to structure the code.",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when there is CreateBook fuction");
            //helpLinkUri: "https://some.url/CC0007");
    }
}
