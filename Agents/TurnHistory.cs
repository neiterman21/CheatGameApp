using CheatGameApp.Model;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheatGameApp.Agents
{

    class TurnHistory
    {
        public Deck claim;
        public Deck actual;
        public WaveStream recording;
        public DateTime creation = new DateTime();
        public bool my_move = false;

        public TurnHistory(Deck claim_ = null, Deck actual_ = null, WaveStream recording_ = null , bool my_move_ = false)
        {
            claim = claim_;
            actual = actual_;
            recording = recording_;
            my_move = my_move_;
            creation = DateTime.Now;
        }

        public bool isTrueclaim()
        {
            if (claim.CompareTo(actual)) return true;
            return false;
        }

        public int getCount()
        {
            return claim.Count;
        }

        public override string ToString()
        {
            string str = "creation time: ";
            str += creation + " ";
            str += "Turn has " + getCount() + " Cards. ";
            str += "Claim is: " + claim.ToShortString() + " ";
            str += "Actual is: " + actual.ToShortString() + " ";
            str += "recording " + (recording== null ? "don't exist " : "exists ");
            str += "IsTrueClaim = " + isTrueclaim();
            str += " Is my move = " + my_move;
            return str;
        }
    }
}
