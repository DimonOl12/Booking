using EasyStay.WebApi.Services;

namespace EasyStay.WebApi.Tests.Services;

public class TimeConverterTests {
	[Fact]
	public void ToDateTimeOffsetFromUtcTimeOnly_8Hours_ReturnsSameTime() {
		// Arrange
		var timeConverter = new TimeConverter();

		// Act
		var result = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(new TimeOnly(8, 0));

		// Assert
		Assert.Equal(new DateTimeOffset(1, 1, 1, 8, 0, 0, 0, 0, TimeSpan.Zero), result);
	}

	[Fact]
	public void ToDateTimeOffsetFromUtcTimeOnly_15Hours25Minutes44Seconds_ReturnsSameTime() {
		// Arrange
		var timeConverter = new TimeConverter();

		// Act
		var result = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(new TimeOnly(15, 25, 44));

		// Assert
		Assert.Equal(new DateTimeOffset(1, 1, 1, 15, 25, 44, 0, 0, TimeSpan.Zero), result);
	}

	[Fact]
	public void ToDateTimeOffsetFromUtcTimeOnly_8Hours_ReturnsZeroOffset() {
		// Arrange
		var timeConverter = new TimeConverter();

		// Act
		var result = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(new TimeOnly(8, 0));
		var offset = result.Offset;

		// Assert
		Assert.Equal(new TimeSpan(0, 0, 0, 0, 0, 0), offset);
	}
}
