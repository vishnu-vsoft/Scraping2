using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using Scraping2.Models;
using System.Collections.Generic;

namespace Scraping2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class WebScraping : ControllerBase
    {
        [HttpPost]
        public async Task virginAtlanticCargo(string id)
        {
            var launchOptions = new LaunchOptions
            {
                Headless = false,
                ExecutablePath = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe"
            };

            AWBStatus awbStatus = new AWBStatus();

            List<FlightDetail> flights = new List<FlightDetail>();
            List<StatusHistory> statusHistory = new List<StatusHistory>();
            List <FlightSchedules> flightSchedules = new List<FlightSchedules>();

            FlightDetail detail = new FlightDetail();
            StatusHistory fistory = new StatusHistory();
            FlightSchedules schedules = new FlightSchedules();
            
            string OrgId = id.Replace("-", "");

            var browser = await Puppeteer.LaunchAsync(launchOptions);
            var page = await browser.NewPageAsync();
            await page.GoToAsync($"https://myvs.virginatlanticcargo.com/app/offerandorder/#/shipments/list?type=D&values={OrgId}");

            await Task.Delay(5000);
            
            await page.WaitForSelectorAsync("#st-container > div > div > div > div > div > div > accelya-shipments-root > ng-component > div > div > div > div > accelya-order-list > div.orders-container > li > accelya-shipment-summary-card > div > accelya-expander-slim");
            await page.ClickAsync("#st-container > div > div > div > div > div > div > accelya-shipments-root > ng-component > div > div > div > div > accelya-order-list > div.orders-container > li > accelya-shipment-summary-card > div > accelya-expander-slim"); 
            var htmlSource = await page.GetContentAsync();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlSource);

            //Flight Detail's goes here
            var flightDetails = document.DocumentNode.SelectNodes("//*[@id=\"st-container\"]/div/div/div/div/div/div/accelya-shipments-root/ng-component/div/div/div/div/accelya-order-list/div[2]/li/accelya-shipment-summary-card/div/accelya-expander-slim/div/div[1]/div[1]/div[1]/div[1]")?.FirstOrDefault();
            //var ffirstChild = flightDetails?.FirstOrDefault()?.ChildNodes[0];
            Console.WriteLine("new line");

            //var destinationChild = ffirstChild?.SelectNodes("//span").Where(x => x.HasClass("simple-journey-airport"));
            //detail.Origin = destinationChild?.FirstOrDefault()?.ChildNodes[0]?.InnerText ?? "";
            //detail.Destination = destinationChild?.LastOrDefault()?.InnerText ?? "";

            

            var details = document.DocumentNode.SelectNodes("//*[@id=\"flight-leg-card00\"]").SingleOrDefault();

            var pieces = details?.ChildNodes;
            //awbStatus.Pieces = pieces?.LastChild.InnerText ?? "";

            var weight = details?.ChildNodes[2];
            awbStatus.Weight = weight?.ChildNodes[2].InnerText ?? "";
            Console.WriteLine("new line");

            var volume = details?.ChildNodes[3];
            var orgVolume = volume?.ChildNodes[2].InnerText ?? "";

            var Cweight = details?.ChildNodes[5];
            var ChargeableWeigth = Cweight?.ChildNodes[3].InnerText;


            var details1 = document.DocumentNode.SelectNodes("//*[@id=\"st-container\"]/div/div/div/div/div/div/accelya-shipments-root/ng-component/div/div/div/div/accelya-order-list/div[2]/li/accelya-shipment-summary-card/div/accelya-expander-slim/div/div[1]/div[1]/div[2]")?.FirstOrDefault();
            var Code = details1?.ChildNodes[3];
            var productCod = Code?.ChildNodes[2].InnerText;

            var GoodsDescription = details1?.ChildNodes[4];
            var goodsDescription = GoodsDescription?.ChildNodes[2].InnerText;

            var shcs = details1?.ChildNodes[5];
            var SHCS = shcs?.ChildNodes[3].InnerText;

            

            var htmlSource1 = await page.GetContentAsync();
            HtmlDocument document1 = new HtmlDocument();
            document1.LoadHtml(htmlSource1);

            await page.WaitForSelectorAsync("#trackingDetails1705478528066");
            await page.ClickAsync("#trackingDetails1711015463307");
            await Task.Delay(5000);

            //Flight Date Here
            //var fDate = document.DocumentNode.SelectNodes("");
            //detail.Date = fDate?.FirstOrDefault()?.InnerText ?? "";
            //Console.WriteLine("new line");


            //Flight Number Here
            //var flightNumber = document.DocumentNode.SelectNodes("//*[@id=\"st-container\"]/div/div/div/div/div/div/accelya-shipments-root/ng-component/div/div/div/div/accelya-spinner/div/accelya-offer-card/div[1]/div/div/div[2]/mcf-journey/div/div/div[1]/div[2]/div/h3").FirstOrDefault()?.ChildNodes;

            var TrackingDetailsShipment = document1.DocumentNode.SelectSingleNode("//*[@id=\"st-container\"]/div/div/div/div/div/div/accelya-shipments-root/ng-component/div/div/div/div/accelya-spinner/div/accelya-offer-card/div[2]");
            var items = TrackingDetailsShipment?.ChildNodes?.Descendants()?.Where(x => x.Name== "accelya-spinner")?.ToList() ;

            int x = 0;
            foreach (var row in items)
            {
                if(x >= 4)
                {
                    break;
                }
                
                var xy = row.ChildNodes[x]?.Descendants("div")?.Where(div => div.GetAttributeValue("class", "") == " steps first-step last-step ng-star-inserted").ToList();//FirstChild?.LastChild?.FirstChild?.FirstChild?.LastChild?.FirstChild?.ChildNodes[1]?.FirstChild?.ChildNodes[1]?.InnerText;//

                x = x + 2;
                
            }

            Console.WriteLine("new line");
        }
        
    }
}
