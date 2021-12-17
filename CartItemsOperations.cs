using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using static lab1PSSC.Domain.CartItems;

namespace lab1PSSC.Domain
{
    public static class CartItemsOperations 
    {
        public static Task<ICartItems> ValidateCartItems(Func<ItemRegistrationNumber, Option<ItemRegistrationNumber>> checkItemExists, UnvalidatedCartItems cartItems) =>
            cartItems.ItemList
                     .Select(ValidateCartItem(checkItemExists))
                     .Aggregate(CreateEmptyValidatedItemsList().ToAsync(), ReduceValidItems)
                     .MatchAsync(
                        Right: validatedItem => new ValidatedCartItems(validatedItem),
                        LeftAsync: errorMessage => Task.FromResult((ICartItems)new InvalidatedCartItems(cartItems.ItemList, errorMessage))
                      );

            private static Func<UnvalidatedCustomerItem, EitherAsync<string, ValidatedCustomerItem>> ValidateCartItem(Func<ItemRegistrationNumber, Option<ItemRegistrationNumber>> checkItemExists) =>
                unvalidatedCustomerItem => ValidateCartItem(checkItemExists, unvalidatedCustomerItem);

            private static EitherAsync<string, ValidatedCustomerItem> ValidateCartItem(Func<ItemRegistrationNumber, Option<ItemRegistrationNumber>> checkItemExists, UnvalidatedCustomerItem unvalidatedItem) =>
                from item in Item.TryParseItem(unvalidatedItem.itemQuantity).ToEitherAsync(() => $"Invalid item quantity ({unvalidatedItem.itemCode}, {unvalidatedItem.itemQuantity})")
                from itemRegistrationNumber in ItemRegistrationNumber.TryParse(unvalidatedItem.itemCode).ToEitherAsync(() => $"Invalid item registration number ({unvalidatedItem.itemCode})")
                from address in Address.TryParseAddress(unvalidatedItem.address).ToEitherAsync(() => $"Invalid address ({unvalidatedItem.itemCode}, {unvalidatedItem.address})")
                from payment in Payment.TryParsePayment(unvalidatedItem.paid).ToEitherAsync(() => $"Invalid payment ({unvalidatedItem.itemCode}, {unvalidatedItem.paid})")
                from itemExists in checkItemExists(itemRegistrationNumber).ToEitherAsync($"Item {itemRegistrationNumber.Value} does not exist")
                select new ValidatedCustomerItem(itemRegistrationNumber, item, address, payment);

            private static Either<string, List<ValidatedCustomerItem>> CreateEmptyValidatedItemsList() =>
                 Right(new List<ValidatedCustomerItem>());

            private static EitherAsync<string, List<ValidatedCustomerItem>> ReduceValidItems(EitherAsync<string, List<ValidatedCustomerItem>> acc, EitherAsync<string, ValidatedCustomerItem> next) =>
                from list in acc
                from nextItem in next
                select list.AppendValidItem(nextItem);

            private static List<ValidatedCustomerItem> AppendValidItem(this List<ValidatedCustomerItem> list, ValidatedCustomerItem validItem)
            {
                list.Add(validItem);
                return list;
            }




            //List<ValidatedCustomerItem> validatedCartItems = new();
            //bool isValidList = true;
            //string invalidReason = string.Empty;
            //foreach (var unvalidatedItem in cartItems.ItemList)
            //{
            //    if (!Item.TryParseItemQuantity(unvalidatedItem.itemQuantity, out Item itemq)) {
            //        invalidReason = $"Invalid item quantity ({unvalidatedItem.itemCode}, {unvalidatedItem.itemQuantity})";
            //        isValidList = false;
            //        break;
            //    }
            //    if (!ItemRegistrationNumber.ValidateInputRegistrationNumber(unvalidatedItem.itemCode, out ItemRegistrationNumber itemRegistration))
            //    {
            //        invalidReason = $"Invalid item registration number ({unvalidatedItem.itemCode})";
            //        isValidList = false;
            //        break;
            //    }
            //    if (!Address.ValidateInputAddress(unvalidatedItem.address, out Address address))
            //    {
            //        invalidReason = $"Invalid address ({unvalidatedItem.itemCode}, {unvalidatedItem.address})";
            //        isValidList = false;
            //        break;
            //    }
            //    if (!Payment.ValidateInputPayment(unvalidatedItem.paid, out Payment payment))
            //    {
            //        invalidReason = $"Invalid payment ({unvalidatedItem.itemCode}, {unvalidatedItem.paid})";
            //        isValidList = false;
            //        break;
            //    }
            //    ValidatedCustomerItem validCommand = new(itemRegistration, itemq, address, payment);
            //    validatedCartItems.Add(validCommand);
            //}

