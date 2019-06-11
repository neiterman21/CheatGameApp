using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using CheatGameApp;
using System.Collections.ObjectModel;
using NAudio.Wave;

namespace CheatGameModel.Network.Messages
{
    public enum MoveType
    {
        PlayMove = 0,
        TakeCard = 1,
        CallCheat = 2,
        StartPressed = 3,
        EndReveal = 4,
        ForfeitGame = 5,
        CallCheatyOpponent = 6
    }

    public sealed class MoveMessage : Message
    {
        private Move _move;

        public MoveMessage(Move move) : base()
        {
            _move = move;
        }

        public MoveMessage(XmlDocument xml)
            : base(xml)
        {
            _move = new Move();
            LoadProperties(_move);
        }

        protected override void AppendProperties()
        {
            base.AppendProperties();
            AppendProperties(_move);
        }

        public Move GetMove()
        {
            return _move;
        }
    }

    public class Move
    {
        public TimeSpan MoveTime { get; set; }
        public MoveType MoveType { get; set; }
        

        // real move cards
        public int Ace { get; set; }
        public int Two { get; set; }
        public int Three { get; set; }
        public int Four { get; set; }
        public int Five { get; set; }
        public int Six { get; set; }
        public int Seven { get; set; }
        public int Eight { get; set; }
        public int Nine { get; set; }
        public int Ten { get; set; }
        public int Jack { get; set; }
        public int Queen { get; set; }
        public int King { get; set; }

        // claim move cards
        public int AceC { get; set; }
        public int TwoC { get; set; }
        public int ThreeC { get; set; }
        public int FourC { get; set; }
        public int FiveC { get; set; }
        public int SixC { get; set; }
        public int SevenC { get; set; }
        public int EightC { get; set; }
        public int NineC { get; set; }
        public int TenC { get; set; }
        public int JackC { get; set; }
        public int QueenC { get; set; }
        public int KingC { get; set; }

        //public ObservableCollection<CardsStruct.DataObject> GetRealMoveCards()
        //{
        //    ObservableCollection<CardsStruct.DataObject> PlayerCards =
        //        new ObservableCollection<CardsStruct.DataObject>();
        //    var cardsStruct = new CardsStruct.DataObject();
        //    PlayerCards.Add(cardsStruct);

        //    PlayerCards[0].Ace = Ace;
        //    PlayerCards[0].Two = Two;
        //    PlayerCards[0].Three = Three;
        //    PlayerCards[0].Four = Four;
        //    PlayerCards[0].Five = Five;
        //    PlayerCards[0].Six = Six;
        //    PlayerCards[0].Seven = Seven;
        //    PlayerCards[0].Eight = Eight;
        //    PlayerCards[0].Nine = Nine;
        //    PlayerCards[0].Ten = Ten;
        //    PlayerCards[0].Jack = Jack;
        //    PlayerCards[0].Queen = Queen;
        //    PlayerCards[0].King = King;

        //    return PlayerCards;
        //}

        public void SetRealMoveCards(int[] PlayerCards)
        {
            Ace = PlayerCards[0];
            Two = PlayerCards[1];
            Three = PlayerCards[2];
            Four = PlayerCards[3];
            Five = PlayerCards[4];
            Six = PlayerCards[5];
            Seven = PlayerCards[6];
            Eight = PlayerCards[7];
            Nine = PlayerCards[8];
            Ten = PlayerCards[9];
            Jack = PlayerCards[10];
            Queen = PlayerCards[11];
            King = PlayerCards[12];
        }

        //public int[] GetClaimMoveCards()
        //{
        //    ObservableCollection<CardsStruct.DataObject> PlayerCards =
        //        new ObservableCollection<CardsStruct.DataObject>();
        //    var cardsStruct = new CardsStruct.DataObject();
        //    PlayerCards.Add(cardsStruct);

        //    PlayerCards[0].Ace = AceC;
        //    PlayerCards[0].Two = TwoC;
        //    PlayerCards[0].Three = ThreeC;
        //    PlayerCards[0].Four = FourC;
        //    PlayerCards[0].Five = FiveC;
        //    PlayerCards[0].Six = SixC;
        //    PlayerCards[0].Seven = SevenC;
        //    PlayerCards[0].Eight = EightC;
        //    PlayerCards[0].Nine = NineC;
        //    PlayerCards[0].Ten = TenC;
        //    PlayerCards[0].Jack = JackC;
        //    PlayerCards[0].Queen = QueenC;
        //    PlayerCards[0].King = KingC;

        //    return PlayerCards;
        //}

        public void SetClaimMoveCards(int[] PlayerCards)
        {
            AceC = PlayerCards[0];
            TwoC = PlayerCards[1];
            ThreeC = PlayerCards[2];
            FourC = PlayerCards[3];
            FiveC = PlayerCards[4];
            SixC = PlayerCards[5];
            SevenC = PlayerCards[6];
            EightC = PlayerCards[7];
            NineC = PlayerCards[8];
            TenC = PlayerCards[9];
            JackC = PlayerCards[10];
            QueenC = PlayerCards[11];
            KingC = PlayerCards[12];
        }
    }
}
