using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0013HttpWebRequestMgtCU : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0013HttpWebRequestMgtCU);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(this.AnalyzeOldHttpVars),
         SymbolKind.GlobalVariable,
         SymbolKind.LocalVariable);
    }

    private void AnalyzeOldHttpVars(SymbolAnalysisContext ctx)
    {
        IVariableSymbol variable = (IVariableSymbol)ctx.Symbol;

        // Then, get the declaring syntax
        var declaringSyntaxReference = variable.DeclaringSyntaxReference;
        if (declaringSyntaxReference != null)
        {
            var syntaxNode = declaringSyntaxReference.GetSyntax();
            var syntaxText = syntaxNode.ToString();

            //Fallback parsing: check if text contains old type names
            if (syntaxText.Contains("Http Web Request Mgt."))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0013HttpWebRequestMgtCU, variable.GetLocation(), syntaxText));
            }
        }
    }

    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0013HttpWebRequestMgtCU = new(
            id: "CC0013",
            title: "Usage of DotNet Http Variables",
            messageFormat: "Use native HttpClient and HttpRequestMessage instead of Http Web Request Mgt. (mostly OnPrem) for better performance and modern syntax. Go to link for official documentation.",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when the variable is of type old Http Dotnet.",
            helpLinkUri: "https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/developer/devenv-httpclient");
    }
}
