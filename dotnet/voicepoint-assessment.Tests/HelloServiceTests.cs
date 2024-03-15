using FluentAssertions;
using Voicepoint.Assessment.Services;

namespace voicepoint_assessment.Tests
{
    public class HelloServiceTests
    {
        public class SayHelloAsync
        {
            [Fact]
            public void SaysHello()
            {
                // arrange
                var testee = Arrange();

                // act
                var actual = testee.SayHello("Peter");

                // assert
                actual.Hello.Should().Be("Hello Peter");
            }

        }

        public static HelloService Arrange()
        {
            var testee = new HelloService();
            return testee;
        }

    }
}