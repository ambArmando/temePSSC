using Microsoft.AspNetCore.Mvc;
using lab1PSSCAmbrusArmando.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using lab1PSSCAmbrusArmando.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using static lab1PSSC.PaidItemWorkflow;
using lab1PSSC;
using L05.Models;
using lab1PSSC.Domain;

namespace L05.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    public class CartController : ControllerBase
    {
        private ILogger<CartController> logger;

        public CartController(ILogger<CartController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems([FromServices] OrderLinesRepository orderLinesRepository) =>
            await orderLinesRepository.TryGetExistingOrderLines().Match(
                    Succ: GetAllItemsHandleSuccess,
                    Fail: GetAllItemsHandleError
                );

        private OkObjectResult GetAllItemsHandleSuccess(List<lab1PSSC.Domain.ItemFinalPrice> items) =>
            Ok(items.Select(item => new
            {
                ItemRegistrationNumber = item.ItemRegistrationNumber.Value,
                item.itemq,
                item.address,
                item.payment,
                item.finalPrice
            }));

        private ObjectResult GetAllItemsHandleError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return base.StatusCode(StatusCodes.Status500InternalServerError, "UnexpectedError");
        }

        [HttpPost]
        public async Task<IActionResult> PublishItems([FromServices] PaidItemWorkflow paidItemWorkflow, [FromBody]InputItem[] items)
        {
            var unvalidatedItems = items.Select(MapInputItemToUnvalidatedItem).ToList().AsReadOnly();
            PayItemsCommand command = new PayItemsCommand(unvalidatedItems);
            var result = await paidItemWorkflow.ExecuteAsync(command);
            return result.Match<IActionResult>(
                whenCartItemsFailedPayEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                whenCartItemsSucceededPayEvent: successEvent => Ok()
                );
        }

        private static UnvalidatedCustomerItem MapInputItemToUnvalidatedItem(InputItem item) => new UnvalidatedCustomerItem(
                itemCode: item.RegistrationNumber,
                itemQuantity: item.Quantity,
                address: item.Address,
                paid: item.Price
            );

    }
}