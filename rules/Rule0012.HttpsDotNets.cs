using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0012HttpDotNet : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0012HttpDotNet);

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
            if (syntaxText.Contains("DotNet HttpStatusCode"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0012HttpDotNet, variable.GetLocation(), syntaxText));
            }
        }
    }

    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0012HttpDotNet = new(
            id: "CC0012",
            title: "Usage of DotNet Http Variables",
            messageFormat: "Use IsSuccessStatusCode syntax from native HttpResponseMessage. To get the actual status code and phrase use HttpResponseMessage.StatusCode and HttpResponseMessage.ReasonPhrase. Use thttpGetWebService snippet for inspiration.",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when the variable is of type old Http Dotnet.");
        //helpLinkUri: "https://some.url/CC0008");
    }
}
