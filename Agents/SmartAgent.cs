using CheatGameModel.Network.Messages;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CheatGameApp.Agents
{
    class SmartAgent : DumbAgent
    {
        protected string claim_dir = "";
        DirectoryInfo di;
        PythonInterface pyapi = new PythonInterface();
        int audioIndex = 0;
        public SmartAgent(String record_dir_, Form1 form_) : base(record_dir_, form_)
        {
            claim_dir = @"C:\Users\Administrator\Desktop\agent\smartAgent\Resources\AgentRec" + DateTime.Now.ToFileTime();
            Console.WriteLine(claim_dir);
            di = Directory.CreateDirectory(claim_dir);
        }
         ~SmartAgent()
        {
            di.Delete(true);
        }
        protected override MoveType decise_move()
        {
            Console.WriteLine("SmartAgent::decise_move()");
            if (board.CallCheatEnable && isClaimLie()) return MoveType.CallCheat;
            if (board.CallCheatEnable && form.getopponentDeck().m_deck.Count == 0) return MoveType.CallCheat;
            if (haveLeagleMove()) return MoveType.PlayMove;
            if (board.TakeCardEnable && random.NextDouble() >= 0.8) return MoveType.TakeCard;

            return MoveType.PlayMove;
        }

        protected bool isClaimLie()
        {
            return isClaimLie(claim);
        }

        protected string saveClaim(WaveStream claim)
        {
            string str = string.Format("audio_{0:0000}.wav", (object)audioIndex);
            string filename = string.Format("{0}\\{1}", (object)claim_dir, (object)str);
            Console.WriteLine("saving audio file to " + filename);
            WaveFileWriter.CreateWaveFile(filename, claim);
            audioIndex++;
            return filename;
        }

        public bool isClaimLie(WaveStream claim)
        {

            string path = saveClaim(claim);
            string[] args = { path };
            int rc = pyapi.run(args);
            Console.WriteLine("exit code = " + rc);

            if (rc == 1) return true; //detected a lie
            return false;
        }
    }
}
