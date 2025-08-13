using Microsoft.AspNetCore.Mvc;

namespace FCamara.CommissionCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommisionController : ControllerBase
    {
        [ProducesResponseType(typeof(CommissionCalculationResponse), 200)]
        [HttpPost]
        public IActionResult Calculate(CommissionCalculationRequest calculationRequest)
        {
            if (calculationRequest == null)
            {
                return BadRequest("Invalid request data.");
            }

            if (calculationRequest.LocalSalesCount < 0 ||
            calculationRequest.ForeignSalesCount < 0 ||
            calculationRequest.AverageSaleAmount < 0)
            {
                return BadRequest("Sales counts and average sale amount must be non-negative.");
            }

            decimal fCamaraCommission = 0.2m; //20%
            decimal fCamaraCommissionForeign = 0.35m;//35%

            decimal competitorCommission = 0.02m; //2%
            decimal competitorCommissionForeign = 0.0755m;//7.55%


            return Ok(new CommissionCalculationResponse()
            {
                FCamaraCommissionAmount = FCamaraCalculeteComission(calculationRequest, fCamaraCommission, fCamaraCommissionForeign),
                CompetitorCommissionAmount = CompetitorCalculeteComission(calculationRequest, competitorCommission, competitorCommissionForeign)
            });
        }

        // Private methods to calculate fCamara commissions
        private decimal FCamaraCalculeteComission(CommissionCalculationRequest calculationRequest,
             decimal fCamaraCommission, decimal fCamaraCommissionForeign)
        {
            decimal totalSalesAmount = calculationRequest.LocalSalesCount * calculationRequest.AverageSaleAmount;
            decimal totalSalesAmountForeign = calculationRequest.ForeignSalesCount * calculationRequest.AverageSaleAmount;
            return (fCamaraCommission * totalSalesAmount)
                        + (fCamaraCommissionForeign * totalSalesAmountForeign);
        }
        // Private methods to calculate Competitor commissions 
        private decimal CompetitorCalculeteComission(CommissionCalculationRequest calculationRequest,
            decimal competitorCalculeteComission, decimal competitorCommissionForeign)
        {
            decimal totalSalesAmount = calculationRequest.LocalSalesCount * calculationRequest.AverageSaleAmount;
            decimal totalSalesAmountForeign = calculationRequest.ForeignSalesCount * calculationRequest.AverageSaleAmount;
            return (competitorCalculeteComission * totalSalesAmount)
                        + (competitorCommissionForeign * totalSalesAmountForeign);
        }
    }

    public class CommissionCalculationRequest
    {
        public int LocalSalesCount { get; set; }
        public int ForeignSalesCount { get; set; }
        public decimal AverageSaleAmount { get; set; }
    }

    public class CommissionCalculationResponse
    {
        public decimal FCamaraCommissionAmount { get; set; }

        public decimal CompetitorCommissionAmount { get; set; }
    }
}
