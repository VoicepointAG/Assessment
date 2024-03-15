using Voicepoint.Assessment.Api.Models;

namespace Voicepoint.Assessment.Services;

public interface IHelloService
{
    SayHelloResponse SayHello(string name);
}