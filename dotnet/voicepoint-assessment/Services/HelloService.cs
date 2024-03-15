using Voicepoint.Assessment.Api.Models;

namespace Voicepoint.Assessment.Services;

public class HelloService : IHelloService
{

    public SayHelloResponse SayHello(string name)
    {
        return new SayHelloResponse { Hello = $"Hello {name}" };
    }
}