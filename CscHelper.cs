using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace CscHelper
{
    class CscHelper
    {
        const string NAME = "cschelper.exe";
         
        static void Main(string[] args)
        {
            try
            {
                Excecute(args);
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            
        }

        static void Excecute(string[] args)
        {
            var op = ExcecuteOption.FromArgs(args);
            var info = new ProcessStartInfo(op.CompilePath,op.CsArgs);
            info.UseShellExecute = false;
            Process.Start(info);

            if(op.OutputBat == null)
            {
                return;
            }

            var enc = Encoding.GetEncoding("Shift_JIS");
            using(var sw = new StreamWriter(op.OutputBat, false, enc))
            {
                sw.WriteLine(NAME + " " + string.Join(" ", args));
                sw.WriteLine("pause");
            }
        }
    }

    class ExcecuteOption
    {
        readonly string _CompilePath;
        readonly string _Directory;
        readonly string _ExeType;
        readonly string _ExeName;
        readonly string _OutputBat;

        public string CsArgs
        {
            get
            {
                var files = Directory.GetFiles(_Directory, "*.cs", SearchOption.AllDirectories);
                return "/out:" + _ExeName + " /target:" + _ExeType + " " + string.Join(" ", files);
            }
        }

        public string CompilePath
        {
            get
            {
                return _CompilePath;
            }
        }

        public string OutputBat
        {
            get
            {
                return _OutputBat;
            }
        }

        public ExcecuteOption(string cmpPth, string dir,string exeType,string name,string bat)
        {
            _CompilePath = cmpPth;
            _Directory = dir;
            _ExeType = exeType;
            _ExeName = name;
            _OutputBat = bat;
        }

        public static ExcecuteOption FromArgs(string[] args)
        {
            if(args.Length < 4)
            {
                throw new ArgumentException("arg err");
            }
            return new ExcecuteOption(args[0],args[1],args[2],args[3],args.Length == 4 ? null: args[4]);
        }
        
    }
}