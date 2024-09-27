using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ValueJetImport.Model;

namespace ValueJetImport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelProcessingController : ControllerBase
    {
        // POST api/excelprocessing/process
        [HttpPost("process")]
        public IActionResult ProcessTickets([FromBody] List<TicketDetail> ticketDetails)
        {
            if (ticketDetails == null || !ticketDetails.Any())
            {
                return BadRequest("No data provided");
            }

            // Group by Origin and Operated Flight Date
            var groupedTickets = ticketDetails
                .GroupBy(t => new { t.Origin, t.OperatedFlightDate })
                .Select(g => new ProcessedResult
                {
                    Origin = g.Key.Origin,
                    FlightDate = g.Key.OperatedFlightDate,
                    SumOfAmount = g.Sum(t => t.Amount),
                    SumOfYQ = g.Sum(t => t.YQ),
                    DistinctCountOfTktnbr = g.Select(t => t.Tktnbr).Distinct().Count(),
                    NG = (g.Sum(t => t.Amount) + g.Sum(t => t.YQ)) * 0.05M,
                    QTRate = GetQTRate(g.Key.Origin),  // Calculate QT RATE based on origin
                    QT = g.Select(t => t.Tktnbr).Distinct().Count() * GetQTRate(g.Key.Origin)  // QT Calculation
                })
                .OrderBy(g => g.Origin)
                .ToList();

            return Ok(groupedTickets);
        }

        private decimal GetQTRate(string origin)
        {
            // Define QT Rate based on origin
            switch (origin)
            {
                case "ABB":
                    return 4000M;
                case "ABV":
                case "PHC":
                case "BNI":
                    return 2000M;
                case "LOS":
                    return 4000M;
                default:
                    return 0M;
            }
        }
    }
}
