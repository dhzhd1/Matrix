using System;
using MySql;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MatrixLibrary
{
	public class ServerActions
	{
		Int32 protocolId = -1;
		Type ContentType = null;
		Object Content = null;
		public Type ResultContentType = null;
		public Object ResultContent = null;
		Database dbInst = null;

		public ServerActions(){

		}

		public ServerActions(Database dbInstance){
			dbInst = dbInstance;
		}

		public bool Start(Int32 protocolId, Type ContentType, Object Content, out Type ResultContentType, out Object ResultContent){
			this.ContentType = ContentType;
			this.protocolId = protocolId;
			this.Content = Content;

			bool result = false;
			switch (protocolId) {
			case ConstValues.PROTOCOL_FN_GUI_HANDSHAKE_WITH_SERVER:
				result = G_HandShake ();
				break;
			case ConstValues.PROTOCOL_FN_AGENT_AUTHENTICATION_CHECK:
				result = AC_RegistedCheck ();
				break;
			case ConstValues.PROTOCOL_FN_UPDATE_AGENT_STATUS:
				result = AC_UpdateStatus ();
				break;
			case ConstValues.PROTOCOL_FN_REQUEST_RCUDA_SERVER_CONFIG:
				result = AR_RequestRcudaServerConfig ();
				break;
			case ConstValues.PROTOCOL_FN_REQUEST_ASSIGNMENT_CONFIG:
				result = AC_RequestAssignmentConfig();
				break;
			case ConstValues.PROTOCOL_FN_PUSH_NEW_ASSIGNMENT:
				result = S_PushAssignment ();
				break;
			case ConstValues.PROTOCOL_FN_UPLOAD_GPU_INFO:
				result = AR_UpsertGpuInfo ();
				break;
			case ConstValues.PROTOCOL_FN_UPLOAD_FABRIC_INFO:
				result = AR_UpsertFabricInfo ();
				break;
			case ConstValues.PROTOCOL_FN_SAVE_RCUDA_SERVER_CONFIG:
				result = G_SaveClientRcudaServerConfig ();
				break;
			case ConstValues.PROTOCOL_FN_REGISTER_AGENT:
				result = G_RegisterAgent ();
				break;
			case ConstValues.PROTOCOL_FN_GET_AGENT_LIST:
				result = G_RequestAgentList ();
				break;
			case ConstValues.PROTOCOL_FN_GET_GPU_LIST:
				result = G_RequestGpuResourceList ();
				break;
			case ConstValues.PROTOCOL_FN_GET_FABRIC_LIST:
				result = G_RequestFabricResourceList ();
				break;
			case ConstValues.PROTOCOL_FN_GET_RCUDA_SRV_CONFIG:
				result = G_RequestRcudaSrvConfig ();
				break;
			case ConstValues.PROTOCOL_FN_GET_RCUDA_CLIENT_CONFIG:
				result = G_RequestRcudaClientConfig ();
				break;
			case ConstValues.PROTOCOL_FN_GET_AGENT_ASSIGNMENT_LIST:
				result = G_RequestGpuAssignByClient ();
				break;
			case ConstValues.PROTOCOL_FN_GET_GPU_VIEW_LIST:
				result = G_RequestGpuViewList ();
				break;
			case ConstValues.PROTOCOL_FN_VERIFY_GPU_EXCLUSIVE:
				result = G_VerifyGpuExclusiveble ();
				break;
			case ConstValues.PROTOCOL_FN_SAVE_RCUDA_CLIENT_CONFIG:
				result = G_SaveRcudaClientConfig ();
				break;
			case ConstValues.PROTOCOL_FN_SAVE_ASSIGNMENT:
				result = G_ResourceAssign ();
				break;
			case ConstValues.PROTOCOL_FN_GET_AGENT_LIST_BY_GPU:
				result = G_RequestClientListByGpu ();
				break;
			case ConstValues.PROTOCOL_FN_GET_GPU_XML:
				result = G_RequestGpuXML ();
				break;
			case ConstValues.PROTOCOL_FN_RESTART_AGENT:
				result = G_RestartAgent ();
				break;
			case ConstValues.PROTOCOL_FN_CHECK_RESOURCE_IF_ASSIGNED:
				result = G_CheckGpuAssignStatus ();
				break;
			case ConstValues.PROTOCOL_FN_REMOVE_AGENT:
				result = G_RemoveAgent ();
				break;
			default:
				result = S_UnknowProtocolId ();
				break;
			}
			ResultContentType = this.ResultContentType;
			ResultContent = this.ResultContent;
			return result;

		}

		// Function naming convention:
		// G_     : the function is call from GUI
		// AP_    : the function is call from Pure Agent (w/o GPU Resource)
		// AM_	  : the function is call from Mixed Mode Agent (w/GPU Resource)
		// AR_    : the function is call from Resource Agent (w/ GPU Resource only)
		// AC_    : the function is call from all type of Agent
		// S_     : the function is general server function


		bool S_UnknowProtocolId(){
			// any type of command send to Matrix(Server/Agent) and occures a bad protocol id
			ResultContent = "UnknowProtocolId";
			ResultContentType = ResultContent.GetType ();
			return true;
		}

		bool G_HandShake(){
			// GUI --> Server --> GUI
			bool result = false;
			if ((String)Content == "Hello") {
				ResultContent = "Live";
				ResultContentType = ResultContent.GetType ();
				result = true;
			}
			return result;
		}

		bool AC_RegistedCheck(){
			// Agent --> Server --> Agent
			String agentId = (String)Content;
//			Database dbInst = new Database ("157.22.244.236", "root", "P@ssw0rd", "matrix");
//			dbInst.Connect ();
			var result = dbInst.IfAgentRegisted (agentId);
//			#if DEBUG
//			result = true;
//			#endif 
			if (result) {
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
			}
			return result;
		}

		bool AC_UpdateStatus(){
			// Agent --> Server --> Agent
			// Send "online, offline, unusable" statue for client agent
			List<String> paramList = (List<String>)Content;
			var agentId = paramList [0];
			var status = paramList [1];
//			Database dbInst = new Database ("157.22.244.236", "root", "P@ssw0rd", "matrix");
//			dbInst.Connect ();
			var result = dbInst.UpdateAgentStatus (agentId, status);
//			#if DEBUG
//			result = true;
//			#endif
			if (result) {
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
			}
			return result;
		}

		bool AC_RequestAssignmentConfig(){
			// Agent --> Server --> Agent
			// request the matrix agent assignment configuration
			String agentId = (String)Content;
//			Database dbInst = new Database ("157.22.244.236", "root", "P@ssw0rd", "matrix");
//			dbInst.Connect ();
			List<String> configStr;
			dbInst.GetRcudaClientConfig (agentId, out configStr);
//			#if DEBUG
//			configStr = "export LD_LIBRARY_PATH=/home/amax/rCUDAv16.11.05alpha2-CUDA8.0/lib:\nexport RCUDA_DEVICE_COUNT=3\nexport RCUDA_DEVICE_0=157.22.244.230:0\nexport RCUDA_DEVICE_1=157.22.244.230:1\nexport RCUDA_DEVICE_2=157.22.244.230:2\nexport RCUDAPROTO=TCP";
//			#endif
			if (configStr.Count == 0) {
				ResultContent = new List<String> ();
				ResultContentType = ResultContent.GetType ();
				return false;
			} else {
				ResultContent = configStr;
				ResultContentType = configStr.GetType ();
				return true;
			}
		}

		bool AR_RequestRcudaServerConfig(){
			// Agent --> Server --> Agent
			//Request latest assignment configuration
			String agentId = (String)Content;
			List<String> configStr;
			dbInst.GetRcudaServerConfig (agentId, out configStr);
			if (configStr.Count == 0) {
				ResultContent = new List<String> ();
				ResultContentType = ResultContent.GetType ();
				return false;
			} else {
				ResultContent = configStr;
				ResultContentType = configStr.GetType ();
				return true;
			}

		}

		bool S_PushAssignment(){
			// Server --> Agent --> Server
			// If assingment was changed, matrix server will Push new assignment configuration to Agent
			List<String> configList = (List<String>)Content;
			var result = Configuration.rCUDACommand (configList);
			//var result = false;
			if (result) {
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
				return true;
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
				return false;
			}

		}

		bool AR_UpsertGpuInfo(){
			// Agent --> Server --> Agent
			// upload GPU information
			List<Dictionary<String,String>> gpuInfoList = (List<Dictionary<String,String>>)Content;
//			Database dbInst = new Database ("157.22.244.236", "root", "P@ssw0rd", "matrix");
//			dbInst.Connect ();
			bool result = dbInst.UpsertGpuInfo (gpuInfoList);
			if (result) {
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
				return true;
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
				return false;
			}
		}

		bool AR_UpsertFabricInfo(){
			// Agent --> Server --> Agent
			// upload Fabric Information
			List<Dictionary<String,String>> fabricInfoList = (List<Dictionary<String,String>>)Content;
//			Database dbInst = new Database ("157.22.244.236", "root", "P@ssw0rd", "matrix");
//			dbInst.Connect ();
			bool result = dbInst.UpsertFabricInfo (fabricInfoList);
			if (result) {
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
				return true;
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
				return false;
			}
		}

		bool S_RequestGpuInfo(){
			// Server --> Agnet --> Server
			// ask gpu information from agent
			return true;
		}

		bool S_RequestFabircInfo(){
			// Server --> Agent --> Server
			// ask fabiric Information from agent
			return true;
		}

		bool AM_UpsertGpuInfo(){
			// Agent --> Server --> Agent
			// upload GPU Info
			return true;
		}

		bool AM_UpsertFabricInfo(){
			// Agent --> Server --> Agent
			// upload Fabric Info
			return true;
		}

		bool G_RequestAgentList(){
			// GUI --> Server --> GUI
			// get all of agent List for GUI 
			String agentId = (String)Content;
			List<Dictionary<String,String>> agentList = new List<Dictionary<string, string>> ();
			if (agentId == "all-agents") {
				agentList = dbInst.GetAgentList ();
				ResultContent = agentList;
				ResultContentType = agentList.GetType ();
			} else {
				ResultContent = new List<Dictionary<String,String>> ();
				ResultContentType = ResultContent.GetType ();
			}
			return true;
		}

		bool G_SaveClientRcudaServerConfig(){
			// GUI --> Server --> GUI
			List<String> rCudaSrvConfig = (List<String>)Content;
			String agentId = rCudaSrvConfig [0].Split ('=') [1];
			bool result = dbInst.SaveRcudaSrvConfig (agentId,rCudaSrvConfig);
			if (result) {
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
				return true;
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
				return false;
			}
		}

		bool G_RegisterAgent(){
			// GUI --> Server --> GUI
			Dictionary<String, String> agentInfo = (Dictionary<String,String>)Content;
			bool result = dbInst.RegistAgent (agentInfo);
			if (result) {
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
				return true;
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
				return false;
			}
		}

		bool G_RequestGpuAssignmentList(){
			// GUI --> Server --> GUI
			// get Gpu Assignment information
			return true;
		}

		bool G_RequestGpuResourceList(){
			// GUI --> Server --> GUI
			// get all GPU information
			String agentId = (String)Content;
			List<Dictionary<String,String>> result = new List<Dictionary<string, string>> ();
			result = dbInst.GetGpuResourceList (agentId);
			ResultContent = result;
			ResultContentType = result.GetType ();
			return true;
		}

		bool G_RequestFabricResourceList(){
			//GUI --> Server --> GUI
			String agentId = (String)Content;
			List<Dictionary<String,String>> result = new List<Dictionary<string, string>> ();
			result = dbInst.GetFabricResourceList (agentId);
			ResultContent = result;
			ResultContentType = result.GetType ();
			return true;
		}

		bool G_RequestRcudaSrvConfig(){
			// GUI --> Server --> GUI
			return AR_RequestRcudaServerConfig();
		}

		bool G_RequestRcudaClientConfig(){
			//GUI --> Server --> GUI
			return AC_RequestAssignmentConfig();
		}

		bool G_ResourceAssign(){
			// GUI --> Server --> GUI
			// change a status of GPU assignment
			List<Dictionary<String,String>> assignmentChanges = (List<Dictionary<String,String>> )Content;
			bool result = dbInst.UpdateGpuAssignment (assignmentChanges);
			if (result) {
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
			}
			return true;
		}

		bool G_SaveRcudaClientConfig(){
			List<String> rCudaClientConfig = (List<String>)Content;
			String agentId = rCudaClientConfig [0].Split ('=') [1];
			bool result = dbInst.SaveRcudaClientConfig (agentId,rCudaClientConfig);
			if (result) {
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
			}
			return true;
		}

		bool G_RequestGpuAssignByClient(){
			// GUI --> Server --> GUI
			// Get GPU List by search an agentID
			String agentId = (String)Content;
			List<Dictionary<String,String>> assignedGpuList = new List<Dictionary<string, string>> ();
			assignedGpuList = dbInst.GetAssignedGpuByClient (agentId);
			ResultContent = assignedGpuList;
			ResultContentType = assignedGpuList.GetType ();
			return true;
		}

		bool G_RequestGpuViewList(){
			// GUI --> Server --> GUI
			// This function will retrieve the gpu list with some extra information which differernt gpu info list that gpu info by client id.
			String contentStr = (String)Content;
			List<Dictionary<String,String>> result = new List<Dictionary<string, string>> ();
			if (contentStr == "all-gpus") {
				result = dbInst.GetGpuViewList ();
			}
			ResultContent = result;
			ResultContentType = result.GetType ();
			return true;
		}

		bool G_VerifyGpuExclusiveble(){
			// GUI --> Server --> GUI
			// check if the gpu could be set as an exclusive resource
			String gpuUUID = (String)Content;
			bool result = dbInst.CheckGpuExclusiveable (gpuUUID);
			if (result) {
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
			}
			return true;
		}

		bool G_RequestClientListByGpu(){
			// GUI --> Server --> GUI
			// query client information by using gpu UUID
			String gpuUUID = (String)Content;
			List<Dictionary<String, String>> result = new List<Dictionary<string, string>> ();
			result = dbInst.GetClientListByGpu (gpuUUID);
			ResultContent = result;
			ResultContentType = result.GetType ();
			return true;
		}

		bool G_RequestGpuXML(){
			String gpuUUID = (String)Content;
			String result = dbInst.GetGpuXmlInfo (gpuUUID);
			ResultContent = result;
			ResultContentType = result.GetType ();
			return true;
		}

		bool G_RestartAgent(){
			SystemCall restartAgent = new SystemCall ();
			restartAgent.CommandText = @"/bin/bash";
			restartAgent.Parameters = @"/etc/init.d/matrix-agent restart";
			restartAgent.EnableStandOutput = false;
			restartAgent.EnableStandError = false;
			restartAgent.CommandExecute ();
			return true;
		}

		bool G_CheckGpuAssignStatus(){
			String agentId = (String)Content;
			bool result = dbInst.CheckGpuAssignStatus (agentId);
			if (result) {
				// Means some gpu has been assinged to other client
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
			}
			return  true;
		}

		bool G_RemoveAgent(){
			String agentId = (String)Content;
			bool result = dbInst.RemoveAgent (agentId);
			if (result) {
				// remove agent successed.
				ResultContent = "True";
				ResultContentType = ResultContent.GetType ();
			} else {
				ResultContent = "False";
				ResultContentType = ResultContent.GetType ();
			}
			return  true;
		}

		bool G_CollectDbgLog(){
			// GUI --> Server --> GUI
			// get debug log from agent to GUI
			return true;
		}

		bool S_GetDbgLog(){
			// Server --> Agent --> Server
			// get debug log from Agent
			return true;
		}

		bool S_GetRuntimeStatus(){
			// Server --> Agent --> Server
			// get all of agent runtime status
			// detail information will be redifined. They will be used on the dashboard
			return true;
		}

		bool G_GetResourceUtilizationData(){
			// GUI --> Server --> GUI
			// Get resource utilization data
			return true;
		}

		bool S_GetResourceUtilizationData(){
			// Server --> Agent --> Server
			// Get hw resource utilization data and import to a temp database
			// matric: cpu, gpu, fabric bandwidth etc.
			return true;
		}

		bool S_ForceReloadAgent(){
			// Server --> Agent --> Server
			// Force agent reload the configuration and reload the rCuda Agent
			return true;
		}

		bool S_ForceShutdownAgent(){
			// Server --> Agent --> Server
			// Force agent shutdown
			return true;
		}

		bool AR_UploadFilter(){
			// Agent --> Server --> Agent
			// This will be a furture feature
			// upload the filter information which will limite the resource to share
			return true;
		}

		bool AM_UploadFilter(){
			// Agent --> Server --> Agent
			// This will be a furture feature
			// upload the filter information which will limite the resource to share
			return true;
		}

		bool S_RemoveAgent(){
			// Server --> Agent --> Server
			// Remove an agent from database
			return true;
		}


	}
}

