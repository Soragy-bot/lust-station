using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Robust.Shared.Maths;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Content.Sunrise.Interfaces.Shared;

namespace Content.Shared._Sunrise.Sponsors;

public sealed class MockSponsorsManager : ISharedSponsorsManager
{
    public void Initialize()
    {
        // Сразу вызываем событие, что данные загружены
        LoadedSponsorInfo?.Invoke();
        LoadedSponsorTiers?.Invoke(GetSponsorTiers());
    }

    public event Action? LoadedSponsorInfo;
    public event Action<List<SponsorInfo>>? LoadedSponsorTiers;

    private static readonly SponsorInfo MockSponsor = new()
    {
        Tier = 5,
        Title = "Ебаная Спонсорка",
        OOCColor = "#00ff9dff", // Cyan
        HavePriorityJoin = true,
        ExtraSlots = 10,
        AllowedRespawn = true,
        AllowedFlavor = true,
        SizeFlavor = 10000,
        GhostThemes = new[]
        {
            "Gold", "Silver", "White", "Platinum", "Narsie", "Kharin", "Legion", "Narbee", "Reaper",
            "Bubblegum", "MegaLegion", "HonkChampion", "Hulk", "Yellow", "Pink", "Cat", "Purple", "Red",
            "BlueArchiveHibiki", "BlueArchiveRio", "BlueArchiveYuuka", "BlueArchiveGirl1", "BlueArchiveGirl2",
            "BlueArchiveGirl3", "BlueArchiveGirl4", "BlueArchiveGirl5", "BlueArchiveGirl6", "BlueArchiveGirl7",
            "BlueArchiveGirl8", "BlueArchiveGirl9", "BlueArchiveGirl10", "BlueArchiveGirl11", "BlueArchiveGirl12",
            "BlueArchiveGirl13", "BlueArchiveGirl14", "BlueArchiveGirl15", "Randy", "WizdenBlock", "Asuka",
            "VigersRay", "RedGirl", "PinkGirl", "PurpleGirl", "Revenant", "HoloCarp", "HoloGirl", "Pirate", "Draedon"
        },
        Pets = new[]
        {
            "PetSelectionRabbit", "PetSelectionAnteater", "PetSelectionOctopus", "PetSelectionOtter",
            "PetSelectionNavySeal", "PetSelectionAlexander", "PetSelectionCorgiIan", "PetSelectionMcGriff",
            "PetSelectionSlimesPet", "PetSelectionCatRuntime", "PetSelectionCorgiLisa", "PetSelectionCatFloppa",
            "PetSelectionWalter", "PetSelectionCrabAtmos", "PetSelectionPossumMorty", "PetSelectionPossumPoppy",
            "PetSelectionPaperwork", "PetSelectionCorgiIanOld", "PetSelectionGoose", "PetSelectionClownGoblin",
            "PetSelectionCorgiIanPup", "PetSelectionParrot"
        },
        SpawnEquipment = "LustSponsorTier3",
        AllowedMarkings = new[] { "CustomMarking" },
        AllowedVoices = new[]
        {
            "Biden", "Papa", "Cirilla", "Sidorovich", "TfEngineer", "TfHeavy", "TfMedic", "TfSniper",
            "TfSpy", "TfDemoman", "Johnny", "EmperorTit", "GuardSkyrim", "HermaeusMora", "Nord", "Ulfric",
            "GoodThalya", "Judy", "PortalGlados", "StalkerSidorovich", "Threedog"
        },
        AllowedLoadouts = new[]
        {
            "SunriseSponsorTier4", "FishSponsorTier3", "NeweraSponsorTier3", "LustSponsorTier3",
            "EngineerCloak", "CaptainSpecialCloak", "CmoSpecialCloak", "HopSpecialCloak", "HosSpecialCloak",
            "IAACloak", "QuartermasterSpecialCloak", "RdSpecialCloak", "SecurityCloak", "QilluCloak",
            "ContributorLoadout", "BlueShieldModsuit", "ComMaidModsuit", "CommonModsuit", "Boombox"
        },
        AllowedSpecies = new[] { "SlimePerson", "Dwarf", "Felinid", "Reptilian", "Miliran", "Milira", "Resomi", "HumanoidXeno", "Predator" },
        OpenAntags = new[] { "Traitor", "NukeOp", "Revolutionary" },
        OpenRoles = new[] { "Captain", "ChiefEngineer", "HeadOfSecurity" },
        OpenGhostRoles = new[] { "GhostRoleTest" },
        PriorityAntags = new[] { "Traitor" },
        PriorityRoles = new[] { "Captain" },
        BypassRoles = new[] { "Captain" }
    };

