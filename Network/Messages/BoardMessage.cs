using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheatGameApp;
using System.Xml;
using System.Collections.ObjectModel;

namespace CheatGameModel.Network.Messages
{
    public class BoardMessage : Message
    {
        private BoardState _state;

        public BoardMessage(BoardState state)
            : base()
        {
            _state = state;
        }

        public BoardMessage(XmlDocument xml)
            : base(xml)
        {
            _state = new BoardState();
            LoadProperties(_state);
        }

        protected override void AppendProperties()
        {
            base.AppendProperties();
            AppendProperties(_state);
        }

        public BoardState GetBoardState()
        {
            return _state;
        }
    }

    public class BoardState
    {
        // following properties can be assigned at the receiving end as is
        public string ComputerMsg { get; set; }
        public string PlayerMsg { get; set; }
        public string BoardMsg { get; set; }

        public int AgentCardsNum { get; set; }
        public int PlayedCardsNum { get; set; }
        public int BoardCardsNum { get; set; }

        public bool AgentStartPressed { get; set; }
        public bool IsServerTurn { get; set; }
        public bool TakeCardEnable { get; set; }
        public bool CallCheatEnable { get; set; }

        public string LastClaimType { get; set; }
        public string LastClaimNum { get; set; }
        public string LastClaimType2 { get; set; }
        public string LastClaimPlayerName { get; set; }

        public bool IsRevealing { get; set; }
        public bool CanDispute { get; set; }
        public string UsedCardsNumbers { get; set; }

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

        public int[] GetCards()
        {
            int[] PlayerCards = new int[13];

            PlayerCards[0] = Ace;
            PlayerCards[1] = Two;
            PlayerCards[2] = Three;
            PlayerCards[3] = Four;
            PlayerCards[4] = Five;
            PlayerCards[5] = Six;
            PlayerCards[6] = Seven;
            PlayerCards[7] = Eight;
            PlayerCards[8] = Nine;
            PlayerCards[9] = Ten;
            PlayerCards[10] = Jack;
            PlayerCards[11] = Queen;
            PlayerCards[12] = King;

            return PlayerCards;
        }

        //public void SetCards(ObservableCollection<CardsStruct.DataObject> PlayerCards)
        //{
        //    Ace = PlayerCards[0].Ace;
        //    Two = PlayerCards[0].Two;
        //    Three = PlayerCards[0].Three;
        //    Four = PlayerCards[0].Four;
        //    Five = PlayerCards[0].Five;
        //    Six = PlayerCards[0].Six;
        //    Seven = PlayerCards[0].Seven;
        //    Eight = PlayerCards[0].Eight;
        //    Nine = PlayerCards[0].Nine;
        //    Ten = PlayerCards[0].Ten;
        //    Jack = PlayerCards[0].Jack;
        //    Queen = PlayerCards[0].Queen;
        //    King = PlayerCards[0].King;
        //}

        public BoardState()
        {
            ComputerMsg = "..Starting Up";
            PlayerMsg = "..Starting Up";
            BoardMsg = "New Game";

            BoardCardsNum = 52;

            IsServerTurn = false; // sets visibility/enabled to don't show/disable
        }
    }
}
