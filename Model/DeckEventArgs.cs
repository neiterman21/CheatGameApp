using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheatGameApp.Model
{
    public class DeckEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the selected deck
        /// </summary>
        public Deck SelectedDeck { get; private set; }
        public DeckEventArgs(Deck deck)
        {
            this.SelectedDeck = deck;
        }
    }
}
