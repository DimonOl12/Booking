namespace EasyStay.Application.Interfaces;

public interface ICollectionValidator {
	public bool IsDistinct<T>(IEnumerable<T> collaction);
	public bool IsNullPossibleDistinct<T>(IEnumerable<T>? collaction);
}
