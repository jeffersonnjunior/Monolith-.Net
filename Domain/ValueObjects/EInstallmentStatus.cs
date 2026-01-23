namespace Domain.ValueObjects;

public class InstallmentStatus
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    private InstallmentStatus(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static readonly InstallmentStatus Pending = new(1, nameof(Pending));
    public static readonly InstallmentStatus Paid = new(2, nameof(Paid));
    public static readonly InstallmentStatus Overdue = new(3, nameof(Overdue));

    public bool IsPaid => Id == Paid.Id;
    public override bool Equals(object? obj)
    {
        if (obj is not InstallmentStatus other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static implicit operator int(InstallmentStatus status) => status.Id;
}