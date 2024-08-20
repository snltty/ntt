using STUN.Client;
using STUN.StunResult;
using System.Net;
using System.Net.Sockets;

namespace ntt
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            string server = "stun.syncthing.net";
            if (args.Length > 0)
            {
                server = args[0];
            }


            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                ClassicStunResult result = await TestClassicNatTypeAsync(server, cts.Token).WaitAsync(TimeSpan.FromMilliseconds(15000));
                Console.WriteLine(result.NatType);
                Console.WriteLine(result.LocalEndPoint);
                Console.WriteLine(result.PublicEndPoint);
            }
            catch (Exception)
            {
                Console.WriteLine("fail");
                await cts.CancelAsync();
            }

            try
            {
                Console.ReadKey();
            }
            catch (Exception)
            {
            }
        }

        private static async Task<ClassicStunResult> TestClassicNatTypeAsync(string server, CancellationToken token)
        {
            try
            {
                IPEndPoint serverEP = GetEndPoint(server, 3478);

                using StunClient3489 client = new(serverEP, new IPEndPoint(IPAddress.Any, 0), null);
                await client.ConnectProxyAsync(token);
                try
                {
                    await client.QueryAsync(token);
                }
                finally
                {
                    await client.CloseProxyAsync(token);
                }

                return client.State;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new ClassicStunResult { };
        }

        public static IPAddress GetDomainIp(string domain)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(domain))
                {
                    return null;
                }
                if (IPAddress.TryParse(domain, out IPAddress ip))
                {
                    return ip;
                }
                return Dns.GetHostEntry(domain, AddressFamily.InterNetwork).AddressList.FirstOrDefault();
            }
            catch (Exception)
            {
            }
            return null;
        }
        public static IPEndPoint GetEndPoint(string host, int defaultPort)
        {
            try
            {
                string[] hostArr = host.Split(':');
                int port = defaultPort;
                if (hostArr.Length == 2)
                {
                    port = int.Parse(hostArr[1]);
                }
                IPAddress ip = GetDomainIp(hostArr[0]);
                return new IPEndPoint(ip, port);
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
