#pragma checksum "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\Components\DisplayTable.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0defb1d613e531fab5bfd31b7654f2d9edaeac0c"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace TheReviewer.Website.Components
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\_Imports.razor"
using TheReviewer.Website;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\_Imports.razor"
using TheReviewer.Website.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\_Imports.razor"
using BlazorInputFile;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\Components\DisplayTable.razor"
using static CommonServices.ExcelConversionService.TablesModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\Components\DisplayTable.razor"
using Services.Common;

#line default
#line hidden
#nullable disable
    public partial class DisplayTable : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 62 "D:\GitHub\Source\repos\TheReviewer.Website\TheReviewer.Website\Components\DisplayTable.razor"
       
    TableModel tableModel = new TableModel();
    [Parameter] public TableModel tableDisplay { get; set; }
    protected override void OnParametersSet()
    {
        tableModel = tableDisplay;
    }


#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591