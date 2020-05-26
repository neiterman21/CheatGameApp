using CheatGameApp.Model;
using CheatGameModel.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheatGameApp.Agents
{
    class DumbAgent : Agent
    {
        public DumbAgent(String record_dir_, Form1 form_) : base(record_dir_, form_)
        {

        }

        ~DumbAgent()
        {

        }
        #region decision making
        protected override MoveType decise_move()
        {
            Console.WriteLine("DumbAgent::decise_move()");
            if (board.CallCheatEnable && random.NextDouble() >= 0.7) return MoveType.CallCheat;
            if (board.CallCheatEnable && form.getopponentDeck().m_deck.Count == 0) return MoveType.CallCheat;
            if (haveLeagleMove()) return MoveType.PlayMove;
            if (board.TakeCardEnable && random.NextDouble() >= 0.8) return MoveType.TakeCard;

            return MoveType.PlayMove;
        }

        protected override int chooseCard()
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
            for (int i = 0; i < cards_chosen; i++)
            {
                int index = random.Next(cardLabels.Count);
                cardLabels[index].Selected = true;
            }
            return cards_chosen;

        }
        protected override void chooseRecord(DeckLabel claim)
        {
            string full_file_path = record_dir + claim.m_deck.ToRecordString();
            form.claim_record = new NAudio.Wave.WaveFileReader(full_file_path);

            return;
        }

        protected override DeckLabel chooseClaim()
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

#endregion decision making
    }
}
