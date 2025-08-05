using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0007SaveAsPdf : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0007SaveAsPdf);


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

        if (string.Equals(methodName, "SaveAsPdf", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.Rule0007SaveAsPdf,
                invocation.GetLocation()));
        }
    }


    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0007SaveAsPdf = new(
            id: "CC0007",
            title: "Usage of SaveAsPdf function",
            messageFormat: "For simple report download, use SaveAs syntax with usage of Streams and RecordRef (if using a Record). You can also use tsaveReportAsPDF snippet, which also takes background based on Mail Template or use function dirrectly from Report Selection.",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when there is SaveAsPdf fuction");
            //helpLinkUri: "https://some.url/CC0007");
    }
}
