using EasyStay.WebApi.Services;

namespace EasyStay.WebApi.Tests.Services;

public class CollectionValidatorTests {
	[Fact]
	public void IsDistinct_EmptyCollection_ReturnsTrue() {
		// Arrange
		var collectionValidator = new CollectionValidator();

		// Act
		var result = collectionValidator.IsDistinct(Array.Empty<int>());

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void IsDistinct_UniqueCollection_ReturnsTrue() {
		// Arrange
		var collectionValidator = new CollectionValidator();

		// Act
		var result = collectionValidator.IsDistinct([1, 2, 3, 4, 5]);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void IsDistinct_NonUniqueCollection_ReturnsFalse() {
		// Arrange
		var collectionValidator = new CollectionValidator();

		// Act
		var result = collectionValidator.IsDistinct([1, 2, 1]);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void IsNullPossibleDistinct_Null_ReturnsTrue() {
		// Arrange
		var collectionValidator = new CollectionValidator();

		// Act
		var result = collectionValidator.IsNullPossibleDistinct<int>(null);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void IsNullPossibleDistinct_EmptyCollection_ReturnsTrue() {
		// Arrange
		var collectionValidator = new CollectionValidator();

		// Act
		var result = collectionValidator.IsNullPossibleDistinct(Array.Empty<int?>());

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void IsNullPossibleDistinct_UniqueCollectionWithNullItems_ReturnsTrue() {
		// Arrange
		var collectionValidator = new CollectionValidator();

		// Act
		var result = collectionValidator.IsNullPossibleDistinct(new int?[] { 1, 2, null, 3, 4, 5, null });

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void IsNullPossibleDistinct_NonUniqueCollectionWithNullItems_ReturnsFalse() {
		// Arrange
		var collectionValidator = new CollectionValidator();

		// Act
		var result = collectionValidator.IsNullPossibleDistinct(new int?[] { 1, 2, null, 2, 2, null });

		// Assert
		Assert.False(result);
	}
}
