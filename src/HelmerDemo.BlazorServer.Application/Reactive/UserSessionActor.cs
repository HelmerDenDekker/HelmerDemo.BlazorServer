namespace HelmerDemo.BlazorServer.Application.Reactive;

public class UserSessionActor
{
	// Rule: incoming Id + Get => if not in store AND not in PersistedStore => Delete from LocalStorage (or return false result, in order to trigger delete)
	// Rule: incoming Id + Get => if in store => Update UserSessionState to ChildContent (or return true)
	// Rule: incoming Id + Get => if not in store AND is in PersistedStore => Rehydrate UserSessionState from PersistedStore, [start a new UserSessionStream for userId, store the UserId value etc)]
	// Rule: incoming Id + Set => if not in store => Store value in store + PersistedStore, [start a new UserSessionStream for userId, store the UserId value etc)]
	
}
