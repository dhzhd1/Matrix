using System;
using System.Diagnostics;

namespace MatrixLibrary
{
	public class ClientActions
	{
		Int32 protocolId = -1;
		Type ContentType = null;
		Object Content = null;
		public Type ResultContentType = null;
		public Object ResultContent = null;

		public bool Start(Int32 ProtocolID, Type ContentType, Object Content, out Type ResultContentType, out Object ResultContent){
			this.protocolId = ProtocolID;
			this.ContentType = ContentType;
			this.Content = Content;

			bool result = false;
			switch (ProtocolID) {
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
				result = C_UnknowProtocolId ();
				break;
			}
			ResultContent = this.ResultContent;
			ResultContentType = this.ResultContentType;
			return result;
		}

		// Function naming convention:
		// G_     : the function is call from GUI
		// AP_    : the function is call from Pure Agent (w/o GPU Resource)
		// AM_	  : the function is call from Mixed Mode Agent (w/GPU Resource)
		// AR_    : the function is call from Resource Agent (w/ GPU Resource only)
		// S_     : the function is general purpose

		bool C_UnknowProtocolId(){
			Debug.WriteLine ("Undefined Protocol Id: " + protocolId.ToString());
			return  true;
		}

		bool G_HandShake(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool AC_RegistedCheck(){
			// Agent --> Server --> Agent
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool AC_UpdateStatus(){
			// Agent --> Server --> Agent
			// Send "online, offline, unusable" statue for client agent
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool AC_RequestAssignmentConfig(){
			// Agent --> Server --> Agent
			// request the matrix agent assignment configuration
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}


		bool AR_RequestRcudaServerConfig(){
			// Agent --> Server --> Agent
			//Request latest rCuda Server configuration
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool S_PushAssignment(){
			// Server --> Agent --> Server
			// Push assignment configuration from Matrix Server to Agent
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool AR_UpsertGpuInfo(){
			// Agent --> Server --> Agent
			// upload GPU information
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool AR_UpsertFabricInfo(){
			// Agent --> Server --> Agent
			// upload Fabric Information
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
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
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_SaveClientRcudaServerConfig(){
			//GUI --> Server --> GUI
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RegisterAgent(){
			// GUI --> Server --> GUI
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RequestGpuAssignmentList(){
			// GUI --> Server --> GUI
			// get Gpu Assignment information
			return true;
		}

		bool G_RequestGpuResourceList(){
			// GUI --> Server --> GUI
			// get all GPU information
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RequestFabricResourceList(){
			// GUI --> Server --> GUI
			// get all Fabric information per agent
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RequestRcudaSrvConfig(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RequestRcudaClientConfig(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RequestGpuAssignByClient(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RequestGpuViewList(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_VerifyGpuExclusiveble (){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_SaveRcudaClientConfig(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_ResourceAssign(){
			// GUI --> Server --> GUI
			// change a status of GPU assignment
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RequestClientListByGpu(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RequestGpuXML(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RestartAgent(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_CheckGpuAssignStatus(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
		}

		bool G_RemoveAgent(){
			ResultContent = Content;
			ResultContentType = Content.GetType ();
			return true;
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


