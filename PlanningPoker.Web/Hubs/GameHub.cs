using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Data.DTOs;

namespace PlanningPoker.Web.Hubs
{
  public static class GameEvents
  {
    internal static string Send = "Send";
    internal static string GameJoined = "GameJoined";
    internal static string UserJoined = "UserJoined";
    internal static string UserLeft = "UserLeft";
    internal static string CardPlayed = "CardPlayed";
    internal static string CardsShown = "CardsShown";
    internal static string NewGameStarted = "NewGameStarted";
  }
  public class GameHub : Hub
  {
    public override async Task OnDisconnectedAsync(Exception exception)
    {
      var user = ConnectedUsers.GetByConnectionId(Context.ConnectionId);
      ConnectedUsers.GameUsers.Remove(user);
      await Clients.Group(user.GroupId).SendAsync(GameEvents.UserLeft, ConnectedUsers.GetByGroup(user.GroupId));
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.GroupId);
      await base.OnDisconnectedAsync(exception);
    }

    public async void JoinGroup(JoinGameRequestDto request)
    {
      var user = new GameUser
      {
        Name = request.Name,
        GroupId = request.GroupId,
        ConnectionId = Context.ConnectionId
      };

      ConnectedUsers.GameUsers.Add(user);

      await Groups.AddToGroupAsync(Context.ConnectionId, user.GroupId);
      await Clients.Client(Context.ConnectionId).SendAsync(GameEvents.GameJoined, user);
      await Clients.Group(user.GroupId).SendAsync(GameEvents.UserJoined, ConnectedUsers.GetByGroup(user.GroupId));
    }

    public async void PlayCard(string card)
    {
      var user = ConnectedUsers.GetByConnectionId(Context.ConnectionId);
      await Clients.Group(user.GroupId).SendAsync(GameEvents.CardPlayed, new CardDTO
      {
        ConnectionId = user.ConnectionId,
        Card = card
      });
    }

    public async void ShowCards()
    {
      var user = ConnectedUsers.GetByConnectionId(Context.ConnectionId);
      await Clients.Group(user.GroupId).SendAsync(GameEvents.CardsShown);
    }

    public async void StartNewGame()
    {
      var user = ConnectedUsers.GetByConnectionId(Context.ConnectionId);
      await Clients.Group(user.GroupId).SendAsync(GameEvents.NewGameStarted);
    }
  }
}