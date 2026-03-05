using EasyStay.WebApi.Services;

namespace EasyStay.WebApi.Tests.Services;

public class DateConverterTests {
	[Theory]
	[InlineData(2024, 12, 14, 2024, 12, 1)]
	[InlineData(2024, 1, 1, 2024, 1, 1)]
	[InlineData(2024, 2, 29, 2024, 2, 1)]
	public void ToFirstDayOfMonth_GivenDate_ReturnsFirstDayOfThatMonth(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay) {
		// Arrange
		var dateConverter = new DateConverter();

		// Act
		var result = dateConverter.ToFirstDayOfMonth(new DateOnly(year, month, day));

		// Assert
		Assert.Equal(new DateOnly(expectedYear, expectedMonth, expectedDay), result);
	}
}
