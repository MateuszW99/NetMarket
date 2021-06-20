namespace Domain.Enums
{
    public enum TransactionStatus
    {
        Started = 0,
        AwaitingSender,
        EnRouteFromSeller,
        Arrived,
        Checked,
        PayoutSend,
        EnRouteFromWarehouse,
        Delivered,
    }
}