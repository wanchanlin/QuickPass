using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System;

namespace QuickPass.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public enum Venue
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
        } // Use the defined enum type
        public DateTime Date { get; set; }
        public int TotalTickets { get; set; }


        public ICollection<Ticket> Tickets { get; set; }


        // // one events is related to many accounts
        //public ICollection<Account> Accounts { get; set; }


    }
}
