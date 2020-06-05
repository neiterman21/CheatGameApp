using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CheatGameApp.Agents
{
    class PythonInterface
    {
        static string py_interpeter = @"C:\Users\Administrator\AppData\Local\Programs\Python\Python37\python.exe";
        public ProcessStartInfo psi = new ProcessStartInfo();
        public string default_script = @"C:\Users\Administrator\Desktop\agent\cheat_detector_model\evaluate.py";

        public PythonInterface()
        {
            psi.FileName = py_interpeter;
            psi.Arguments = $"\"{default_script}\"";
            //process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
        }

        public int run()
        {
            return run(new string[0]);
        }

        public int run(string[] args)
        {
            return run(default_script, args);
        }

        public int run(string script_ , string[] args)
        {
            var error = "";
            var result = "";
            int exit_code = -1;

            psi.Arguments = $"\"{script_}\"";
            foreach (string arg in args)
            {
                psi.Arguments += $" \"{arg}\"";
            }

            Console.WriteLine("args: " + psi.Arguments);



            using (var process = Process.Start(psi))
            {
                error = process.StandardError.ReadToEnd();
                result = process.StandardOutput.ReadToEnd();
                exit_code = process.ExitCode;
            }
            if(exit_code > 1)
            {
                Console.WriteLine("output:");
                Console.WriteLine(result);
                Console.WriteLine("error:");
                Console.WriteLine(error);
            }
          

            return exit_code;
        }
    }
}