            //if (isValidList)
            //{
            //    return new ValidatedCartItems(validatedCartItems);
            //}
            //else
            //{
            //    return new InvalidatedCartItems(cartItems.ItemList, invalidReason);
            //}

        public static ICartItems CalculatePrice(ICartItems cartItems) => cartItems.Match(
            whenEmptyCartItems: empty => empty,
            whenUnvalidatedCartItems: unvalidated => unvalidated,
            whenInvalidatedCartItems: invalid => invalid,
            whenFailedCartItem: failed => failed,
            whenCalculatedItemPrice: calculated => calculated,
            whenPaidCartItems: paid => paid,
            whenValidatedCartItems: validatedItems =>
            {
                var calculatePrice = validatedItems.ItemList.Select(validItem =>
                    new ItemFinalPrice(validItem.ItemRegistrationNumber, validItem.item, validItem.address, validItem.paid, validItem.item + validItem.item));

                return new CalculatedItemPrice(calculatePrice.ToList());
            }
            );

        public static ICartItems PayCartItems(ICartItems cartItems) => cartItems.Match(
                whenEmptyCartItems: empty => empty,
                whenInvalidatedCartItems: invalid => invalid,
                whenUnvalidatedCartItems: unvalidated => unvalidated,
                whenValidatedCartItems: validated => validated,
                whenFailedCartItem: failed => failed,
                whenPaidCartItems: paid => paid,
                whenCalculatedItemPrice: calculatedPrice =>
                {
                    StringBuilder csv = new();
                    calculatedPrice.ItemList.Aggregate(csv, (export, price) => export.AppendLine($"Cod produs: {price.ItemRegistrationNumber.Value}, Pret final:{price.finalPrice}"));

                    PaidCartItems paidCartItems = new(calculatedPrice.ItemList, csv.ToString());

                    return paidCartItems;
                }

            );

        public static ICartItems MergeItems(ICartItems items, IEnumerable<ItemFinalPrice> existingItems) =>
            items.Match(
                whenEmptyCartItems: empty => empty,
                whenInvalidatedCartItems: invalid => invalid,
                whenUnvalidatedCartItems: unvalidated => unvalidated,
                whenValidatedCartItems: validated => validated,
                whenFailedCartItem: failed => failed,
                whenPaidCartItems: paid => paid,
                whenCalculatedItemPrice: calculatedPrice => MergeItems(calculatedPrice.ItemList, existingItems)
                );

        private static CalculatedItemPrice MergeItems(IEnumerable<ItemFinalPrice> newList, IEnumerable<ItemFinalPrice> existingList)
        {
            var updateAndNewItems = newList.Select(item => item with { ItemId = existingList.FirstOrDefault(i => i.ItemRegistrationNumber == item.ItemRegistrationNumber).ItemId, IsUpdated = true });
            var oldItems = existingList.Where(item => !newList.Any(i => i.ItemRegistrationNumber == item.ItemRegistrationNumber));
            var allItems = updateAndNewItems.Union(oldItems).ToList().AsReadOnly();

            return new CalculatedItemPrice(allItems);
        }

        public static ICartItems PublishCartItems(ICartItems cartItems) => cartItems.Match(
                whenEmptyCartItems: empty => empty,
                whenInvalidatedCartItems: invalid => invalid,
                whenUnvalidatedCartItems: unvalidated => unvalidated,
                whenValidatedCartItems: validated => validated,
                whenFailedCartItem: failed => failed,
                whenPaidCartItems: paid => paid,
                whenCalculatedItemPrice: GenerateExport
            );

        private static ICartItems GenerateExport(CalculatedItemPrice calculateItem) =>
            new PaidCartItems(calculateItem.ItemList, calculateItem.ItemList.Aggregate(new StringBuilder(), CreateCsvLine).ToString());

        private static StringBuilder CreateCsvLine(StringBuilder export, ItemFinalPrice item) =>
            export.AppendLine($"{item.ItemRegistrationNumber.Value}, {item.itemq}, {item.address}, {item.payment}, {item.finalPrice}");
    }
}
