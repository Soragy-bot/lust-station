<<<<<<< Updated upstream
#if SUNRISE_PRIVATE
using Content.Server._SunrisePrivate.JoinQueue;
using Content.Server._SunrisePrivate.ServiceAuth;
using Content.Server._SunrisePrivate.Sponsors;
using Content.Server._SunrisePrivate.AntiNuke;
using Content.Sunrise.Interfaces.Server;
using Content.Sunrise.Interfaces.Shared;
#endif
=======
using Content.Shared._Sunrise.Sponsors;
using Content.Sunrise.Interfaces.Shared;
>>>>>>> Stashed changes

namespace Content.Server._Sunrise.IoC;

internal static class SunriseServerContentIoC
{
    public static void Register(IDependencyCollection deps)
    {
<<<<<<< Updated upstream
#if SUNRISE_PRIVATE
        IoCManager.Register<ISharedSponsorsManager, ServerSponsorsManager>();
        IoCManager.Register<IServerServiceAuthManager, ServiceAuthManager>();
        IoCManager.Register<IServerJoinQueueManager, JoinQueueManager>();
        IoCManager.Register<AntiNukeManager>();
#endif
=======
        deps.Register<ISharedSponsorsManager, MockSponsorsManager>();
>>>>>>> Stashed changes
    }
}
