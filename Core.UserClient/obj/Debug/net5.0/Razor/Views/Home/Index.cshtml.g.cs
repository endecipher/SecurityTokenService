#pragma checksum "C:\Users\Ayan.Choudhury\source\repos\SecurityTokenService\Core.UserClient\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a1e28717b0a3bb51b27d9f6bec09c810b36e6721"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Ayan.Choudhury\source\repos\SecurityTokenService\Core.UserClient\Views\_ViewImports.cshtml"
using static Core.UserClient.Startup;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Ayan.Choudhury\source\repos\SecurityTokenService\Core.UserClient\Views\_ViewImports.cshtml"
using Core.UserClient.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Ayan.Choudhury\source\repos\SecurityTokenService\Core.UserClient\Views\_ViewImports.cshtml"
using Core.UserClient;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a1e28717b0a3bb51b27d9f6bec09c810b36e6721", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0871315033338f09b4d28f83248856d489b7a3c0", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<MessageModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Ayan.Choudhury\source\repos\SecurityTokenService\Core.UserClient\Views\Home\Index.cshtml"
 foreach (var errorMessage in Model.Errors)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <br /><span class=\"alert-danger\">");
#nullable restore
#line 5 "C:\Users\Ayan.Choudhury\source\repos\SecurityTokenService\Core.UserClient\Views\Home\Index.cshtml"
                                Write(errorMessage);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 6 "C:\Users\Ayan.Choudhury\source\repos\SecurityTokenService\Core.UserClient\Views\Home\Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"panel-body\">\r\n    ");
#nullable restore
#line 8 "C:\Users\Ayan.Choudhury\source\repos\SecurityTokenService\Core.UserClient\Views\Home\Index.cshtml"
Write(Model.Message);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<MessageModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
