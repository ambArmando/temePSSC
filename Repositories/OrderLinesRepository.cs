using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab1PSSC.Domain;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;
using LanguageExt;
using static lab1PSSC.Domain.CartItems;
using lab1PSSCAmbrusArmando.Models;
namespace lab1PSSCAmbrusArmando.Repositories
{
    public class OrderLinesRepository
    {
        private readonly ProductContext itemContext;

        public OrderLinesRepository(ProductContext itemContext)
        {
            this.itemContext = itemContext;
        }

        public TryAsync<List<ItemFinalPrice>> TryGetExistingOrderLines() => async () => (await (
                      from l in itemContext.OrderLine
                      join p in itemContext.Product on l.OrderId equals p.ProductId
                      join h in itemContext.OrderHeader on l.OrderId equals h.OrderId
                      select new { h.OrderId, p.Code, l.Quantity, h.Address, l.Price, h.Total }).AsNoTracking().ToListAsync())
                      .Select(result => new ItemFinalPrice(
                            ItemRegistrationNumber: new(result.Code),
                            itemq: new(result.Quantity),
                            address: new(result.Address),
                            payment: new(result.Price),
                            finalPrice: new(result.Total))
                      {
                          ItemId = result.OrderId
                      }).ToList();

        public TryAsync<Unit> TrySaveOrderLines(PaidCartItems cartItems) => async () =>
        {
            var products = (await itemContext.Product.ToListAsync()).ToLookup(product => product.Code);
            var orders = (await itemContext.OrderHeader.ToListAsync()).ToLookup(order => order.OrderId);
            var newCartItems = cartItems.ItemList
                                    .Where(fp => fp.IsUpdated && fp.ItemId == 0)
                                    .Select(fp => new OrderLineDto()
                                    {
                                        ProductId = products[fp.ItemRegistrationNumber.Value].Single().ProductId,
                                        OrderId = orders[fp.ItemId].Single().OrderId,
                                        Quantity = fp.itemq.Value,
                                        Price = fp.payment.intValue
                                    });

            var updatedcartItems = cartItems.ItemList.Where(fp => fp.IsUpdated && fp.ItemId > 0)
                                    .Select(fp => new OrderLineDto()
                                    {
                                        OrderId = fp.ItemId,
                                        ProductId = products[fp.ItemRegistrationNumber.Value].Single().ProductId,
                                        Quantity = fp.itemq.Value,
                                        Price = fp.payment.intValue
                                    });

            itemContext.AddRange(newCartItems);
            foreach (var entity in updatedcartItems)
            {
                itemContext.Entry(entity).State = EntityState.Modified;
            }

            await itemContext.SaveChangesAsync();

            return unit;
        };

        //public TryAsync<List<ItemFinalPrice>> TryGetExistingOrderLines() => async () => (await (
        //                from o in itemContext.Product
        //                from h in itemContext.OrderHeader
        //                join l in itemContext.OrderLine on p.ProductId equals l.ProductId
        //                select new { h.OrderId, p.Code, p.Stoc, h.Address, h.Total })
        //                .AsNoTracking()
        //                .ToListAsync())
        //                .Select(result => new ItemFinalPrice(
        //                                          ItemRegistrationNumber: new(result.Code),
        //                                          itemq: new(result.Stoc),
        //                                          address: new(result.Address),
        //                                          payment: new("y"),
        //                                          finalPrice: new(result.Total))
        //                {
        //                    ItemId = result.OrderId
        //                })
        //                .ToList();
    }
}
