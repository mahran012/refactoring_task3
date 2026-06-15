using AutoServiceApp.Models;

namespace AutoServiceApp.Services;

public class OrderCostCalculator
{
    public const decimal ImmediatePartUsageMarkup = 1.50m;

    private const decimal CalculatedPartMarkup = 1.20m;
    private const decimal CardPaymentFeeRate = 0.05m;
    private const int LoyalCustomerCarCountThreshold = 2;
    private const decimal LoyalCustomerDiscountRate = 0.10m;
    private const decimal FinalReadyOrderFee = 500m;
    private const decimal LargeOrderDiscountThreshold = 10000m;
    private const decimal LargeOrderDiscountRate = 0.15m;

    public decimal Calculate(RepairOrder order, IEnumerable<Part> availableParts, bool final, string paymentMethod)
    {
        var worksCost = CalculateWorksCost(order);
        var partsCost = CalculatePartsCost(order, availableParts);
        var total = worksCost + partsCost;

        total = ApplyCardFee(total, paymentMethod);
        total = ApplyLoyalCustomerDiscount(total, order.Customer);
        total = ApplyFinalReadyOrderFee(total, order, final);
        total = ApplyLargeOrderDiscount(total);

        return total;
    }

    private static decimal CalculateWorksCost(RepairOrder order)
    {
        return order.Works.Sum(work => work.Cost + (decimal)work.Hours * (order.AssignedMechanic?.HourRate ?? 0));
    }

    private static decimal CalculatePartsCost(RepairOrder order, IEnumerable<Part> availableParts)
    {
        var partsById = availableParts.ToDictionary(part => part.Id);

        return order.UsedPartIds
            .Select(partId => partsById.GetValueOrDefault(partId))
            .Where(part => part != null)
            .Sum(part => part!.Price * CalculatedPartMarkup);
    }

    private static decimal ApplyCardFee(decimal total, string paymentMethod)
    {
        return PaymentMethod.IsCard(paymentMethod)
            ? total + total * CardPaymentFeeRate
            : total;
    }

    private static decimal ApplyLoyalCustomerDiscount(decimal total, Customer? customer)
    {
        return customer != null && customer.Cars.Count > LoyalCustomerCarCountThreshold
            ? total - total * LoyalCustomerDiscountRate
            : total;
    }

    private static decimal ApplyFinalReadyOrderFee(decimal total, RepairOrder order, bool final)
    {
        return final && OrderStatus.IsReady(order.Status)
            ? total + FinalReadyOrderFee
            : total;
    }

    private static decimal ApplyLargeOrderDiscount(decimal total)
    {
        return total > LargeOrderDiscountThreshold
            ? total - total * LargeOrderDiscountRate
            : total;
    }
}
