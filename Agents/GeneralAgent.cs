using CheatGameApp.Agents;
using CheatGameApp.Model;
using CheatGameModel.Network.Messages;
using CheatGameModel.Players;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CheatGameApp.Agents
{
    abstract class Agent
    {
        protected String record_dir;
        protected BoardState board;
        protected bool board_valid = false;
        protected WaveStream claim = null;
        protected bool claim_valid = false;
        protected Form1 form = null;
        public Random random = new Random();
        public bool first_move  = false;
        protected Demographics demografic_form = new Demographics();
        protected List<TurnHistory> session = new List<TurnHistory>();


        public Agent(String record_dir_ , Form1 form_)
        {
            record_dir = record_dir_;
            form = form_;
        }
        ~Agent()
        {

        }

        #region game orepation and logging

        public Demographics fill_demographic_form()
        {

            demografic_form.Age = 1;
            demografic_form.FullName = "Bob";
            demografic_form.Gender = CheatGameModel.Players.Enumarations.Genders.Male;
            demografic_form.EducationType = EducationType.BSc;
            demografic_form.EducationField = EducationFields.ComputerSciense;
            return demografic_form;
        }

        public void onCardsRevel(Deck revel_deck)
        {
            revel_deck.Reverse();
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

            if (!form.is_agent) return;

            Debug.WriteLine("got new recording at: " + DateTime.Now);
            if (session.Count == 0 || session[session.Count - 1].recording != null)
                session.Add(new TurnHistory());
            else if(session[session.Count - 1].recording == null && session[session.Count - 1].actual != null)
                session.Add(new TurnHistory());
            session[session.Count - 1].recording = new RawSourceWaveStream(_claim, new WaveFormat(16000, 1));
        }

        public void setFirstCard(Deck first)
        {
            if (session.Count > 0) return;
            Debug.WriteLine("adding first card");
            session.Add(new TurnHistory(new Deck(first), new Deck(first),null, false));
        }

        public void updateBoardState(BoardState _board)
        {
            board = _board;
            board_valid = true;

            if (!form.is_agent) return;

            if (!form.getStartGameButton().Visible && !board.AgentStartPressed) return; //end game
            if (session.Count == 0) return;
            else if (session[session.Count - 1].claim != null && session[session.Count - 1].claim.CompareTo(form.getLastClaimDeck().m_deck)) return;
            else if (session[session.Count - 1].claim != null) session.Add(new TurnHistory(new Deck(form.getLastClaimDeck().m_deck)));
            else session[session.Count - 1].claim = new Deck(form.getLastClaimDeck().m_deck);

            if (getSessionCount() != _board.PlayedCardsNum)
            {
                throw  new System.ArgumentException("extra cards added session count = " + getSessionCount() + " played cards num = " + _board.PlayedCardsNum); 
            }
        }

        public void gameEnd()
        {
            session.Clear();
            first_move = false;
            board_valid = false;
            claim_valid = false;
            Debug.WriteLine("agent endgame");
        }

        private bool shouldWait()
        {
            if (!form.is_agent) return true;
            if (form.getStartGameButton().Enabled)
            {   first_move = true;
                board_valid = false;
                claim_valid = false;
                Console.WriteLine("agent clicking start game");
                form.StartGameButton_Click(this, null);
                return true;
            }
            if (board_valid == false) return true;
            if (board.IsServerTurn) return true;
            if (!form.getStartGameButton().Visible && !board.AgentStartPressed) return true; //end game
            if (form.getmyDeck().m_deck.Count == 0) return true;

            if (first_move) return false;
            if (!board.CanDispute) return false;
            if (!claim_valid && !board.BoardMsg.Contains("cards from the unused stack")) return true;
           
            return false;
        }

        private void makeMove()
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
            int wait = random.Next(7, 13000);
            Form1.DelayAction(3000, new Action(() => { this.makeMove(); }));
        }

        private void play_move(DeckLabel claim)
        {
            if (shouldWait()) return;
            Deck c = new Deck(claim.m_deck);
            Deck act = new Deck();
            foreach (CardLabel c_lable in form.getmyDeck().m_cardLabels)
            {
                if (c_lable.Selected)
                {
                    act.Add(new Card(c_lable.Card));
                }
            }

            session.Add(new TurnHistory(c, act, form.claim_record, true));
            claim.RaiseSelectionChanged();
        }

        #endregion game orepation and logging

        #region game info
        protected int getSessionCount()
        {
            int count = 0;
            foreach (TurnHistory turn in session)
            {
                if (turn != null)
                    count += turn.getCount();
            }
            return count;
        }


        protected bool haveLeagleMove()
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

        #endregion #region game info

        #region decision making 

        protected abstract  MoveType decise_move();


        protected abstract int chooseCard();

        protected abstract void chooseRecord(DeckLabel claim);
 

    protected abstract DeckLabel chooseClaim();

        #endregion decision making

        

        


    }
}
