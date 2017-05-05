using System;
using System.Net.Mime;
using System.Net.Sockets;
using MatrixLibrary;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;


namespace MatrixLibrary
{
	public class Protocol
	{
		public object SendContent = null;
		public Type SendContentType = null;
		public Int32 ProtocolID = -1;
		public object ReceivedContent = null;
		public Type ReceivedContentType = null;
		public TcpClient Client = null;
		public NetworkStream ns = null;
		public object ResultObject = null;
		public Type ResultObjectType = null;
		bool flag = false;
		public Database dbInst = null;

		public Protocol (TcpClient client, Int32 protocolId, Type objType, Object obj)
		{
			ProtocolID = protocolId;
			SendContentType = objType;
			SendContent = obj;
			Client = client;
			ns = Client.GetStream ();
			flag = true;

		}

		public Protocol (TcpClient client){
			this.Client = client;
			ns = client.GetStream ();

		}

		virtual public string BuildCommunicationString (Type contentType, object content){
			String commStr = ProtocolID.ToString () + ":";
			commStr += Utility.EncodingBase64String (contentType.ToString ()) + ":";
			commStr += Utility.EncodingBase64String (Utility.SerializeParams (contentType, content));
			Debug.WriteLine (commStr);
			MatrixLibrary.log.Debug (commStr);
			return commStr;
		}

		virtual public void Send(string commStr){
			try {
				byte[] commStr_byte = Encoding.ASCII.GetBytes (commStr);
				ns.Write (commStr_byte, 0, commStr_byte.Length);
				MatrixLibrary.log.Debug("Send: "+ commStr); 
			} catch (Exception ex) {
				Debug.WriteLine (ex.Message);
				MatrixLibrary.log.Debug (ex.Message);
			}
		}

		virtual public string Receive(){
			byte[] receive_buffer = new byte[4096];
			String receivedStr = string.Empty;
			int count = 0;
			using (MemoryStream ms = new MemoryStream ()) {
				if (ns.CanRead)
					do {
						count = ns.Read(receive_buffer, 0, receive_buffer.Length);
						ms.Write(receive_buffer, 0, count);
						Thread.Sleep(50);
					} while(ns.DataAvailable);
				receivedStr = Encoding.ASCII.GetString (ms.ToArray (), 0, (int)ms.Length);
			}
//			if (ns.CanRead) {
//				do {
//					count = ns.Read (receive_buffer, 0, receive_buffer.Length);
//					receivedStr += Encoding.ASCII.GetString (receive_buffer, 0, count);
//				} while (ns.DataAvailable);
//			}
			Debug.WriteLine (receivedStr);
			MatrixLibrary.log.Debug ("Received: " + receivedStr);
			return receivedStr;
		}

		virtual public void Start(){
			try {
				if (flag) {
					// Run Client Mode
					Send (BuildCommunicationString (SendContentType, SendContent));
					string recvStr = Receive();
					//ParseReceivedString (Receive ());
					ParseReceivedString(recvStr);
					new ClientActions ().Start (ProtocolID, ReceivedContentType, ReceivedContent, out ResultObjectType, out ResultObject);
				} else {
					// Run Server Mode
					ParseReceivedString (Receive ());
					if (dbInst == null) {
						new ServerActions ().Start (ProtocolID, ReceivedContentType, ReceivedContent, out SendContentType, out SendContent);
					} else {
						new ServerActions (dbInst).Start (ProtocolID, ReceivedContentType, ReceivedContent, out SendContentType, out SendContent);
					}
				
					Send (BuildCommunicationString (SendContentType, SendContent));
				}
			} catch (Exception ex) {
				MatrixLibrary.log.Error (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
			}
		}

		virtual public void ParseReceivedString(string receivedString){
			try {
				string[] splitedStr = receivedString.Split (':');
				ProtocolID = Int32.Parse (splitedStr [0]);
				ReceivedContentType = Type.GetType (Utility.DecodingBase64String (splitedStr [1]));
				ReceivedContent = (object)Utility.DeserializeParams (ReceivedContentType, Utility.DecodingBase64String (splitedStr [2]));
			} catch (Exception ex) {
				MatrixLibrary.log.Error (ex.Message);
				MatrixLibrary.log.Debug (ex.StackTrace);
			}

		}


	}
}


