using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0014XMLDOMMgtCU : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0014XMLDOMMgtCU);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(this.AnalyzeOldXmlVars),
         SymbolKind.GlobalVariable,
         SymbolKind.LocalVariable);
    }

    private void AnalyzeOldXmlVars(SymbolAnalysisContext ctx)
    {
        IVariableSymbol variable = (IVariableSymbol)ctx.Symbol;

        // Then, get the declaring syntax
        var declaringSyntaxReference = variable.DeclaringSyntaxReference;
        if (declaringSyntaxReference != null)
        {
            var syntaxNode = declaringSyntaxReference.GetSyntax();
            var syntaxText = syntaxNode.ToString();

            // Fallback parsing: check if text contains old type names
            if (syntaxText.Contains("XML DOM Management"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0014XMLDOMMgtCU, variable.GetLocation(), syntaxText));
            }
        }
    }

    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0014XMLDOMMgtCU = new(
            id: "CC0014",
            title: "Usage of XML DOM Management codeunit",
            messageFormat: "XML DOM Management codeunit is scoped OnPrem. Use native XML variables instead. You can use snippets tInitXMLDoc and tAddXmlValue for inspiration how to initialize XML and write simple nodes. You can also check the article link how to write XML in AL.",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when the variable is of type XML DOM Management.",
            helpLinkUri: "https://www.dvlprlife.com/2022/12/dynamics-365-business-central-create-an-xml-file-in-al/");
    }
}
