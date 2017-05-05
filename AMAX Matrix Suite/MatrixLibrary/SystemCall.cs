//
//  Copyright 2017  AMAX Information Technologies, Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;



namespace MatrixLibrary
{
	public class SystemCall
	{
		public String CommandText { get; set; }
		public String Parameters { get; set; }
		public String StandOutput { get; set;}
		public String StandError { get; set;}
		public Boolean EnableStandError{ get; set; } = true;
		public Boolean EnableStandOutput{ get; set; } = true;
		public Boolean EnableUseShellExecute  { get; set; } = false;

		public SystemCall ()
		{

		}

		public Boolean CommandExecute()
		{
			StringBuilder sbStdOut = new StringBuilder ();
			StringBuilder sbStdErr = new StringBuilder ();
			Process sysCall = new Process ();
			sysCall.StartInfo.RedirectStandardError = EnableStandError;
			sysCall.StartInfo.RedirectStandardOutput = EnableStandOutput;
			sysCall.StartInfo.UseShellExecute = EnableUseShellExecute;
			sysCall.StartInfo.FileName = CommandText;
			sysCall.StartInfo.Arguments = Parameters;
			try {
				sysCall.Start ();
				while (!sysCall.HasExited) {
					sbStdOut.Append(sysCall.StandardOutput.ReadToEnd());
					sbStdErr.Append(sysCall.StandardError.ReadToEnd());
				}
				StandOutput = sbStdOut.ToString();
				StandError = sbStdErr.ToString();
				return true;
			} catch (Exception ex) {
				MatrixLibrary.log.Error ("Exception", ex);
				MatrixLibrary.log.Debug (String.Format(ConstValues.MSG_ERR_SYSTEM_CALL_FAILED, CommandText, Parameters));
				return false;
			}

		}
	}
}
