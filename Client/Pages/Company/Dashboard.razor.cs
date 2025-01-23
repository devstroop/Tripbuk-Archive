using System.Globalization;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace Tripbuk.Client.Pages.Company
{
    public partial class Dashboard
    {
        [Inject] protected IJSRuntime JsRuntime { get; set; }

        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Inject] protected DialogService DialogService { get; set; }

        [Inject] protected TooltipService TooltipService { get; set; }

        [Inject] protected ContextMenuService ContextMenuService { get; set; }

        [Inject] protected NotificationService NotificationService { get; set; }

        [Inject] protected SecurityService Security { get; set; }

        void OnSeriesClick(SeriesClickEventArgs args)
        {
            NotificationService.Notify(new NotificationMessage()
                { Severity = NotificationSeverity.Info, Summary = "Series Click", Detail = $"Title: {args.Title}" });
        }

        class MastersByType
        {
            public string Type { get; set; }
            public double Count { get; set; }
        }

        private readonly MastersByType[] _accounts =
        [
            new MastersByType
            {
                Type = "Accounts",
                Count = 10
            },
            new MastersByType
            {
                Type = "Items",
                Count = 20
            },
            new MastersByType
            {
                Type = "Units",
                Count = 30
            },
            new MastersByType
            {
                Type = "Tax Slabs",
                Count = 40
            },
            new MastersByType
            {
                Type = "Miscellaneous",
                Count = 5
            },
        ];

        /// <summary>
        /// DataItem class
        /// </summary>
        class DataItem1
        {
            public string Date { get; set; }
            public double Revenue { get; set; }
        }

        string FormatAsUSD(object value)
        {
            return ((double)value).ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
        }

        DataItem1[] revenue2023 = new DataItem1[]
        {
            new DataItem1
            {
                Date = "Jan",
                Revenue = 234000
            },
            new DataItem1
            {
                Date = "Feb",
                Revenue = 269000
            },
            new DataItem1
            {
                Date = "Mar",
                Revenue = 233000
            },
            new DataItem1
            {
                Date = "Apr",
                Revenue = 244000
            },
            new DataItem1
            {
                Date = "May",
                Revenue = 214000
            },
            new DataItem1
            {
                Date = "Jun",
                Revenue = 253000
            },
            new DataItem1
            {
                Date = "Jul",
                Revenue = 274000
            },
            new DataItem1
            {
                Date = "Aug",
                Revenue = 284000
            },
            new DataItem1
            {
                Date = "Sept",
                Revenue = 273000
            },
            new DataItem1
            {
                Date = "Oct",
                Revenue = 282000
            },
            new DataItem1
            {
                Date = "Nov",
                Revenue = 289000
            },
            new DataItem1
            {
                Date = "Dec",
                Revenue = 294000
            }
        };

        DataItem1[] revenue2024 = new DataItem1[]
        {
            new DataItem1
            {
                Date = "Jan",
                Revenue = 334000
            },
            new DataItem1
            {
                Date = "Feb",
                Revenue = 369000
            },
            new DataItem1
            {
                Date = "Mar",
                Revenue = 333000
            },
            new DataItem1
            {
                Date = "Apr",
                Revenue = 344000
            },
            new DataItem1
            {
                Date = "May",
                Revenue = 314000
            },
            new DataItem1
            {
                Date = "Jun",
                Revenue = 353000
            },
            new DataItem1
            {
                Date = "Jul",
                Revenue = 374000
            },
            new DataItem1
            {
                Date = "Aug",
                Revenue = 384000
            },
            new DataItem1
            {
                Date = "Sept",
                Revenue = 373000
            },
            new DataItem1
            {
                Date = "Oct",
                Revenue = 382000
            },
            new DataItem1
            {
                Date = "Nov",
                Revenue = 389000
            },
            new DataItem1
            {
                Date = "Dec",
                Revenue = 394000
            }
        };

        /// <summary>
        /// Transaction class
        /// </summary>
        class Txn
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public double Total { get; set; }
        }

        private readonly Txn[] _txns =
        [
            new Txn
            {
                Id = "1",
                Name = "Online",
                Date = DateTime.Now,
                Total = 30000
            },
            new Txn
            {
                Id = "2",
                Name = "In-Store",
                Date = DateTime.Now,
                Total = 40000
            },
            new Txn
            {
                Id = "3",
                Name = "Mail-Order",
                Date = DateTime.Now,
                Total = 50000
            },
            new Txn
            {
                Id = "4",
                Name = "Others",
                Date = DateTime.Now,
                Total = 80000
            },
            new Txn
            {
                Id = "5",
                Name = "Online",
                Date = DateTime.Now,
                Total = 30000
            },
            new Txn
            {
                Id = "6",
                Name = "In-Store",
                Date = DateTime.Now,
                Total = 40000
            },
            new Txn
            {
                Id = "7",
                Name = "Mail-Order",
                Date = DateTime.Now,
                Total = 50000
            },
            new Txn
            {
                Id = "8",
                Name = "Others",
                Date = DateTime.Now,
                Total = 80000
            },
            new Txn
            {
                Id = "9",
                Name = "Online",
                Date = DateTime.Now,
                Total = 30000
            },
            new Txn
            {
                Id = "10",
                Name = "In-Store",
                Date = DateTime.Now,
                Total = 40000
            },
            new Txn
            {
                Id = "11",
                Name = "Mail-Order",
                Date = DateTime.Now,
                Total = 50000
            },
            new Txn
            {
                Id = "12",
                Name = "Others",
                Date = DateTime.Now,
                Total = 80000
            }
        ];
        
        class DataItem2
        {
            public string Quarter { get; set; }
            public double Revenue { get; set; }
        }

        DataItem2[] revenue = new DataItem2[] {
            new DataItem2
            {
                Quarter = "Q1",
                Revenue = 30000
            },
            new DataItem2
            {
                Quarter = "Q2",
                Revenue = 40000
            },
            new DataItem2
            {
                Quarter = "Q3",
                Revenue = 50000
            },
            new DataItem2
            {
                Quarter = "Q4",
                Revenue = 80000
            },
        };
    }
}