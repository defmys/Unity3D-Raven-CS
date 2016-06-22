using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Unity3DRavenCS
{
	public class RavenStackTrace
	{
		public struct RavenFrame
		{
			public string filename;
            public string function;
			public int lineno;
			public int colno;

			public RavenFrame(StackFrame frame)
			{
				filename = frame.GetFileName();
                function = frame.GetMethod().Name;
				lineno = frame.GetFileLineNumber();
				colno = frame.GetFileColumnNumber();
			}

            public RavenFrame(string filename, string function, int lineno, int colno)
            {
                this.filename = filename;
                this.function = function;
                this.lineno = lineno;
                this.colno = colno;
            }
		}
			
		public List<RavenFrame> frames = new List<RavenFrame>();

		public RavenStackTrace(Exception exception)
		{
			StackTrace stackTrace = new StackTrace(exception, true);
			foreach (var frame in stackTrace.GetFrames()) 
			{
				frames.Add(new RavenFrame(frame));
			}
		}

        public RavenStackTrace(StackTrace stackTrace)
        {
            foreach (var frame in stackTrace.GetFrames())
            {
                frames.Add(new RavenFrame(frame));
            }
        }

        public RavenStackTrace(string stackTrace)
        {
            Regex reg = new Regex(@"([\w\d\._\-\s\(\)]+)\s+\(at\s([\w\d\/\-_\.:]+):([\d]+)\)\n", RegexOptions.IgnoreCase);
            var matches = reg.Matches(stackTrace);

            if (matches.Count > 0)
            {
                BuildFrames(matches, 4);
            }
            else
            {
                reg = new Regex(@"[\s*at\s*]*([\w\d\._\-\s\(\),\`]+?)[\s]*\n", RegexOptions.IgnoreCase);
                matches = reg.Matches(stackTrace);

                BuildFrames(matches, 2);
            }
        }

        private void BuildFrames(MatchCollection matches, int groupCount)
        {
            foreach (Match match in matches)
            {
                if (match.Groups.Count == groupCount)
                {
                    string function = groupCount > 1 ? match.Groups[1].Value : "";
                    string filename = groupCount > 2 ? match.Groups[2].Value : "";
                    int lineno = groupCount > 3 ? Convert.ToInt32(match.Groups[3].Value) : 0;
                    int colno = 0;
                    frames.Add(new RavenFrame(filename, function, lineno, colno));
                }
            }
        }
	}
}