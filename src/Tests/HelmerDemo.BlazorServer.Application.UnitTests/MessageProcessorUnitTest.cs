using System.Reactive.Disposables;
using System.Reactive.Linq;
using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Interfaces;
using HelmerDemo.BlazorServer.Application.Reactive;
using Moq;

namespace HelmerDemo.BlazorServer.Application.UnitTests;

public class MessageProcessorUnitTests
{
	
	[Fact]
	public void ProcessMessageStream_WhenCalled_ShouldProcessMessageStream()
	{
		// Arrange
		var messageBoxStreamMock = new Mock<IMessageBoxStream>();
		messageBoxStreamMock.Setup(x=>x.WhenMessageChanged).Returns(CreateMessageStream());
		var messageProcessor = new MessageRulesProcessor(messageBoxStreamMock.Object);

		// Act
		messageProcessor.OnFirstValidContent();

		// Assert
		messageBoxStreamMock.Verify(x => x.WhenMessageChanged, Times.Once);
		messageBoxStreamMock.Verify(x => x.OnMessageChanged(It.IsAny<Message>()), Times.Exactly(1));
	}
	
	private IObservable<Message> CreateMessageStream()
	{
		return Observable.Create(
			(IObserver<Message> observer) =>
			{
				observer.OnNext(Message.Create(""));
				observer.OnNext(Message.Create("Hello"));
				observer.OnNext(Message.Create("World"));
				observer.OnCompleted();
				return Disposable.Empty;
			}

		);
	}
	
}
