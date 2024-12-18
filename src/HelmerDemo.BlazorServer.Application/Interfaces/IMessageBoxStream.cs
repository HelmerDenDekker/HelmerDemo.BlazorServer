using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Interfaces;

public interface IMessageBoxStream
{
	public IObservable<Message> WhenMessageChanged { get; }

	public void OnMessageChanged(Message message);
}
