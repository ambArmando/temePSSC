using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using lab1PSSC.Domain;
using Microsoft.EntityFrameworkCore;

namespace lab1PSSCAmbrusArmando.Repositories
{
    public class ProductRepository
    {
        private readonly ProductContext itemContext;

        public ProductRepository(ProductContext itemContext)
        {
            this.itemContext = itemContext;
        }

        public TryAsync<List<ItemRegistrationNumber>> TryGetExistingItems(IEnumerable<string> itemsToCheck) => async () =>
        {
            var items = await itemContext.Product
                                              .Where(item => itemsToCheck.Contains(item.Code))
                                              .AsNoTracking()
                                              .ToListAsync();
            return items.Select(item => new ItemRegistrationNumber(item.Code)).ToList();
        };
    }
}
