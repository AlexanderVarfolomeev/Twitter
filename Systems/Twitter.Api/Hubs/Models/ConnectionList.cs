namespace Twitter.Api.Hubs.Models;

public class ConnectionList
{
    // userName : connections
    private Dictionary<string, HashSet<string>> _connections;

    public ConnectionList(Dictionary<string, HashSet<string>> connections)
    {
        _connections = connections;
    }

    public ConnectionList()
    {
        _connections = new Dictionary<string, HashSet<string>>();
    }

    public void Add(string userName, string connectionId)
    {
        lock (_connections)
        {
            if (_connections.ContainsKey(userName))
            {
                _connections[userName].Add(connectionId);
            }
            else
            {
                _connections.Add(userName, new HashSet<string>(){connectionId});
            }
        }
    }

    public void Remove(string userName, string connectionId)
    {
        lock (_connections)
        {
            _connections[userName].Remove(connectionId);
        }
    }

    public IEnumerable<string> GetConnections(string userName)
    {
        lock (_connections)
        {
            if (_connections.ContainsKey(userName))
                return _connections[userName].ToList();
            
            return new List<string>(){};
        }
    }

}