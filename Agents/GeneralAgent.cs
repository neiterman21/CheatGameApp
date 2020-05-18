using CheatGameApp.Agents;
using CheatGameApp.Model;
using CheatGameModel.Network.Messages;
using CheatGameModel.Players;
using NAudio.Wave;
using System;
using System.Collections.Generic;


namespace CheatGameApp.Agents
{
    class Agent
    {
        private String record_dir;
        private BoardState board;
        private bool board_valid = false;
        private WaveStream claim = null;
        private bool claim_valid = false;
        private Form1 form = null;
        public Random random = new Random();
        public bool first_move = false;
        protected List<TurnHistory> session = new List<TurnHistory>();
        public Agent(String record_dir_ , Form1 form_)
        {
            record_dir = record_dir_;
            form = form_;
        }

        public Demographics fill_demographic_form()
        {
            Demographics form = new Demographics();
            form.Age = 1;
            form.FullName = "Bob";
            form.Gender = CheatGameModel.Players.Enumarations.Genders.Male;
            form.EducationType = EducationType.BSc;
            form.EducationField = EducationFields.ComputerSciense;
            return form;
        }

        public void onCardsRevel(Deck revel_deck)
        {
            IEnumerator<Card> enumerator = revel_deck.GetEnumerator();
            int j = 0;
            foreach (TurnHistory turn in session)
            {
                Deck actual = new Deck();
                for(int i = 0; i < turn.getCount(); i++)
                {
                    enumerator.MoveNext();
                    actual.Add(new Card(enumerator.Current));
                }             
                turn.actual = actual;
                Console.WriteLine("turn number " + j + " " + turn.ToString());
                j++;
            }
            claim_valid = false;
            board_valid = false;
            session.Clear();

        }

        public void get_claim(WaveStream _claim)
        {   
            claim = _claim;
            claim_valid = true;

            if (session.Count == 0 || session[session.Count - 1].recording != null)
                session.Add(new TurnHistory());
            session[session.Count - 1].recording = _claim;
        }

        public void setFirstCard(Deck first)
        {
            if (session.Count > 0) return;
            session.Add(new TurnHistory(new Deck(first), new Deck(first),null, false));
        }

        public void updateBoardState(BoardState _board)
        {
            if (_board.IsServerTurn) return;
            board = _board;
            board_valid = true;

            if (first_move) return;
            if (session.Count == 0 || session[session.Count - 1].claim != null)
                session.Add(new TurnHistory(new Deck(form.getLastClaimDeck().m_deck)));
            else
                session[session.Count - 1].claim = new Deck (form.getLastClaimDeck().m_deck);
            if (getSessionCount() != _board.PlayedCardsNum)
            {
                throw  new System.ArgumentException("extra cards added session count = " + getSessionCount() + " played cards num = " + _board.PlayedCardsNum); 
            }
        }

        protected bool shouldWait()
        {
            if (form.isStartGameEnabled())
            {   first_move = true;
                board_valid = false;
                claim_valid = false;
                Console.WriteLine("agent clicking start game");
                form.StartGameButton_Click(this, null);
                return true;
            }
            if (board_valid == false) return true;
            if (board.IsServerTurn) return true;

            if (first_move) return false;
            if (!board.CanDispute) return false;
            if (!claim_valid && !board.BoardMsg.Contains("cards from the unused stack")) return true;
           
            return false;
        }

        protected void makeMove()
        {
            switch (decise_move())
            {
                case MoveType.PlayMove:
                    chooseCard();
                    DeckLabel claim = chooseClaim();
                    chooseRecord(claim);
                    play_move(claim);
                    break;
                case MoveType.TakeCard:
                    form.TakeCardButton_Click(this, null);
                    break;
                case MoveType.CallCheat:
                    form.CallCheatButton_Click(this, null);
                    break;
            }

            first_move = false;
            board_valid = false;
            claim_valid = false;
        }
        
        public void agentMove()
        {
           
            if (shouldWait()) return;
            Form1.DelayAction(3500, new Action(() => { this.makeMove(); }));
        }

        protected DeckLabel chooseClaim()
        {
            DeckLabel cardLabels = form.getmyDeck();
            Deck selected_cards = cardLabels.GetSelectedCards();
            DeckLabel last_claim = form.getLastClaimDeck();
            Card previusclaimcard = new Card(last_claim.Deck.GetCounts());
            List<List<Card>> redused_selected_list = selected_cards.ToReducedList();

            if (redused_selected_list.Count == 1 && previusclaimcard.Increase() == redused_selected_list[0][0])
            {
                return form.gethighClaimOptionDeck();
            }

            if (redused_selected_list.Count == 1 && previusclaimcard.Decrease() == redused_selected_list[0][0])
            {
                return form.getlowClaimOptionDeck();
            }
            else
            {

                if (random.NextDouble() >= 0.5) return form.gethighClaimOptionDeck();
                return form.getlowClaimOptionDeck();
            }
        }

        protected MoveType decise_move()
        {
            return MoveType.PlayMove;
            if (claim_valid && random.NextDouble() >= 0.5) return MoveType.CallCheat; 
           if(haveLeagleMove()) return MoveType.PlayMove;
           if (random.NextDouble() >= 0.7) return MoveType.TakeCard;

            return MoveType.PlayMove;
        }

        private bool haveLeagleMove()
        {
            List<CardLabel> cardLabels = form.getmyDeck().m_cardLabels;
            DeckLabel last_claim = form.getLastClaimDeck();
            Card previusclaimcard = new Card(last_claim.Deck.GetCounts());
            foreach (CardLabel c_lable in cardLabels)
            {
                if (previusclaimcard.Decrease() == c_lable.Card || previusclaimcard.Increase() == c_lable.Card)
                {
                    return true;
                }
            }
            return false;
        }

        protected int chooseCard()
        {
            int cards_chosen = 0;
            List<CardLabel> cardLabels = form.getmyDeck().m_cardLabels;

            DeckLabel last_claim = form.getLastClaimDeck();
            Card previusclaimcard = new Card(last_claim.Deck.GetCounts());

            foreach (CardLabel c_lable in cardLabels)
            {
                if (previusclaimcard.Decrease() == c_lable.Card)
                {
                    c_lable.Selected = true;
                    cards_chosen++;
                    
                }
            }
            if (cards_chosen > 0) return cards_chosen;

            foreach (CardLabel c_lable in cardLabels)
            {
                if (previusclaimcard.Increase() == c_lable.Card)
                {
                    c_lable.Selected = true;
                    cards_chosen++;
                }
            }
            if (cards_chosen > 0) return cards_chosen;

            cards_chosen = 1;
            for(int i = 0; i < cards_chosen; i++)
            {
                int index = random.Next(cardLabels.Count);
                cardLabels[index].Selected = true;
            }
            return cards_chosen;

        }
        protected void chooseRecord(DeckLabel claim)
        {
            string full_file_path = record_dir + claim.m_deck.ToRecordString();
            form.claim_record = new NAudio.Wave.WaveFileReader(full_file_path);

            return;
        }

        void play_move(DeckLabel claim)
        {
            Deck c = new Deck(claim.m_deck);
            Deck act = new Deck();
            foreach (CardLabel c_lable in form.getmyDeck().m_cardLabels)
            {
                if(c_lable.Selected)
                {
                    act.Add(new Card(c_lable.Card));
                }
            }

            session.Add(new TurnHistory(c , act ,form.claim_record , true));
            claim.RaiseSelectionChanged();
        }

        public int getSessionCount()
        {
            int count = 0;
            foreach(TurnHistory turn in session)
            {
                count += turn.getCount();
            }
            return count;
        }


    }
}
