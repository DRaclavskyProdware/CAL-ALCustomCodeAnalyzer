using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0003FileStreamsHandling : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0003FileStreamsHandling);


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

        if (string.Equals(methodName, "Upload", StringComparison.OrdinalIgnoreCase) || string.Equals(methodName, "UploadFile", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.Rule0003FileStreamsHandling,
                invocation.GetLocation()));
        }
    }


    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0003FileStreamsHandling = new(
            id: "CC0003",
            title: "Usage of Upload functions",
            messageFormat: "Use UploadIntoStream syntax instead with usage of TempBlob and Streams",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when there is Upload fuction",
            helpLinkUri: "https://some.url/CC0002");
    }
}
