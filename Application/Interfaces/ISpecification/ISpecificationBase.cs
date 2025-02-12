namespace Application.Interfaces.ISpecification;

public interface ISpecificationBase<T>
{
    bool IsSatisfiedBy(T dto);
}
