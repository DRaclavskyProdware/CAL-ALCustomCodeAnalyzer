using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0005BuildInvLineBuffer2 : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0005BuildInvLineBuffer2);

    // public override void Initialize(AnalysisContext context)
    // {
    //     context.RegisterOperationAction(CheckForBuildInvLineBuffer2Usage, OperationKind.InvocationExpression);
    // }

    // private void CheckForBuildInvLineBuffer2Usage(OperationAnalysisContext ctx)
    // {
    //     //Working code, if some method exists
    //     //var invocationExpression = (IInvocationExpression)ctx.Operation;

    //     // Check if the method is called "BuildInvLineBuffer2"
    //     //if (invocationExpression.TargetMethod.Name.Equals("BuildInvLineBuffer2", StringComparison.OrdinalIgnoreCase))
    //     //{
    //     //    // If the method doesn't exist, AL compiler will throw an error, but this custom warning should still show up
    //     //    var diagnostic = Diagnostic.Create(DiagnosticDescriptors.Rule0005BuildInvLineBuffer2, invocationExpression.Syntax.GetLocation());
    //     //    ctx.ReportDiagnostic(diagnostic);
    //     //}
    // }

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(CheckForBuildInvLineBuffer2Usage, SyntaxKind.InvocationExpression);
    }

    private static void CheckForBuildInvLineBuffer2Usage(SyntaxNodeAnalysisContext ctx)
    {
        var invocation = (InvocationExpressionSyntax)ctx.Node;
        // Get the expression being invoked, e.g., PurchPostPrepmt.BuildInvLineBuffer2()
        var expr = invocation.Expression;

        // Check if the invoked method's name is BuildInvLineBuffer2
        string methodName = null;
        if (expr is MemberAccessExpressionSyntax memberAccess)
        {
            methodName = memberAccess.Name.Identifier.ValueText;
        }
        else if (expr is IdentifierNameSyntax identifier)
        {
            methodName = identifier.Identifier.ValueText;
        }

        if (string.Equals(methodName, "BuildInvLineBuffer2", StringComparison.OrdinalIgnoreCase))
        {
            ctx.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.Rule0005BuildInvLineBuffer2,
                invocation.GetLocation()));
        }
    }


    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0005BuildInvLineBuffer2 = new(
            id: "CC0005",
            title: "Usage of BuildInvLineBuffer2",
            messageFormat: "Replace with BuildInvLineBuffer",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when there is BuildInvLineBuffer2 variable",
            helpLinkUri: "https://some.url/CC0002");
    }
}
