using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0016FileWriteFunctions : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0016FileWriteFunctions);


    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(CheckForFileUsage, SyntaxKind.InvocationExpression);
    }

    private static void CheckForFileUsage(SyntaxNodeAnalysisContext ctx)
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

        if (string.Equals(methodName, "TextMode", StringComparison.OrdinalIgnoreCase) || string.Equals(methodName, "WriteMode", StringComparison.OrdinalIgnoreCase)
            || string.Equals(methodName, "Write", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.Rule0016FileWriteFunctions,
                invocation.GetLocation()));
        }
    }


    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0016FileWriteFunctions = new(
            id: "CC0016",
            title: "Usage of OnPrem File functions",
            messageFormat: "Use OutStream.WriteText to write text to files. Use tdownloadFileStream snippet for inspiration.",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when there is OnPrem File fuctions");
            //helpLinkUri: "https://some.url/CC0003");
    }
}
