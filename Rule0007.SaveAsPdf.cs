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

        var expr = invocation.Expression;

        string methodName = null;
        if (expr is MemberAccessExpressionSyntax memberAccess)
        {
            methodName = memberAccess.Name.Identifier.ValueText;
        }
        else if (expr is IdentifierNameSyntax identifier)
        {
            methodName = identifier.Identifier.ValueText;
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
            messageFormat: "Use SaveAs syntax with usage of Streams and RecordRef (is using a Record)",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when there is SaveAsPdf fuction",
            helpLinkUri: "https://some.url/CC0002");
    }
}