    private static readonly List<string> AllSponsorPrototypes = new()
    {
        "Asuka", "Biden", "BlueArchiveGirl1", "BlueArchiveGirl10", "BlueArchiveGirl11", "BlueArchiveGirl12",
        "BlueArchiveGirl13", "BlueArchiveGirl14", "BlueArchiveGirl15", "BlueArchiveGirl2", "BlueArchiveGirl3",
        "BlueArchiveGirl4", "BlueArchiveGirl5", "BlueArchiveGirl6", "BlueArchiveGirl7", "BlueArchiveGirl8",
        "BlueArchiveGirl9", "BlueArchiveHibiki", "BlueArchiveRio", "BlueArchiveYuuka", "BlueShieldModsuit",
        "Boombox", "Brejnev", "Bubblegum", "CaptainSpecialCloak", "Cat", "Cirilla", "CmoSpecialCloak",
        "ColinMoriartyFL1", "ComMaidModsuit", "CommonModsuit", "ContributorLoadout", "Draedon", "ElderLyonsFL3",
        "EmperorTit", "EngineerCloak", "EyesHudSecurityAlt", "Gold", "GoodThalya", "GuardSkyrim", "HeartstoneThrud",
        "HermaeusMora", "HoloCarp", "HoloGirl", "HonkChampion", "HopSpecialCloak", "HosSpecialCloak", "Hulk",
        "IAACloak", "JerichoFL3", "Jirinovskiy", "Johnny", "Judy", "KateSmirnovaTB", "Kharin", "Kopatich",
        "Legion", "Livsey2", "LordHarkon", "Losyash", "MaksimPoltavskiy", "MarratorD3", "MegaLegion", "Milira",
        "Minkir", "MoiraBrown", "Myron", "Narbee", "Narsie", "Nord", "OswaldForrest", "Papa",
        "Pathologic2AlexanderBlok", "Pathologic2AlexanderSaburov", "Pathologic2AnglayaLilich",
        "Pathologic2ArtemiyBurach", "Pathologic2GeorgiyKain", "Pathologic2Grif", "Pathologic2Inkvizitor",
        "Pathologic2MarkBessmertnik", "PetSelectionAlexander", "PetSelectionAnteater", "PetSelectionCatFloppa",
        "PetSelectionCatRuntime", "PetSelectionClownGoblin", "PetSelectionCorgiIan", "PetSelectionCorgiIanOld",
        "PetSelectionCorgiIanPup", "PetSelectionCorgiLisa", "PetSelectionCrabAtmos", "PetSelectionGoose",
        "PetSelectionMcGriff", "PetSelectionNavySeal", "PetSelectionOctopus", "PetSelectionOtter",
        "PetSelectionPaperwork", "PetSelectionParrot", "PetSelectionPossumMorty", "PetSelectionPossumPoppy",
        "PetSelectionRabbit", "PetSelectionSlimesPet", "PetSelectionWalter", "Pink", "PinkGirl", "Pirate",
        "Platinum", "PortalGlados", "Purple", "PurpleGirl", "QilluCloak", "QuartermasterSpecialCloak",
        "Randy", "RdSpecialCloak", "Reaper", "Red", "RedGirl", "Resomi", "Revenant", "RobertMaccready",
        "Romka", "SBSquidward", "SecurityCloak", "SecuritySheathBelt", "SemenBaburinTB", "Serana", "Sidorovich",
        "Silver", "SophroniaFranklin", "Squidward", "StalkerSidorovich", "TF2Medic", "TemplarAssasin",
        "TfDemoman", "TfEngineer", "TfHeavy", "TfMedic", "TfSniper", "TfSpy", "Threedog", "ThreedogRadio",
        "TihonovTB", "Ulfric", "VigersRay", "White", "WizdenBlock", "Yellow", "ZenobiaNoke", "templar_assassin_dota_2",
        "HumanoidXeno", "Predator", "SlimePerson", "Dwarf", "Felinid", "Reptilian", "Miliran"
    };


