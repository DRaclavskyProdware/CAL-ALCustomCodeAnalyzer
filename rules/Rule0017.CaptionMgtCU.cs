using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0017CaptionMgtCU : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0017CaptionMgtCU);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(this.CheckForCaptionMgt),
         SymbolKind.GlobalVariable,
         SymbolKind.LocalVariable);
    }

    private void CheckForCaptionMgt(SymbolAnalysisContext ctx)
    {
        IVariableSymbol variable = (IVariableSymbol)ctx.Symbol;

        // Then, get the declaring syntax
        var declaringSyntaxReference = variable.DeclaringSyntaxReference;
        if (declaringSyntaxReference != null)
        {
            var syntaxNode = declaringSyntaxReference.GetSyntax();
            var syntaxText = syntaxNode.ToString();

            // Fallback parsing: check if text contains old type names
            if (syntaxText.Contains("CaptionManagement"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0017CaptionMgtCU, variable.GetLocation(), syntaxText));
            }
        }
    }

    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0017CaptionMgtCU = new(
            id: "CC0017",
            title: "Usage of old CaptionManagement",
            messageFormat: "Variable '{0}' doesn't exist anymore. You can use Codeunit \"Format Document\" to get GetRecordFiltersWithCaptions function or Codeunit \"Caption Class\" for other functions/events",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when the variable is of type old CaptionManagement");
            //helpLinkUri: "https://some.url/CC0004");
    }
}
