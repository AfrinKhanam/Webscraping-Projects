#pragma checksum "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0d8f55ece9dba49ca08266c1057fd221809b8945"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Dashboard_RealTimeDashboard), @"mvc.1.0.view", @"/Views/Dashboard/RealTimeDashboard.cshtml")]
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
#line 1 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/_ViewImports.cshtml"
using IndianBank_ChatBOT;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/_ViewImports.cshtml"
using IndianBank_ChatBOT.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/_ViewImports.cshtml"
using IndianBank_ChatBOT.ViewModel;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0d8f55ece9dba49ca08266c1057fd221809b8945", @"/Views/Dashboard/RealTimeDashboard.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1a7405cbc833fae39c0d58e8c9546f01e8fc5f2e", @"/Views/_ViewImports.cshtml")]
    public class Views_Dashboard_RealTimeDashboard : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<VisitorsStatisticsViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/stisla/modules/chart.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
#nullable restore
#line 3 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
  
    ViewData["Title"] = "RealTime Dashboard";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n");
#nullable restore
#line 7 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
  
    var currentMonthText = DateTime.Now.ToString("MMM");
    var currentYear = DateTime.Now.Year;
    var currentMonthYearText = currentMonthText + " " + currentYear;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "0d8f55ece9dba49ca08266c1057fd221809b89454496", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n\n");
            WriteLiteral(@"
<div class=""row"">
    <div class=""col-lg-6 col-md-6 col-sm-12"">
        <div class=""card card-statistic-2"" style=""height:90%"">
            <div class=""card-stats"">
                <div class=""card-stats-title"" style=""font-weight:bold"">
                    Number of Visits
                </div>
                <div class=""card-stats-items"">
                    <div class=""card-stats-item"">
                        <div class=""card-stats-item-count"">");
#nullable restore
#line 81 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfVisitsToday.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Today</div>\n                    </div>\n                    <div class=\"card-stats-item\">\n                        <div class=\"card-stats-item-count\">");
#nullable restore
#line 85 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfVisitsYesterday.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Yesterday</div>\n                    </div>\n                    <div class=\"card-stats-item\">\n                        <div class=\"card-stats-item-count\">");
#nullable restore
#line 89 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfVisitsCurrentMonth.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Current Month</div>\n                    </div>\n                    <div class=\"card-stats-item\">\n                        <div class=\"card-stats-item-count\">");
#nullable restore
#line 93 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfVisitsLastMonth.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Last Month</div>\n                    </div>\n                    <div class=\"card-stats-item\">\n                        <div class=\"card-stats-item-count\">");
#nullable restore
#line 97 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfVisitsCurrentYear.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Current Year</div>\n                    </div>\n                    <div class=\"card-stats-item\">\n                        <div class=\"card-stats-item-count\">");
#nullable restore
#line 101 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfVisitsLastYear.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>
                        <div class=""card-stats-item-label"">Last Year</div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class=""col-lg-6 col-md-6 col-sm-12"">
        <div class=""card card-statistic-2"" style=""height:90%"">
            <div class=""card-stats"">
                <div class=""card-stats-title"" style=""font-weight:bold"">
                    Number of Unsatisfactory Visits
                </div>
                <div class=""card-stats-items"">
                    <div class=""card-stats-item"">
                        <div class=""card-stats-item-count"">");
#nullable restore
#line 117 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfUnsatisfactoryVisitsToday.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Today</div>\n                    </div>\n                    <div class=\"card-stats-item\">\n                        <div class=\"card-stats-item-count\">");
#nullable restore
#line 121 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfUnsatisfactoryVisitsYesterday.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Yesterday</div>\n                    </div>\n                    <div class=\"card-stats-item\">\n                        <div class=\"card-stats-item-count\">");
#nullable restore
#line 125 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfUnsatisfactoryVisitsCurrentMonth.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Current Month</div>\n                    </div>\n                    <div class=\"card-stats-item\">\n                        <div class=\"card-stats-item-count\">");
#nullable restore
#line 129 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfUnsatisfactoryVisitsLastMonth.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Last Month</div>\n                    </div>\n                    <div class=\"card-stats-item\">\n                        <div class=\"card-stats-item-count\">");
#nullable restore
#line 133 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfUnsatisfactoryVisitsCurrentYear.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Current Year</div>\n                    </div>\n                    <div class=\"card-stats-item\">\n                        <div class=\"card-stats-item-count\">");
#nullable restore
#line 137 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                                      Write(Model.NumberOfUnsatisfactoryVisitsLastYear.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\n                        <div class=\"card-stats-item-label\">Last Year</div>\n                    </div>\n                </div>\n            </div>\n        </div>\n    </div>\n</div>\n\n");
            WriteLiteral("\n");
            WriteLiteral("\n<div class=\"row\">\n    <div class=\"col-lg-6 col-md-6 col-12\">\n        ");
#nullable restore
#line 268 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
   Write(await Component.InvokeAsync("DomainVisitors"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </div>\n    <div class=\"col-lg-6 col-md-6 col-12\">\n        <div class=\"card\">\n            <div class=\"card-header\">\n                <h4>Most Queried Domains - ");
#nullable restore
#line 273 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
                                      Write(currentMonthYearText);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\n            </div>\n            <div class=\"card-body\" id=\"MostQueriedDomainsByYearMonthData\">\n            </div>\n        </div>\n    </div>\n");
            WriteLiteral("</div>\n\n<div class=\"row\">\n    <div class=\"col-lg-6 col-md-6 col-12\">\n        ");
#nullable restore
#line 292 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
   Write(await Component.InvokeAsync("ChatBOTVisitorsByMonth"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n    </div>\n\n    <div class=\"col-lg-6 col-md-6 col-12\">\n        ");
#nullable restore
#line 296 "/home/ashutosh/Desktop/WorkSpace/indian_bank_afrin_important/K-BOT/chatbot/chatbot.web/Views/Dashboard/RealTimeDashboard.cshtml"
   Write(await Component.InvokeAsync("ChatBOTVisitorsByMonthYear"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
    </div>
</div>


<script type=""text/javascript"">
    ""use strict"";
    var renderMostQueriedDomainsByYearMonthReport = function () {
        $.ajax({
            url: ""/Dashboard/GetMostQueriedDomainsByYearMonth"",
            type: ""GET"",
            contentType: ""application/json; charset=utf-8"",
            dataType: ""json"",
            success: function (result) {
                if (result.length == 0) {
                    $('#MostQueriedDomainsByYearMonthData').append('<div style=""text-align:center"">No records found.</div>');
                } else {
                    var maxValue = Math.max.apply(Math, result.map(function (o) { return o.hitPercentage; }));

                    $.each(result, function (index, value) {
                        var percentage = parseInt((value.hitPercentage * 100) / maxValue);
                        var html = generateHtml(value.domainName, value.totalHits, percentage);
                        $(""#MostQueriedDomainsByYearMonthData"").append(html);
                   ");
            WriteLiteral(@" });
                }
            },
            error: function () {
                $('#MostQueriedDomainsByYearMonthData').append('<div style=""text-align:center"">No records found.</div>');
            }
        });
    };

    var renderMostQueriedDomainsByYearReport = function () {
        $.ajax({
            url: ""/Dashboard/GetMostQueriedDomainsByYear"",
            type: ""GET"",
            contentType: ""application/json; charset=utf-8"",
            dataType: ""json"",
            success: function (result) {
                if (result.length == 0) {
                    $('#MostQueriedDomainsByYearData').append('<div style=""text-align:center"">No records found.</div>');
                } else {

                    var maxValue = Math.max.apply(Math, result.map(function (o) { return o.hitPercentage; }));

                    $.each(result, function (index, value) {
                        var percentage = parseInt((value.hitPercentage * 100) / maxValue);
                        var html = generateHtml(val");
            WriteLiteral(@"ue.domainName, value.totalHits, percentage);
                        $(""#MostQueriedDomainsByYearData"").append(html);
                    });
                }
            },
            error: function () {
                $('#MostQueriedDomainsByYearData').append('<div style=""text-align:center"">No records found.</div>');
            }
        });
    };

    var generateHtml = function (domainName, totalHits, hitPercentage) {
        var hitPercentageText = hitPercentage + ""%"";
        var html = '<div class=""mb-4"">' +
            '<div class=""text-small float-right font-weight-bold text-muted"">' + totalHits + '</div>' +
            '<div class=""font-weight-bold mb-1"">' + domainName + '</div>' +
            '<div class=""progress"" data-height=""3"" style=""height: 3px;"">' +
            '<div class=""progress-bar"" role=""progressbar"" data-width=""' + hitPercentageText + ';"" aria-valuenow=""' + hitPercentage + '"" aria-valuemin=""0"" aria-valuemax=""100"" style=""width: ' + hitPercentageText + ';""></div>' +
            '</");
            WriteLiteral("div>\' +\n            \'</div > \';\n        return html;\n    };\n\n    $(document).ready(function () {\n        renderMostQueriedDomainsByYearMonthReport();\n        renderMostQueriedDomainsByYearReport();\n    });\n\n</script>\n\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<VisitorsStatisticsViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
