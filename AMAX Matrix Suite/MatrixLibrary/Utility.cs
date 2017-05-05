using System;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;

namespace MatrixLibrary
{
	static public class Utility
	{
		static public String SerializeParams (Type t, Object o)
		{
			StringBuilder xmlStr = new StringBuilder ();
			using (XmlWriter xmlWrite = XmlWriter.Create (xmlStr)) {
				DataContractSerializer serializer = new DataContractSerializer (t);
				serializer.WriteObject (xmlWrite, o);
			}
			Debug.WriteLine (xmlStr.ToString ());
			return xmlStr.ToString ();
		}

		static public Object DeserializeParams (Type t, String serializedStr){
			Object deserializedObj = new object ();
			using (XmlReader reader = XmlReader.Create (new StringReader (serializedStr))) {
				DataContractSerializer deserializer = new DataContractSerializer (t);
				deserializedObj = deserializer.ReadObject (reader);
			}
			Debug.WriteLine (deserializedObj.ToString ());
			return deserializedObj;
			// for list/dictionary etc, you need to cast this return value to (IEnumerable) frist before using for/foreach)
		}

		static public String EncodingBase64String(String originalStr){
			byte[] byteArray = Encoding.ASCII.GetBytes (originalStr);
			String encodedStr = Convert.ToBase64String (byteArray);
			Debug.WriteLine (encodedStr);
			return encodedStr;
		}

		static public String DecodingBase64String(String base64Str){
			String decodedStr = Encoding.ASCII.GetString(Convert.FromBase64String (base64Str));
			Debug.WriteLine (decodedStr.ToString());
			return decodedStr.ToString();
		}

		static public Boolean FileExist(String fileFullPath){
			if (File.Exists (fileFullPath)) {
				return true;
			} else {
				return false;
			}
		}

		static public Boolean FolderExist(String folderPath){
			if (Directory.Exists (folderPath)) {
				return true;
			} else {
				return false;
			}
		}

		static public String GenerateUUID (){
			return Guid.NewGuid ().ToString ();
		}
	}
}
