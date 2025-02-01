using System.ComponentModel.DataAnnotations;
namespace QuickPass.Models
{
    public enum VenueType
    {
        ScotiabankArena,
        MasseyHall,
        DanforthMusicHall,
        BellCentre,
        RogersArena,
        Saddledome,
        MTSCentre,
        BudweiserGardens,
        PlaceBell,
        CentreVideotron,
        RogersPlace,
        ScotiabankSaddledome,
        SaveOnFoodsMemorialCentre,
        MileOneCentre,
        AvenirCentre,
        FirstOntarioCentre,
        LeonCentre,
        MeridianCentre,
        AbbotsfordCentre,
        CNCentre,
        EnmaxCentre,
        MosaicPlace,
        SandmanCentre,
        SouthOkanaganEventsCentre,
        WestobaPlace,
        KeystoneCentre,
        TCUPlace,
        SaskTelCentre,
        BrandtCentre,
        CreditUnionCentre,
        BellMTSPlace,
        BellAliantCentre,
        EastlinkCentre,
        HarbourStation,
        MonctonColiseum,
        MileOneStadium,
        PepsiCentre,
        RathEastlinkCommunityCentre,
        ScotiabankCentre,
        SleemanCentre,
        WindsorFamilyCreditUnionCentre
    }
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public VenueType Venue { get; set; } // Use the defined enum type
        public DateTime Date { get; set; }
        public int TotalTickets { get; set; }





    }
}
