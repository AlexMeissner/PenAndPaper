using DataTransfer.Dice;
using DataTransfer.Map;
using DataTransfer.Token;
using static Website.Services.ServiceExtension;

namespace Website.Services;

internal interface ICampaignEvents
{
    event Func<DiceRolledEventArgs, Task>? DiceRolled;
    event Func<MapChangedEventArgs, Task>? MapChanged;
    event Func<MapCollectionChangedEventArgs, Task>? MapCollectionChanged;
    event Func<TokenAddedEventArgs, Task>? TokenAdded;
    event Func<TokenMovedEventArgs, Task>? TokenMoved;
}

[SingletonService]
internal class CampaignEvents : ICampaignEvents
{
    public event Func<DiceRolledEventArgs, Task>? DiceRolled;
    public event Func<MapChangedEventArgs, Task>? MapChanged;
    public event Func<MapCollectionChangedEventArgs, Task>? MapCollectionChanged;
    public event Func<TokenAddedEventArgs, Task>? TokenAdded;
    public event Func<TokenMovedEventArgs, Task>? TokenMoved;
}
