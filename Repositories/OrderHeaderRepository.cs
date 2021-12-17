using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using lab1PSSC.Domain;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;
using static lab1PSSC.Domain.CartItems;

namespace lab1PSSCAmbrusArmando.Repositories
{
    public class OrderHeaderRepository
    {
        private readonly ProductContext dbContext;

        public OrderHeaderRepository(ProductContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<int>> TryGetExistingOrderHeaders(IEnumerable<int> itemsToCheck) => async () =>
        {
            var items = await dbContext.OrderHeader
                                              .Where(item => itemsToCheck.Contains(item.OrderId))
                                              .AsNoTracking()
                                              .ToListAsync();
            return items.Select(items => items.OrderId).ToList();
        };

        

        //public TryAsync<Unit> TrySaveItems(PaidCartItems items) => async () =>
        //{
        //    var products = (await dbContext.Product.ToListAsync()).ToLookup(product => product.Code)
        //    var newItem = items.ItemList
        //                            .Where(g => g.IsUpdated && g.GradeId == 0)
        //                            .Select(g => new Prod()
        //                            {
        //                                StudentId = students[g.StudentRegistrationNumber.Value].Single().StudentId,
        //                                Exam = g.ExamGrade.Value,
        //                                Activity = g.ActivityGrade.Value,
        //                                Final = g.FinalGrade.Value,
        //                            });
        //    var updatedGrades = grades.GradeList.Where(g => g.IsUpdated && g.GradeId > 0)
        //                            .Select(g => new GradeDto()
        //                            {
        //                                GradeId = g.GradeId,
        //                                StudentId = students[g.StudentRegistrationNumber.Value].Single().StudentId,
        //                                Exam = g.ExamGrade.Value,
        //                                Activity = g.ActivityGrade.Value,
        //                                Final = g.FinalGrade.Value,
        //                            });

        //    dbContext.AddRange(newGrades);
        //    foreach (var entity in updatedGrades)
        //    {
        //        dbContext.Entry(entity).State = EntityState.Modified;
        //    }

        //    await dbContext.SaveChangesAsync();

        //    return unit;
        //};

    }
}
