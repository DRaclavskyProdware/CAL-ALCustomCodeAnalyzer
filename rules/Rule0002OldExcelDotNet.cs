using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0002OldExcelDotnet : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0002OldExcelDotnet);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(this.AnalyzeOldExcelVars),
         SymbolKind.GlobalVariable,
         SymbolKind.LocalVariable);
    }

    private void AnalyzeOldExcelVars(SymbolAnalysisContext ctx)
    {
        IVariableSymbol variable = (IVariableSymbol)ctx.Symbol;

        var variableName = variable.Name;

        if (variableName == "XlApp" || variableName == "XlWrkBk" || variableName == "XlWrkSht" || variableName == "XlRange")
        {
            ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0002OldExcelDotnet, variable.GetLocation(), variableName));
            return;
        }

        // Then, get the declaring syntax
        var declaringSyntaxReference = variable.DeclaringSyntaxReference;
        if (declaringSyntaxReference != null)
        {
            var syntaxNode = declaringSyntaxReference.GetSyntax();
            var syntaxText = syntaxNode.ToString();

            // Fallback parsing: check if text contains old type names
            if (syntaxText.Contains("XlApp") || syntaxText.Contains("XlWrkBk") || syntaxText.Contains("XlWrkSht") || syntaxText.Contains("XlRange")
                || syntaxText.Contains("DotNet WorksheetClass") || syntaxText.Contains("DotNet WorkbookClass") || syntaxText.Contains("DotNet ApplicationClass") || syntaxText.Contains("DotNet Range"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0002OldExcelDotnet, variable.GetLocation(), syntaxText));
            }
        }
    }

    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0002OldExcelDotnet = new(
            id: "CC0002",
            title: "Usage of Automation Variables",
            messageFormat: "Variable '{0}' is old Excel DotNet variable. Rewrite with Excel Buffer and Excel Automation. You can use tenterCell and texcelSheetCode snippets to get started.",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when the variable is of type old Excel Dotnet and named XlApp, XlWrkBk, XlWrkSht, or XlRange.");
            //helpLinkUri: "https://some.url/CC0002");
    }
}
