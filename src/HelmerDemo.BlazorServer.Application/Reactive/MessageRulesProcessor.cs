using System.Reactive.Linq;
using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Interfaces;
using HelmerDemo.BlazorServer.Shared.Tools.Extensions;

namespace HelmerDemo.BlazorServer.Application.Reactive;

// I want a class to process the messages
// It is some sort of a Mediator in the sense that it is ONE class processing for many BlazorPage instances
public class MessageRulesProcessor : IDisposable
{
	private readonly IMessageBoxStream _messageBoxStream;
	private IDisposable? _firstValidContentSubscription;
	private IDisposable? _contentRemovedSubscription;

	public MessageRulesProcessor(IMessageBoxStream messageBoxStream)
	{
		_messageBoxStream = messageBoxStream;
	}
	
	// TODO: These rules should be registered somewhere. A "BUS" or something like that.

	/// <summary>
	/// When the user starts composing a message, start a new message in the stream
	/// </summary>
	public void OnFirstValidContent()
	{
		// Define stream of first valid content
		IObservable<IList<Message>> firstValidContentStream = _messageBoxStream.WhenMessageChanged
			.Buffer(2, 1);

		_firstValidContentSubscription = firstValidContentStream.Subscribe(m=> {
			if (m.Count == 2 && m[0].Content.IsNullOrWhiteSpace() && !m[1].Content.IsNullOrWhiteSpace())
			{
				// Update CreatedAt
				
			}
		});
	}

	/// <summary>
	/// When the content was removed, but not on send, the user state moves to active
	/// </summary>
	public void ContentRemoved()
	{
		IObservable<IList<Message>> contentRemovedStream = _messageBoxStream.WhenMessageChanged.Buffer(2, 1);
		// TODO: Merge with changeEventSubscription to detect the Send event
		_contentRemovedSubscription = contentRemovedStream.Subscribe(m =>
		{
			if (m.Count == 2 && !m[0].Content.IsNullOrWhiteSpace() && m[1].Content.IsNullOrWhiteSpace())
			{
				// TODO: Update UserState
			}
		});
	}
	
	

	public void Dispose()
	{
		// TODO: release managed resources here
		_firstValidContentSubscription?.Dispose();
		_contentRemovedSubscription?.Dispose();
	}
}
