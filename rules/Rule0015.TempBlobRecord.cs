using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Symbols;
using System.Collections.Immutable;

namespace CustomCodeCop;

[DiagnosticAnalyzer]
public class Rule0015TempBlobRecord : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create<DiagnosticDescriptor>(DiagnosticDescriptors.Rule0015TempBlobRecord);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSymbolAction(new Action<SymbolAnalysisContext>(this.AnalyzeTempBlobRecord),
         SymbolKind.GlobalVariable,
         SymbolKind.LocalVariable);
    }

    private void AnalyzeTempBlobRecord(SymbolAnalysisContext ctx)
    {
        IVariableSymbol variable = (IVariableSymbol)ctx.Symbol;

        // Then, get the declaring syntax
        var declaringSyntaxReference = variable.DeclaringSyntaxReference;
        if (declaringSyntaxReference != null)
        {
            var syntaxNode = declaringSyntaxReference.GetSyntax();
            var syntaxText = syntaxNode.ToString();

            // Check if there is Record TempBlob but exclude Codeunit "Temp Blob" if it's also named as variable TempBlob
            if (syntaxText.Contains("Record TempBlob"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0015TempBlobRecord, variable.GetLocation(), syntaxText));
            }
            // if (string.Equals(syntaxText, "Record TempBlob", StringComparison.OrdinalIgnoreCase))
            // {
            //     ctx.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Rule0015TempBlobRecord, variable.GetLocation(), syntaxText));
            // }
        }
    }

    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor Rule0015TempBlobRecord = new(
            id: "CC0015",
            title: "Usage of TempBlob Record",
            messageFormat: "Replace with Codeunit \"Temp Blob\". Use InStream and OutStream to read/write data.",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: "Raise a diagnostic when the variable is of type TempBlob.");
    }
}
