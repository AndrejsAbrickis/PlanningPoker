using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlanningPoker.Data.DTOs;

namespace PlanningPoker.Core
{
    public class DefaultPresenceHublifetimeManager<THub> : PresenceHubLifetimeManager<THub, DefaultHubLifetimeManager<THub>>
            where THub : HubWithPresence
    {
        public DefaultPresenceHublifetimeManager(
            IUserTracker<THub> userTracker,
            IServiceScopeFactory serviceScopeFactory,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider) : base(userTracker, serviceScopeFactory, loggerFactory, serviceProvider)
        {
        }
    }

    public class PresenceHubLifetimeManager<THub, THubLifetimeManager> : HubLifetimeManager<THub>, IDisposable
        where THubLifetimeManager : HubLifetimeManager<THub>
        where THub : HubWithPresence
    {
        private readonly HubConnectionList _connections = new HubConnectionList();
        private readonly IUserTracker<THub> _userTracker;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly HubLifetimeManager<THub> _wrappedHubLifetimeManager;
        private IHubContext<THub> _hubContext;

        public PresenceHubLifetimeManager(
            IUserTracker<THub> userTracker,
            IServiceScopeFactory serviceScopeFactory,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            _userTracker = userTracker;
            _userTracker.UsersJoined += OnUsersJoined;
            _userTracker.UsersLeft += OnUsersLeft;

            _serviceScopeFactory = serviceScopeFactory;
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<PresenceHubLifetimeManager<THub, THubLifetimeManager>>();
            _wrappedHubLifetimeManager = serviceProvider.GetRequiredService<THubLifetimeManager>();
        }

        public override async Task OnConnectedAsync(HubConnectionContext connection)
        {
            await _wrappedHubLifetimeManager.OnConnectedAsync(connection);
            _connections.Add(connection);
            await _userTracker.AddUser(connection, new UserDetailsDto(connection.ConnectionId, connection.User.Identity.Name, Guid.NewGuid().ToString()));
        }

        public override async Task OnDisconnectedAsync(HubConnectionContext connection)
        {
            await _wrappedHubLifetimeManager.OnDisconnectedAsync(connection);
            _connections.Remove(connection);
            await _userTracker.RemoveUser(connection);
        }

        private async void OnUsersJoined(UserDetailsDto[] users)
        {
            await Notify(hub =>
            {
                if (users.Length == 1)
                {
                    if (users[0].ConnectionId != hub.Context.ConnectionId)
                    {
                        return hub.OnUsersJoined(users);
                    }
                }
                else
                {
                    return hub.OnUsersJoined(
                        users.Where(u => u.ConnectionId != hub.Context.Connection.ConnectionId).ToArray());
                }
                
                return Task.CompletedTask;
            });
        }

        private async void OnUsersLeft(UserDetailsDto[] users)
        {
            await Notify(hub => hub.OnUsersLeft(users));
        }

        private async Task Notify(Func<THub, Task> invocation)
        {
            foreach (var connection in _connections)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var hubActivator = scope.ServiceProvider.GetRequiredService<IHubActivator<THub>>();
                    var hub = hubActivator.Create();

                    if (_hubContext == null)
                    {
                        // Cannot be injected due to circular dependency
                        _hubContext = _serviceProvider.GetRequiredService<IHubContext<THub>>();
                    }

                    hub.Context = new HubCallerContext(connection);
                    hub.Groups = _hubContext.Groups;

                    try
                    {
                        await invocation(hub);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Presence notification failed.");
                    }
                    finally
                    {
                        hubActivator.Release(hub);
                    }
                }
            }
        }

        public void Dispose()
        {
            _userTracker.UsersJoined -= OnUsersJoined;
            _userTracker.UsersLeft -= OnUsersLeft;
        }

        public override Task SendAllAsync(string methodName, object[] args)
        {
            return _wrappedHubLifetimeManager.SendAllAsync(methodName, args);
        }

        public override Task SendAllExceptAsync(string methodName, object[] args, IReadOnlyList<string> excludedIds)
        {
            return _wrappedHubLifetimeManager.SendAllExceptAsync(methodName, args, excludedIds);
        }

        public override Task SendConnectionAsync(string connectionId, string methodName, object[] args)
        {
            return _wrappedHubLifetimeManager.SendConnectionAsync(connectionId, methodName, args);
        }

        public override Task SendGroupAsync(string groupName, string methodName, object[] args)
        {
            return _wrappedHubLifetimeManager.SendGroupAsync(groupName, methodName, args);
        }

        public override Task SendUserAsync(string userId, string methodName, object[] args)
        {
            return _wrappedHubLifetimeManager.SendUserAsync(userId, methodName, args);
        }

        public override Task AddGroupAsync(string connectionId, string groupName)
        {
            return _wrappedHubLifetimeManager.AddGroupAsync(connectionId, groupName);
        }

        public override Task RemoveGroupAsync(string connectionId, string groupName)
        {
            return _wrappedHubLifetimeManager.RemoveGroupAsync(connectionId, groupName);
        }

        public override Task SendConnectionsAsync(IReadOnlyList<string> connectionIds, string methodName, object[] args)
        {
            throw new NotImplementedException();
        }

        public override Task SendGroupsAsync(IReadOnlyList<string> groupNames, string methodName, object[] args)
        {
            throw new NotImplementedException();
        }

        public override Task SendGroupExceptAsync(string groupName, string methodName, object[] args, IReadOnlyList<string> excludedIds)
        {
            throw new NotImplementedException();
        }

        public override Task SendUsersAsync(IReadOnlyList<string> userIds, string methodName, object[] args)
        {
            throw new NotImplementedException();
        }
    }
}