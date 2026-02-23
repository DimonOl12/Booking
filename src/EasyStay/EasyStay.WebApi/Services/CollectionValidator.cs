using EasyStay.Application.Interfaces;

namespace EasyStay.WebApi.Services;

public class CollectionValidator : ICollectionValidator {
	public bool IsDistinct<T>(IEnumerable<T> collaction) {
		HashSet<T> uniqueItems = [];

		foreach (var item in collaction) {
			if (!uniqueItems.Add(item))
				return false;
		}
		return true;
	}

	public bool IsNullPossibleDistinct<T>(IEnumerable<T>? collaction) {
		if (collaction is null)
			return true;

		return IsDistinct(collaction.Where(x => x is not null));
	}
}
