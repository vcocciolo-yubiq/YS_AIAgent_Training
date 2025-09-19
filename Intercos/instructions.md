# Istruzioni per la Creazione di Form (YubikStudio/Intercos)

## 1. Tipi di Form
- **Bounded**: Collegate a un Workflow e a un WorkItem
- **Unbounded**: Non collegate a workflow né a WorkItem
- **SubForm**: Per righe di tabelle o oggetti business

## 2. Dove posizionare le Form
- Tutte le form vanno nella cartella `Forms` del progetto

## 3. Namespace standard da includere
```csharp
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
```
Aggiungi altri namespace se servono per i tipi usati nella form.

## 4. Struttura base di una Form
```csharp
namespace %project%.Forms
{
    public class %FormName% : Form<%WorkItem%> // oppure : Form per unbounded
    {
        // Campi
        public virtual TextField Campo1 { get; set; }
        [Unbound]
        public virtual DateField DataExtra { get; set; }
        // ...

        public override void ConfigureFields()
        {
            base.ConfigureFields();
            // Configurazioni (Required, ReadOnly, IsVisible, ecc)
        }

        public override FormPart GetLayout()
        {
            return Flat(
                Card("Campi principali",
                    Row(Col(Campo1)),
                    Row(Col(DataExtra))
                )
            );
        }

        public override void OnLoad()
        {
            base.OnLoad();
            // Logica di inizializzazione
        }
    }
}
```

## 5. Tipi di Field più usati
- TextField, MemoField, TextAreaField
- IntField, DecimalField
- DateField, DateTimeField
- ToggleField
- BoLookupField<T>, UserLookupField
- TableField<T, SubForm>
- HtmlPart, ChartPart, ButtonField

## 6. Attributi utili
- `[Unbound]` per campi non legati al WorkItem
- `[Display(Name = "Nome")]` per etichette personalizzate
- `[Required]` oppure `Campo.Required = true` in `ConfigureFields()`

## 7. Proprietà configurabili nei Field
- `Required`, `ReadOnly`, `IsVisible`, `DependsOn`, `PageSize`, `CssClass`, `OnGetOptions`, `ColumnWidth`

## 8. Layout
- Usa `Flat`, `Row`, `Col`, `Card`, `RawHtml` per organizzare i campi

## 9. Esempi di Prompt
- "Crea una Form bounded al workflow Packaging chiamata PackagingView che mostra i campi Description e QuotationId del WorkItem PackagingWI e due campi unbound: TextField Notes e DecimalField EstimatedCost"
- "Crea una Form unbounded chiamata ActionConfirm con due campi unbound: TextField ActionMsg e MemoField ActionNote"
- "Crea una SubForm<Ingredient> chiamata IngredientRow con campi Description, Material (BoLookupField<Material>), Percentage (DecimalField) e Phase (EnumField<FormulaPhase>)"

## 10. Pattern comuni
- Inizializzazione valori in `OnLoad()`
- Campi ReadOnly per visualizzazione
- TableField per tabelle
- Dipendenze tra campi con `DependsOn`
- Validazioni custom in `OnRefresh`

## 11. Template prompting
```
Crea una Form [bounded al workflow X / unbounded] chiamata [NomeForm] che [gestisce/mostra/permette di editare] i campi [lista campi] [del WorkItem XWI / unbound] [con eventuali specifiche di layout/configurazione]
```