    // Client
    public List<string> GetClientPrototypes() => AllSponsorPrototypes;
    public bool ClientAllowedRespawn() => true;
    public bool ClientAllowedFlavor() => true;
    public int ClientGetSizeFlavor() => 10000;
    public bool ClientIsSponsor() => true;
    public List<SponsorInfo> GetSponsorTiers() => new() { MockSponsor };

    // Server
    public bool TryGetPrototypes(NetUserId userId, [NotNullWhen(true)] out List<string>? prototypes)
    {
        prototypes = AllSponsorPrototypes;
        return true;
    }

    public bool TryGetOocTitle(NetUserId userId, [NotNullWhen(true)] out string? title)
    {
        title = MockSponsor.Title ?? "Sponsor";
        return true;
    }

    public bool TryGetOocColor(NetUserId userId, [NotNullWhen(true)] out Color? color)
    {
        color = Color.FromHex(MockSponsor.OOCColor ?? "#ffffff");
        return true;
    }

    public bool TryGetSpawnEquipment(NetUserId userId, [NotNullWhen(true)] out string? spawnEquipment)
    {
        spawnEquipment = MockSponsor.SpawnEquipment ?? "SponsorLoadoutSunrise";
        return true;
    }

    public bool TryGetGhostThemes(NetUserId userId, [NotNullWhen(true)] out List<string>? ghostThemes)
    {
        ghostThemes = MockSponsor.GhostThemes.ToList();
        return true;
    }

    public bool TryGetBypassRoles(NetUserId userId, [NotNullWhen(true)] out List<string>? bypassRoles)
    {
        bypassRoles = MockSponsor.BypassRoles.ToList();
        return true;
    }

    public int GetSizeFlavor(NetUserId userId) => 10000;
    public bool IsAllowedFlavor(NetUserId userId) => true;
    public int GetExtraCharSlots(NetUserId userId) => 10;
    public bool HavePriorityJoin(NetUserId userId) => true;
    public bool IsSponsor(NetUserId userId) => true;
    public bool IsAllowedRespawn(NetUserId userId) => true;

    public List<ICommonSession> PickPrioritySessions(List<ICommonSession> sessions, string roleId) => sessions;

    public NetUserId? PickRoleSession(HashSet<NetUserId> users, string roleId) => users.FirstOrDefault();

    public bool TryGetPriorityGhostRoles(NetUserId userId, [NotNullWhen(true)] out List<string>? priorityRoles)
    {
        priorityRoles = MockSponsor.PriorityGhostRoles.ToList();
        return true;
    }

    public bool TryGetPriorityAntags(NetUserId userId, [NotNullWhen(true)] out List<string>? priorityAntags)
    {
        priorityAntags = MockSponsor.PriorityAntags.ToList();
        return true;
    }

    public bool TryGetPriorityRoles(NetUserId userId, [NotNullWhen(true)] out List<string>? priorityRoles)
    {
        priorityRoles = MockSponsor.PriorityRoles.ToList();
        return true;
    }

    public bool TryGetPets(NetUserId userId, [NotNullWhen(true)] out List<string>? petSelections)
    {
        petSelections = MockSponsor.Pets.ToList();
        return true;
    }

    public void Update() { }
}
