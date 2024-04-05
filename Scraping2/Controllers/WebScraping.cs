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
            
            Thread.Sleep(10000);
            //await page.ClickAsync("#st-container > div > div > div > div > div > div > accelya-shipments-root > ng-component > div > div > div > div > accelya-order-list > div.orders-container > li > accelya-shipment-summary-card > div > div.row-container.container-compact.offer-summary-body.no-icon > div.row.header-row.order-status.ng-star-inserted > div.col-xs-2.col-sm-12.col-md-5.col-lg-5.header-buttons > div > span > span");
            
            await page.ClickAsync("#st-container > div > div > div > div > div > div > accelya-shipments-root > ng-component > div > div > div > div > accelya-order-list > div.orders-container > li > accelya-shipment-summary-card > div > accelya-expander-slim"); 
            var htmlSource = await page.GetContentAsync();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlSource);

            //Flight Detail's goes here
            var flightDetails = document.DocumentNode.SelectNodes("//*[@id=\"st-container\"]/div/div/div/div/div/div/accelya-shipments-root/ng-component/div/div/div/div/accelya-order-list/div[2]/li/accelya-shipment-summary-card/div/accelya-expander-slim/div/div[1]/div[1]")?.FirstOrDefault()?.ChildNodes;
            var ffirstChild = flightDetails?.FirstOrDefault()?.ChildNodes[0];
            Console.WriteLine("new line");

            var destinationChild = ffirstChild?.SelectNodes("//span").Where(x => x.HasClass("simple-journey-airport"));
            detail.Origin = destinationChild?.FirstOrDefault()?.ChildNodes[0]?.InnerText ?? "";
            detail.Destination = destinationChild?.LastOrDefault()?.InnerText ?? "";

            var volume = flightDetails?.FirstOrDefault()?.SelectNodes("//span").Where(x => x.HasClass(""));

            var details = document.DocumentNode.SelectNodes("//*[@id=\"st-container\"]/div/div/div/div/div/div/accelya-shipments-root/ng-component/div/div/div/div/accelya-order-list/div[2]/li/accelya-shipment-summary-card/div/accelya-expander-slim/div/div[1]/div[1]/div[1]")?.FirstOrDefault()?.ChildNodes;
            string[] fdetails;
            foreach (var item in details)
            {
                var firstChild = item.FirstChild;

                //if (firstChild != null)
                //{
                //    // Process the first child of the current item
                //    fdetails.Append(firstChild.InnerText);
                //}
                //else
                //{
                //    Console.WriteLine("No first child found for current item.");
                //}
            }


            //var details2 = details1.


            Console.WriteLine("new line");

            //Flight Date Here
            //var fDate = document.DocumentNode.SelectNodes("");
            //detail.Date = fDate?.FirstOrDefault()?.InnerText ?? "";
            //Console.WriteLine("new line");


            //Flight Number Here
            //var flightNumber = document.DocumentNode.SelectNodes("//*[@id=\"st-container\"]/div/div/div/div/div/div/accelya-shipments-root/ng-component/div/div/div/div/accelya-spinner/div/accelya-offer-card/div[1]/div/div/div[2]/mcf-journey/div/div/div[1]/div[2]/div/h3").FirstOrDefault()?.ChildNodes;


            Console.WriteLine("new line");
        }
        
    }
}
