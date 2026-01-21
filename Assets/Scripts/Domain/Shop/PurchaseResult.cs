
public readonly struct PurchaseResult
{
    public bool Success { get; }
    public string Reason { get; }

    private PurchaseResult(bool success, string reason)
    {
        Success = success;
        Reason = reason;
    }

    public static PurchaseResult Ok() => new PurchaseResult(true, string.Empty);
    public static PurchaseResult Fail(string reason) => new PurchaseResult(false, reason);
}
